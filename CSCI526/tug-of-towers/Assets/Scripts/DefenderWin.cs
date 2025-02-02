using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Required for TextMeshPro

public class DefenderWin : MonoBehaviour
{
    // Reference to the TextMeshPro text object for the title
    public TMP_Text titleText;

    private void Start()
    {
        // Update the text to show the attacker's name in all caps
        if (!string.IsNullOrEmpty(GameVariables.defenderName))
        {
            titleText.text = GameVariables.defenderName.ToUpper() + " WON!!!";
        }
        else
        {
            titleText.text = "DEFENDER WON!!!"; // Fallback in case name is not set
        }
    }
    // This function is called when the "Back to Main Menu" button is clicked
    public void BackToMainMenu()
    {
        // Assuming the MainMenu scene is at index 0 in Build Settings
        SceneManager.LoadScene("Menu");
    }
}
