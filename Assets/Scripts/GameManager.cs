using UnityEngine;
using UnityEngine.SceneManagement; // Import the SceneManager class to manage scenes
using System.Collections; // Import the System.Collections namespace for IEnumerator

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static GameManager Instance;
    public float worldSpeed;
    public int critterCounter;
    [SerializeField] private GameObject boss1; 
    [SerializeField] public int goldfishCounter; // count số cá vàng thu đc trong game, khác với goldfish cua player
    [SerializeField] private GameObject bigGoldfish1; 
    void Awake(){
        if (Instance != null){
            Destroy(gameObject);
        } else{
            Instance = this;
        }
    }

    void Start(){
        critterCounter = 0; // Initialize the critter counter to 0
        goldfishCounter = 0; // Initialize the goldfish counter to 0
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P) || Input.GetButtonDown("Fire3")){
            pause();
        }
        if(critterCounter > 10) {
            critterCounter = 0;
            Instantiate(boss1, new Vector2(11f, 0), Quaternion.identity); // Spawn the boss
        }
        if(goldfishCounter >= 10) {
            goldfishCounter = 0;
            Debug.Log("Spawned big goldfish"); // Log the spawning of the big goldfish
            Instantiate(bigGoldfish1, new Vector2(11f, Random.Range(-5f,5f)), Quaternion.identity); // Spawn the big goldfish
        }
    }

    public void pause() {
        if(UIController.Instance.pausePanel.activeSelf == false){
            AudioManager.Instance.PlaySound(AudioManager.Instance.pause);
            Time.timeScale = 0f; // Pause the game
            UIController.Instance.pausePanel.SetActive(true); // Show the pause panel
        } else {
            AudioManager.Instance.PlaySound(AudioManager.Instance.unpause);
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
