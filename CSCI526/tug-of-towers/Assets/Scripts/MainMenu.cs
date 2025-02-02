using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private GameVariables gameVariables;

    public void PlayGame()
    {
        SceneManager.LoadScene("PlayerSelection");
    }

    public void PlayLevel1()
    {
        PlayerPrefs.SetString("SelectedLevel", "Level1");
        SceneManager.LoadScene("PlayerSelection");
    }
    
    public void PlayLevel2()
    {
        PlayerPrefs.SetString("SelectedLevel", "Level2");
        SceneManager.LoadScene("PlayerSelection");
    }
    
    public void PlayLevel3()
    {
        PlayerPrefs.SetString("SelectedLevel", "Level3");
        SceneManager.LoadScene("PlayerSelection");
    }

    public void PlayTutorial()
    {
        SceneManager.LoadScene("TutorialScene");
    }
}
