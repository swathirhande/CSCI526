using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject pauseMenu; // Reference to the pause menu panel
    public Button restartButton;
    public Button menuButton;
    public Button closeButton;

    private bool isGamePaused = false;

    private void Start()
    {
        // Add listeners to the buttons
        restartButton.onClick.AddListener(RestartGame);
        menuButton.onClick.AddListener(GoToMainMenu);
        closeButton.onClick.AddListener(ClosePauseMenu);
    }

    private void Update()
    {
        // Toggle pause menu with the Escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    // Called when the pause button is clicked
    public void TogglePauseMenu()
    {
        isGamePaused = !isGamePaused;
        pauseMenu.SetActive(isGamePaused);

        // Pause or resume the game
        Time.timeScale = isGamePaused ? 0 : 1;
    }

    // Restart the current game scene
    private void RestartGame()
    {
        Time.timeScale = 1; // Resume time before restarting
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Go back to the main menu
    private void GoToMainMenu()
    {
        Time.timeScale = 1; // Resume time before switching scenes
        SceneManager.LoadScene("Menu"); // Assuming Main Menu is Scene 0
    }

    // Close the pause menu and resume the game
    private void ClosePauseMenu()
    {
        isGamePaused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1; // Resume time
    }
}
