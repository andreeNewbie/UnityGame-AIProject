using UnityEngine;

public class Whale : MonoBehaviour
{
    void Update()
    {
        float moveX = GameManager.Instance.worldSpeed * Time.deltaTime;;
        transform.position += new Vector3(-moveX, 0);
        if (Mathf.Abs(transform.position.x) > 11f){
            Destroy(gameObject); // Destroy the asteroid if it goes out of bounds
        }
    }
}
