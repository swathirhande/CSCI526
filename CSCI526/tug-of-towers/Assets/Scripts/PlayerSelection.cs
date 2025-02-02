using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PlayerSelection : MonoBehaviour
{
    // References to the input fields for player names
    public TMP_InputField attackerNameInput;
    public TMP_InputField defenderNameInput;

    // Reference to the Play button
    public Button playButton;

    public Button mainMenuButton;

    // Start is called before the first frame update
    void Start()
    {
        // Add listener to play button to check input and change scene
        playButton.onClick.AddListener(OnPlayButtonClicked);
    }

    // Function called when Play button is clicked
    public void OnPlayButtonClicked()
    {
        // Get text from input fields
        string attackerName = attackerNameInput.text.Trim(); // Trim to remove leading/trailing spaces
        string defenderName = defenderNameInput.text.Trim();

        // Check if both names are entered
        if (!string.IsNullOrEmpty(attackerName) && !string.IsNullOrEmpty(defenderName))
        {
            // Store names in GameVariables (or directly pass them to the main game scene)
            GameVariables.attackerName = attackerName;
            GameVariables.defenderName = defenderName;

            // Load the chosen level
            SceneManager.LoadScene(PlayerPrefs.GetString("SelectedLevel"));
        }
        else
        {
            // Optionally, you can show a message to the player if they haven't entered both names
            Debug.Log("Please enter both attacker and defender names.");
        }
    }

    public void OnMainMenuButtonClicked()
    {
        // Load the Main Menu scene (Scene 0)
        SceneManager.LoadScene("Menu");
    }
}
