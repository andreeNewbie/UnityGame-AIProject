using UnityEngine;
using UnityEngine.SceneManagement; // Import the SceneManager class to manage scenes

public class BigGoldfish : MonoBehaviour
{
    private int count = 0;
    void Update()
    {
        float moveX = GameManager.Instance.worldSpeed * Time.deltaTime;
        transform.position += new Vector3(-moveX, 0);
        if (Mathf.Abs(transform.position.x) > 11f){
            Destroy(gameObject); // Destroy the asteroid if it goes out of bounds
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player")){
            count++;
            AudioManager.Instance.PlaySound(AudioManager.Instance.collectGoldfish);
            Destroy(gameObject); // Destroy the big goldfish when it collides with the player
            GameManager.Instance.countBigGoldfish++;
        }
    }
}
