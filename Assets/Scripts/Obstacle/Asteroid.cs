using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    [SerializeField] private Sprite[] sprites; // Speed of the asteroid
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)]; // Randomly select a sprite from the array
        float pushX = Random.Range(-1f, 0); // Randomly select a push direction on the x-axis
        float pushY = Random.Range(-1f, 1f); // Randomly select a push direction on the y-axis
        rb.linearVelocity = new Vector2(pushX, pushY);
    }   

    // Update is called once per frame
    void Update()
    {
        float moveX = (GameManager.Instance.worldSpeed * PlayerController.Instance.boost) * Time.deltaTime;;
        transform.position += new Vector3(-moveX, 0);
        if (Mathf.Abs(transform.position.x) > 11f){
            Destroy(gameObject); // Destroy the asteroid if it goes out of bounds
        }
    }
}
