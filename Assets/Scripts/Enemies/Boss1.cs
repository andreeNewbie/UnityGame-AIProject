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
    }

    // Update is called once per frame
    void Update()
    {
        if(switchTimer > 0){
            switchTimer -= Time.deltaTime; // Decrease the switch timer
        }else{
            if(charging){
                EnterPatrolState(); // Switch to patrol state
            }else{
                EnterChargeState(); // Switch to charge state
            }
        }
        if(transform.position.y > 3 || transform.position.y < -3){
            speedY = -speedY; // Reverse the y speed if the boss goes out of bounds
        }
        float moveX = speedX * PlayerController.Instance.boost * Time.deltaTime;
        float moveY = speedY * Time.deltaTime;
        transform.position += new Vector3(moveX, moveY);
        if (Mathf.Abs(transform.position.x) > 11f){
            Destroy(gameObject); // Destroy the asteroid if it goes out of bounds
        }
    }

    void EnterPatrolState(){
        speedX = 0;
        speedY = Random.Range(-2f, 2f);  
        switchInterval = Random.Range(4f, 9f); // Randomize the switch interval between 4 and 9 seconds
        switchTimer = switchInterval;
        charging = false; // Set the charging state to false
    }
    void EnterChargeState(){
        speedX = -5f;
        speedY = 0;  
        switchInterval = Random.Range(1f, 1.5f); // Randomize the switch interval between 1 and 1.5 seconds
        switchTimer = switchInterval;
        charging = true; // Set the charging state to true
        AudioManager.Instance.PlaySound(AudioManager.Instance.bossCharge); // Play the boss charge sound 
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
