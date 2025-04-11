using UnityEngine;
using UnityEngine.SceneManagement; // Import the SceneManager class to manage scenes
using System.Collections; // Import the System.Collections namespace for IEnumerator

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static GameManager Instance;
    public float worldSpeed;
    void Awake(){
        if (Instance != null){
            Destroy(gameObject);
        } else{
            Instance = this;
        }
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P) || Input.GetButtonDown("Fire3")){
            pause();
        }
    }

    public void pause() {
        if(UIController.Instance.pausePanel.activeSelf == false){
            Time.timeScale = 0f; // Pause the game
            UIController.Instance.pausePanel.SetActive(true); // Show the pause panel
        } else {
            Time.timeScale = 1f; // Resume the game
            UIController.Instance.pausePanel.SetActive(false); // Hide the pause panel
            PlayerController.Instance.ExitBoost(); // Exit boost mode if the game is resumed
        }
    }

    public void quitGame() {
        Application.Quit(); // Quit the game
    }

    public void GoToMainMenu() {
        SceneManager.LoadScene("MainMenu"); // Load the main menu scene
    }

    public void GameOver() {
        StartCoroutine(ShowGameOverScreen()); // Start the coroutine to show the game over screen
    }

    IEnumerator ShowGameOverScreen() {
        yield return new WaitForSeconds(3f); // Wait for 2 seconds before showing the game over screen
        SceneManager.LoadScene("GameOver"); // Load the game over scene
    }
};
