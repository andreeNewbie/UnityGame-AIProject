using UnityEngine;
using UnityEngine.SceneManagement; // Import the SceneManager class to manage scenes

public class MenuManager1 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.timeScale = 1f; // Ensure the game is not paused at the start   
    }

    public void newGame() {
        SceneManager.LoadScene("Level1"); // Load the game scene
    }

    // Update is called once per frame
    public void quitGame() {
        Application.Quit(); // Quit the game
    }


}
