using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Minigame : MonoBehaviour
{
    private TimeSystem timeSystem;
    private bool minigameActive = false; // Check if the minigame is active
    private Coroutine popupTimeoutCoroutine; // Reference to the timeout coroutine

    [SerializeField] private GameObject minigameGameObject; // The UI popup with TextMeshPro
    [SerializeField] private GameObject TextObject; // Reference to the Text child object
    private TextMeshProUGUI textMeshPro;

    [SerializeField] private int[] activationTimes; // Array of times (seconds left) for minigame activations
    private HashSet<int> remainingActivations; // Tracks remaining activation times to prevent duplicates

    void Start()
    {
        // Reference the TimeSystem component
        timeSystem = FindObjectOfType<TimeSystem>();

        // Ensure the minigame popup is initially hidden
        if (minigameGameObject != null && TextObject != null)
        {
            minigameGameObject.SetActive(false);

            // Fetch the TextMeshPro component from the "Text" child
            textMeshPro = TextObject.GetComponent<TextMeshProUGUI>();
            if (textMeshPro == null)
            {
                Debug.LogError("TextMeshPro component not found in the 'Text' child of Minigame popup!");
            }
        }
        else
        {
            Debug.LogError("Minigame popup GameObject or TextObject is not assigned!");
        }

        // Initialize the activation times as a set for efficient checks
        remainingActivations = new HashSet<int>(activationTimes);
    }

    private void Update()
    {
        if (timeSystem != null && remainingActivations.Count > 0)
        {
            int totalSeconds = (int)timeSystem.remainingTime.TotalSeconds;

            if (remainingActivations.Contains(totalSeconds))
            {
                TriggerPopup();
                remainingActivations.Remove(totalSeconds); // Remove the time to prevent re-triggering
            }
        }

        // Check for "M" press to activate the minigame
        if (minigameGameObject.activeSelf && Input.GetKeyDown(KeyCode.M) && !minigameActive)
        {
            ActivateMinigame();
        }

        // Detect user input during the minigame
        if (minigameActive)
        {
            DetectUserInput();
        }
    }
    
    private void DetectUserInput()
    {
        if (!isInputAllowed || keySequence == null || keySequence.Count == 0)
        {
            return; // Skip input detection if not allowed or sequence is empty
        }

        // Get the key at the current index
        string expectedKey = keySequence[currentKeyIndex];

        // Check if the user presses the expected key
        if (Input.GetKeyDown(expectedKey.ToLower()))
        {
            currentKeyIndex++; // Progress to the next key
            UpdateProgressDisplay(); // Update progress after each correct key press

            // Check if the user has completed the sequence
            if (currentKeyIndex >= keySequence.Count)
            {
                MinigameSuccess();
            }
        }
        else if (Input.anyKeyDown) // Handle incorrect input
        {
            MinigameFailure("Incorrect sequence! Minigame has ended.");
        }
    }
    
    private void UpdateProgressDisplay()
    {
        if (!minigameActive || textMeshPro == null) return;
        textMeshPro.text = $"Enter the keys in order.\nProgress: {currentKeyIndex}/{keySequence.Count}\nTime Left: {Mathf.CeilToInt(currentTimeRemaining)}s";
    }
    
    private void MinigameSuccess()
    {
        minigameActive = false; // Mark the minigame as inactive

        if (textMeshPro != null)
        {
            textMeshPro.text = "Correct sequence! Destroying one of the defender's towers."; // Show success message
        }

        StopAllCoroutines(); // Stop all coroutines to prevent further progress or countdown updates
        StartCoroutine(DestroyEnemyTowerAfterDelay(5f)); // Wait for 5 seconds before disabling the popup and applying the effect
    }

    private void MinigameFailure(String message)
    {
        minigameActive = false; // Mark the minigame as inactive

        if (textMeshPro != null)
        {
            textMeshPro.text = message; // Show failure message
        }

        StopAllCoroutines(); // Stop all coroutines to prevent further progress or countdown updates
        StartCoroutine(DisablePopupAfterDelay(10f)); // Wait for 10 seconds before disabling the popup
    }

    
    private IEnumerator InputCountdown()
    {
        while (minigameActive && currentTimeRemaining > 0)
        {
            yield return new WaitForSeconds(1f);
            currentTimeRemaining--;

            // Update the progress display with the remaining time
            UpdateProgressDisplay();
        }

        // If the timer runs out, fail the minigame
        if (currentTimeRemaining <= 0 && minigameActive)
        {
            MinigameFailure("Time ran out! Minigame has ended.");
        }
    }
    
    private IEnumerator DisablePopupAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (minigameGameObject != null)
        {
            minigameGameObject.SetActive(false); // Disable the popup
            textMeshPro.text = ""; // Clear the text
        }
    }
    
    private IEnumerator DestroyEnemyTowerAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (minigameGameObject != null)
        {
            minigameGameObject.SetActive(false); // Disable the popup
            textMeshPro.text = ""; // Clear the text
        }

        // Add logic to destroy one of the enemy's towers
        Debug.Log("Enemy tower destroyed!");
        DestroyRandomTower();
    }
    
    private void DestroyRandomTower()
    {
        if (Plot.plotsWithTowers.Count == 0)
        {
            Debug.Log("No towers to destroy!");
            return;
        }

        // Select a random plot with a tower
        int randomIndex = UnityEngine.Random.Range(0, Plot.plotsWithTowers.Count);
        Plot randomPlot = Plot.plotsWithTowers[randomIndex];

        // Destroy the tower and update resources
        randomPlot.DestroyTower();
        Debug.Log("A tower has been destroyed!");
    }



    private void TriggerPopup()
    {
        if (minigameGameObject != null)
        {
            // Set the popup text to the activation message
            if (textMeshPro != null)
            {
                textMeshPro.text = "Press M to activate minigame!";
            }

            minigameGameObject.SetActive(true); // Show the popup
            popupTimeoutCoroutine = StartCoroutine(HidePopupAfterDelay(10f)); // Start the timeout coroutine
        }
    }

    private List<string> keySequence; // Stores the generated key sequence
    private int currentKeyIndex; // Tracks the user's progress in the sequence
    private bool isInputAllowed = false; // Determines if user input is allowed
    private float inputTimer = 13f; // Time limit for the user to complete the sequence
    private float currentTimeRemaining; // Tracks the remaining time for the user
    
    private void ActivateMinigame()
    {
        minigameActive = true; // Mark the minigame as active
        isInputAllowed = false; // Disable input detection initially
        currentTimeRemaining = inputTimer; // Reset the timer for user input

        if (popupTimeoutCoroutine != null)
        {
            StopCoroutine(popupTimeoutCoroutine); // Cancel the timeout coroutine if "M" is pressed
        }

        if (minigameGameObject != null)
        {
            // Initialize user input tracking
            keySequence = GenerateKeySequence(6); // Store the generated key sequence
            currentKeyIndex = 0; // Reset user's progress
            Debug.Log("Minigame activated! Keys to match: " + string.Join(" ", keySequence));

            StartCoroutine(DisplayRandomKeysWithTimer(13f)); // Display random keys with countdown timer
        }
    }
    
    private List<string> GenerateKeySequence(int count)
    {
        string[] possibleKeys =
        {
            // Skipping A, S, D & 1, 2 since they have corresponding in-game functionality
            "b", "c", "e", "f", "g", "h", "i", "j", "k", "l",
            "m", "n", "o", "p", "q", "r", "t", "u", "v", "w",
            "x", "y", "z", "0", "3", "4", "5", "6", "7", "8", "9"
        };

        System.Random random = new System.Random(); // Random number generator
        List<string> sequence = new List<string>();

        // Generate the sequence
        for (int i = 0; i < count; i++)
        {
            string randomKey = possibleKeys[random.Next(possibleKeys.Length)];
            sequence.Add(randomKey); // Add the random key to the sequence
        }

        return sequence;
    }


    private IEnumerator DisplayRandomKeysWithTimer(float duration)
    {
        if (textMeshPro != null)
        {
            float timeRemaining = duration;
            string randomKeys = string.Join(" ", keySequence); // Convert the list to a space-separated string

            while (timeRemaining > 0)
            {
                textMeshPro.text = $"Remember this key combination!\n{randomKeys}\nTime Left: {Mathf.CeilToInt(timeRemaining)}s"; // Update text with timer
                yield return new WaitForSeconds(1f); // Wait for 1 second
                timeRemaining -= 1f;
            }

            textMeshPro.text = ""; // Clear the text when the timer ends
        }

        isInputAllowed = true; // Enable input detection after sequence display
        StartCoroutine(InputCountdown()); // Start the countdown timer for user input
    }


    private IEnumerator HidePopupAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (!minigameActive && minigameGameObject != null)
        {
            minigameGameObject.SetActive(false); // Hide the popup if "M" is not pressed
        }
    }
}
