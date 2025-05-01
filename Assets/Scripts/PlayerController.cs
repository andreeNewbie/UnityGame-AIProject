using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    private Rigidbody2D rb;
    private Vector2 playerDirection;
    private Animator animator; // Reference to the Animator component
    private SpriteRenderer spriteRenderer;
 
    private Material defaultMaterial;
    [SerializeField] private Material whiteMaterial;

    [SerializeField] private float moveSpeed; // Speed of the player
    public float boost = 1f;
    private float boostPower = 5f;
    private bool boosting = false;

    [SerializeField] private float energy; // Speed of the player when boosted
    [SerializeField] private float maxEnergy; // Maximum energy of the player
    [SerializeField] private float energyRegen; // Maximum energy of the player
    
    [SerializeField] private float health; // health of the player
    [SerializeField] private float maxHealth; // Maximum health of the player
    
    [SerializeField] private GameObject destroyEffect;

    [SerializeField] private int goldfish = 0;
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); // Get the Animator component attached to the player
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultMaterial = spriteRenderer.material;

        energy = maxEnergy; // Initialize energy to maximum energy
        UIController.Instance.UpdateEnergySlider(energy, maxEnergy); // Update the UI with the initial energy value
        
        health = maxHealth; // Initialize health to maximum health
        UIController.Instance.UpdateEnergySlider(health, maxHealth); // Update the UI with the initial health value
    }

    void Awake()
    {
        if (Instance != null){
            Destroy(gameObject);
        } else{
            Instance = this;
        }
    }
    void Update()
    {
        if(Time.timeScale > 0) {

            float directionX = Input.GetAxisRaw("Horizontal");
            float directionY = Input.GetAxisRaw("Vertical");
            animator.SetFloat("moveX", directionX);
            animator.SetFloat("moveY", directionY); // Set the animator parameters for movement
            
            playerDirection = new Vector2(directionX, directionY).normalized;
            if(Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire2")){
                EnterBoost();
            } else if (Input.GetKeyUp(KeyCode.Space) || Input.GetButtonUp("Fire2")){
                ExitBoost();
            } else if (Input.GetKeyUp(KeyCode.Space) || Input.GetButtonUp("Fire2")){
            }
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(playerDirection.x * moveSpeed, playerDirection.y * moveSpeed);
        if (boosting){
            if (energy >= 0.2f) energy -= 0.2f;
            else ExitBoost();
        } else
        {
            if (energy < maxEnergy){
                energy += energyRegen;
            }
        }
        UIController.Instance.UpdateEnergySlider(energy, maxEnergy); // Update the UI with the current energy value
        UIController.Instance.UpdateHealthSlider(health, maxHealth); // Update the UI with the current energy value

    }

    private void EnterBoost()
    {
        if (energy > 10){
            animator.SetBool("boosting", true);
            boost = boostPower;
            boosting = true;
        }
    }
    public void ExitBoost()
    {
        animator.SetBool("boosting", false);
        boost = 1f;
        boosting = false;
    }

    private void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.CompareTag("Obstacle")){
            TakeDamage(1);
        }
    }

    private void TakeDamage(int damage){
        health -= damage;
        UIController.Instance.UpdateHealthSlider(health, maxHealth);
        spriteRenderer.material = whiteMaterial;
        StartCoroutine("ResetMaterial");
        
        if (health <= 0)
        {
            boost = 0f;
            gameObject.SetActive(false);
            Instantiate(destroyEffect, transform.position, transform.rotation);
            GameManager.Instance.GameOver(); // Call the GameOver method from the GameManager script
        }
    }

    IEnumerator ResetMaterial(){
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.material = defaultMaterial;
    }

    public void UpdateGoldfish(int amount)
    {
        if(amount < 0 && goldfish < amount) return;
        goldfish += amount;
    }
}
