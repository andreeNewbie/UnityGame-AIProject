using System.Collections;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    
    // private Material defaultMaterial;
    // [SerializeField] private Material whiteMaterial;

    private FlashWhite flashWhite; // Reference to the FlashWhite script

    [SerializeField] private int lives = 3; // Set the initial lives of the obstacle

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        // defaultMaterial = spriteRenderer.material;
        flashWhite = GetComponent<FlashWhite>(); // Get the FlashWhite component attached to the obstacle
        
        float pushX = Random.Range(-1f, 0); // Randomly select a push direction on the x-axis
        float pushY = Random.Range(-1f, 1f); // Randomly select a push direction on the y-axis
        rb.linearVelocity = new Vector2(pushX, pushY);
    }   

    //Update is called once per frame
    void Update()
    {
        float moveX = GameManager.Instance.worldSpeed * Time.deltaTime;;
        transform.position += new Vector3(-moveX, 0);
        if (Mathf.Abs(transform.position.x) > 11f){
            Destroy(gameObject); // Destroy the asteroid if it goes out of bounds
        }
    }
    
    private void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Bullet")){
            TakeDamage(1); // Call the TakeDamage method when colliding with player or bullet
        }
        else if(collision.gameObject.CompareTag("Boss")){
            TakeDamage(10); // Call the TakeDamage method when colliding with boss
        }
    }

    public void TakeDamage(int damage){
        // spriteRenderer.material = whiteMaterial;
        // StartCoroutine("ResetMaterial");
        AudioManager.Instance.PlaySound(AudioManager.Instance.hitRock); // Play the hit sound
        lives -= damage; // Decrease the lives of the obstacle

        flashWhite.Flash(); // Call the Flash method from the FlashWhite script to change the material to white

        if(lives <= 0){
            Destroy(gameObject); // Destroy the obstacle if its lives reach zero
        }
    }
    
    // IEnumerator ResetMaterial(){
    //     yield return new WaitForSeconds(0.2f);
    //     spriteRenderer.material = defaultMaterial;
    // }
}
