using UnityEngine;

public class Firefly : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float predictTime; // thời gian dự đoán trước (giây)
    private Vector3 lastPlayerPosition;
    private Vector3 predictedTargetPosition;
    private Quaternion targetRotation; 

    

    [SerializeField] private GameObject boomEffect;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        lastPlayerPosition = PlayerController.Instance.transform.position;
    }

    void Update()
    {
        PredictPlayerPosition();
        RotateTowardsTarget(predictedTargetPosition);
        MoveTowardsTarget(predictedTargetPosition); // di chuyển về phía vị trí dự đoán của player
        lastPlayerPosition = PlayerController.Instance.transform.position; // cập nhật vị trí player sau mỗi frame
    }

    private void MoveTowardsTarget(Vector3 targetPosition)
    {
        // Di chuyển về phía vị trí dự đoán của player
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime); 
    }
    private void RotateTowardsTarget(Vector3 targetPosition)
    {
        targetPosition = PlayerController.Instance.transform.position;
        Vector3 relativePos = targetPosition - transform.position; // vector hướng từ firefly đến player

        if (relativePos != Vector3.zero) { 
            targetRotation = Quaternion.LookRotation(Vector3.forward, relativePos);  // xác định góc quay
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 1000f * Time.deltaTime); // tiến hành quay
        }

        // Flip theo hướng di chuyển trái - phải
        if (targetPosition.x > transform.position.x) { // di chuyển sang phải
            spriteRenderer.flipX = true;
        } else {
            spriteRenderer.flipX = false;
        }
    }

    private void PredictPlayerPosition()
    {
         // vị trí hiện tại của player
        Vector3 currentPlayerPosition = PlayerController.Instance.transform.position;

        // vận tốc của player 
        // (tốc độ và hướng di chuyển của player trong một khoảng time rất nhỏ)
        Vector3 playerVelocity = (PlayerController.Instance.transform.position - lastPlayerPosition) / Time.deltaTime; 

        // vị trí dự đoán của player
        predictedTargetPosition = currentPlayerPosition + playerVelocity * predictTime; 
    }

    // Khi va chạm với Player => firefly biến mất, -1 goldfish của player
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) 
        {
            PlayerController.Instance.UpdateGoldfish(-1);

            if (boomEffect != null)
            {
                Instantiate(boomEffect, transform.position, Quaternion.identity);
                Debug.Log("BoomEffect is assigned, instantiating!");
            }
            else
            {
                Debug.LogWarning("BoomEffect is not assigned in the Inspector.");
            }
            Destroy(gameObject, 0.05f); // 0.05 seconds delay
            Debug.Log("Firefly hit player!");
        }
    }
}
