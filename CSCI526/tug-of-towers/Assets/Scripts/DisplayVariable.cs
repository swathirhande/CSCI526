using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
using System.Collections.Generic;
using TMPro;

public class DisplayVariable : MonoBehaviour
{
    // Enum to specify the type of information to be displayed
    public enum InfoType { SYSTEM_INFO, RESOURCE_INFO, STATISTIC_INFO };
    public InfoType infoType;

    [Tooltip("Only required when InfoType is set to RESOURCE_INFO. Specifies which resource variable to display.")]
    public string variableName;

    private GameVariables gameVariables; // Reference to the GameVariables script
    private Text displayText;            // UI Text component
    private TextMeshProUGUI displayTMP;   // TextMeshPro UI component

    void Start()
    {
        // Initialize references to GameVariables, Text, and TextMeshPro components
        gameVariables = GameObject.Find("Variables").GetComponent<GameVariables>();
        displayText = GetComponent<Text>();
        displayTMP = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        // Display the variable value based on the specified InfoType
        if (infoType == InfoType.SYSTEM_INFO || infoType == InfoType.RESOURCE_INFO || infoType == InfoType.STATISTIC_INFO)
        {
            // Attempt to retrieve the specified variable from GameVariables
            KeyValuePair<Info, FieldInfo>? variable = gameVariables.GetVariable(variableName);
            try
            {
                // Update the text in the UI based on the retrieved variable
                if (displayText != null)
                {
                    displayText.text = variable.Value.Value.GetValue(variable.Value.Key).ToString();
                }
                else if (displayTMP != null)
                {
                    displayTMP.text = variable.Value.Value.GetValue(variable.Value.Key).ToString();
                }
            }
            catch
            {
                Debug.Log($"Display: Invalid Variable '{variableName}'");
            }
        }

    }
}
