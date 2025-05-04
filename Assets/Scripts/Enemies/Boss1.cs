using UnityEngine;

public class Boss1 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private float speedX;
    private float speedY;
    private bool switchState = true;
    private float switchInterval;
    private float switchTimer;
    private int lives;
    private FlashWhite flashWhite; // Reference to the FlashWhite script

    private float predictionTime = 0.8f; // Time to predict player's future position
    private Vector3 lastPlayerPos;
    private Vector3 playerVelocity;
    private float speed = 7f;
    private Vector3 pursueDirection; // hướng pursue được cố định khi bắt đầu
    private bool isPursuing = false; // trạng thái pursue
    [SerializeField] private GameObject destroyBossEffect;

    void Start()
    {
        lives = 100; // Set the initial lives of the boss
        EnterChargeState();

        flashWhite = GetComponent<FlashWhite>(); // Get the FlashWhite component attached to the boss
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlayerVelocity(); // Update the player's velocity
        float playerPosition = PlayerController.Instance.transform.position.x; 

        if(switchTimer > 0){
            switchTimer -= Time.deltaTime; 
        }else{
            if(transform.position.x > playerPosition){
                if(switchState) {
                    EnterPatrolState(); 
                }
                else {
                    EnterPursueState();
                }
            }else{
                EnterChargeState(); 
            }
        }


        if(transform.position.y > 3 || transform.position.y < -3){
            speedY = -speedY; // Reverse the y speed if the boss goes out of bounds
        }
        if(transform.position.x < playerPosition) {
            EnterChargeState(); 
        }
        bool boots = PlayerController.Instance.boosting; // Get the player's boots state
        float moveX; // Get the world speed and reverse it
        if(boots && !switchState) {
            moveX = GameManager.Instance.worldSpeed * Time.deltaTime * -0.5f;
        }
        else {
            moveX = speedX * Time.deltaTime; // Use the speedX for movement
        }

        if(!isPursuing) {
            float moveY = speedY * Time.deltaTime;
            transform.position += new Vector3(moveX, moveY);
        }
        else {
            Vector3 targetPosition = PredictPlayerPosition(predictionTime); // Predict the player's position
            PursueTarget(targetPosition); // Pursue the predicted position
        }
        if (Mathf.Abs(transform.position.x) > 11f){
            Destroy(gameObject); // Destroy the asteroid if it goes out of bounds
        }
    }

    void UpdatePlayerVelocity()
    {
        Vector3 currentPos = PlayerController.Instance.transform.position;
        playerVelocity = (currentPos - lastPlayerPos) / Time.deltaTime;
        lastPlayerPos = currentPos;
    }

    Vector3 PredictPlayerPosition(float time)
    {
        return PlayerController.Instance.transform.position + playerVelocity * time;
    }

    void PursueTarget(Vector3 _)
    {
        transform.position += pursueDirection * speed * Time.deltaTime;
    }

    void EnterPursueState() {
        AudioManager.Instance.PlaySound(AudioManager.Instance.bossCharge);
        isPursuing = true; // Set the pursuing state to true
        switchState = true;
        switchInterval = Random.Range(0.6f, 1.3f);
        switchTimer = switchInterval;

        Vector3 targetPosition = PredictPlayerPosition(predictionTime);
        Vector3 dir = targetPosition - transform.position;

        // Boss không được quay ngược
        if (dir.x >= 0f)
        {
            // Nếu hướng tới là bên phải thì coi như từ chối pursuit
            EnterChargeState();
            return;
        }

        pursueDirection = dir.normalized; // chỉ tính hướng 1 lần
    }


    void EnterPatrolState(){
        speedX = 0;
        speedY = Random.Range(-2f, 2f);  
        switchInterval = Random.Range(5f, 10f); // Randomize the switch interval between 4 and 9 seconds
        switchTimer = switchInterval;
        isPursuing = false; // Set the pursuing state to false
        switchState = false;
    }
    void EnterChargeState(){
        AudioManager.Instance.PlaySound(AudioManager.Instance.bossCharge); // Play the boss charge sound
        speedX = -10f;
        speedY = 0;  
        switchInterval = Random.Range(0.6f, 1.3f); // Randomize the switch interval between 1 and 1.5 seconds
        switchTimer = switchInterval;
        isPursuing = false; // Set the pursuing state to false
        switchState = true;
         
    }

    public void TakeDamage(int damage){
        AudioManager.Instance.PlaySound(AudioManager.Instance.hitArmor); // Play the hit sound
        lives -= damage; // Decrease the boss's lives by the damage taken
        flashWhite.Flash(); // Call the Flash method from the FlashWhite script to change the material to white
        if(lives <= 0){
            Instantiate(destroyBossEffect, transform.position, transform.rotation);
            Destroy(gameObject); // Destroy the boss if its lives reach zero
        }
    }

    private void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.CompareTag("Bullet")){
            TakeDamage(5);
        }
            
    }

}