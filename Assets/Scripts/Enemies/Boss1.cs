using UnityEngine;

public class Boss1 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private float speedX;
    private float speedY;
    private bool charging;
    private float switchInterval;
    private float switchTimer;
    private int lives;
    void Start()
    {
        lives = 100; // Set the initial lives of the boss
        EnterChargeState(); 
        AudioManager.Instance.PlaySound(AudioManager.Instance.bossSpawn);
    }

    // Update is called once per frame
    void Update()
    {
        float playerPosition = PlayerController.Instance.transform.position.x; // Get the player's x position

        if(switchTimer > 0){
            switchTimer -= Time.deltaTime; // Decrease the switch timer
        }else{
            if(charging && transform.position.x > playerPosition){
                EnterPatrolState(); // Switch to patrol state
            }else{
                EnterChargeState(); // Switch to charge state
            }
        }
        if(transform.position.y > 3 || transform.position.y < -3){
            speedY = -speedY; // Reverse the y speed if the boss goes out of bounds
        }
        else if(transform.position.x < playerPosition) {
            EnterChargeState(); 
        }
        bool boots = PlayerController.Instance.boosting; // Get the player's boots state
        float moveX; // Get the world speed and reverse it
        if(boots && !charging) {
            moveX = GameManager.Instance.worldSpeed * Time.deltaTime * -0.5f;
        }
        else {
            moveX = speedX * Time.deltaTime; // Use the speedX for movement
        }

        //float moveX = speedX * Time.deltaTime;
        float moveY = speedY * Time.deltaTime;
        transform.position += new Vector3(moveX, moveY);
        if (Mathf.Abs(transform.position.x) > 11f){
            Destroy(gameObject); // Destroy the asteroid if it goes out of bounds
        }
    }

    void EnterPatrolState(){
        speedX = 0;
        speedY = Random.Range(-2f, 2f);  
        switchInterval = Random.Range(5f, 10f); // Randomize the switch interval between 4 and 9 seconds
        switchTimer = switchInterval;
        charging = false; // Set the charging state to false
    }
    void EnterChargeState(){
        if(!charging) AudioManager.Instance.PlaySound(AudioManager.Instance.bossCharge); // Play the boss charge sound
        speedX = -10f;
        speedY = 0;  
        switchInterval = Random.Range(0.6f, 1.3f); // Randomize the switch interval between 1 and 1.5 seconds
        switchTimer = switchInterval;
        charging = true; // Set the charging state to true
         
    }

    public void TakeDamage(int damage){
        AudioManager.Instance.PlaySound(AudioManager.Instance.hitArmor); // Play the hit sound
        lives -= damage; // Decrease the boss's lives by the damage taken
        if(lives <= 0){
            Destroy(gameObject); // Destroy the boss if its lives reach zero
        }
    }

    private void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.CompareTag("Bullet")){
            TakeDamage(2);
        }
            
    }

}
