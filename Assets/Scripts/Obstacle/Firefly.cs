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
        // Validate the target position before moving
        if (float.IsNaN(targetPosition.x) || float.IsNaN(targetPosition.y) || float.IsNaN(targetPosition.z))
        {
            Debug.LogWarning("Invalid target position detected (NaN). Using current player position instead.");
            targetPosition = PlayerController.Instance.transform.position;
        }
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

        // Check for NaN or infinity values in velocity
        if (float.IsNaN(playerVelocity.x) || float.IsInfinity(playerVelocity.x) ||
            float.IsNaN(playerVelocity.y) || float.IsInfinity(playerVelocity.y) ||
            float.IsNaN(playerVelocity.z) || float.IsInfinity(playerVelocity.z)) {
            predictedTargetPosition = currentPlayerPosition;
            return;
        }

        // Check player lại gần firefly => nếu ko thì firefly sẽ bay ra xa player
         Vector3 toFirefly = (transform.position - currentPlayerPosition).normalized;
        Vector3 playerDir = playerVelocity.normalized;
        float approachFactor = Vector3.Dot(toFirefly, playerDir); // > 0 nếu player tiến tới

        float distance = Vector3.Distance(currentPlayerPosition, transform.position);

        // Nếu player đang tiến sát firefly (và khoảng cách gần) thì bỏ dự đoán
        if (approachFactor > 0.7f && distance < 2f)
        {
            predictedTargetPosition = currentPlayerPosition;
            return;
        }
        // vị trí dự đoán của player như bth
        predictedTargetPosition = currentPlayerPosition + playerVelocity * predictTime; 
    }

    // Khi va chạm với Player => firefly biến mất, -1 goldfish của player
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Bullet")) 
        {
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
            GameManager.Instance.critterCounter++;
            Debug.Log("Firefly hit player!");
        }
    }
}
