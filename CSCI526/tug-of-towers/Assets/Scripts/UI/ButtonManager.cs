using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    [System.Serializable]
    public class ButtonCondition
    {
        public Button button;  // Reference to the button
        public int defenseMoneyThreshold;  // Threshold value for defenseMoney
    }

    [SerializeField] private List<ButtonCondition> buttonConditions;  // List of buttons and their thresholds
    private GameVariables gameVariables;

    private void Start()
    {
        // Get the GameVariables object
        gameVariables = GameObject.Find("Variables").GetComponent<GameVariables>();

        // Optionally check if buttonConditions have valid references
        if (buttonConditions == null || buttonConditions.Count == 0)
        {
            Debug.LogError("No button conditions have been assigned!");
        }
    }

    private void Update()
    {
        // Loop through each button condition and apply the threshold logic
        foreach (ButtonCondition condition in buttonConditions)
        {
            if (gameVariables.resourcesInfo.defenseMoney < condition.defenseMoneyThreshold)
            {
                condition.button.interactable = false;  // Disable button if defenseMoney is below threshold
            }
            else
            {
                condition.button.interactable = true;  // Enable button if defenseMoney is above threshold
            }
        }
    }
}
