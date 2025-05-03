using UnityEngine;
public class GoldFish : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    private Vector3 basePos; // Vị trí cơ bản khi ko có floating
    private Vector3 movementDirection = Vector3.zero; // Hướng di chuyển của cá vàng

    [SerializeField] private float moveSpeed  = 0.5f;
    [SerializeField] public float directionChangeInterval = 1.5f; // Thời gian giữa các lần đổi hướng
    private float timeToNextDirectionChange;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        basePos = GeneratePositionWithGA();
        transform.position = basePos;

        PickNewDirection(); // Chọn hướng di chuyển ngẫu nhiên ngay từ đầu
        timeToNextDirectionChange = directionChangeInterval;
    }

    private void Update()
    {
        // Cách vài giây đổi hướng
        timeToNextDirectionChange -= Time.deltaTime;
        if (timeToNextDirectionChange <= 0)
        {
            PickNewDirection();
            timeToNextDirectionChange = directionChangeInterval;
        }
        // di chuyển theo hướng đã chọn
        basePos += movementDirection * moveSpeed * Time.deltaTime;
        
        // floating effect
        float move_y = Mathf.Sin(Time.time) * 0.3f * Time.deltaTime * GameManager.Instance.worldSpeed;
        float move_x = GameManager.Instance.worldSpeed * Time.deltaTime;
        
        // Vị trí cuối cùng là vị trí cơ bản + hiệu ứng lên xuống
        // transform.position = new Vector3(
        //     basePos.x - move_x,
        //     basePos.y + move_y,
        //     basePos.z
        // );

        transform.position += new Vector3(-move_x, move_y); // Di chuyển đối tượng về phía trước

        if (Mathf.Abs(transform.position.x) > 11f){
            Destroy(gameObject); // Destroy the asteroid if it goes out of bounds
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.goldfishCounter++;
            Destroy(gameObject); 
        }
    }
    private void PickNewDirection()
    {
        float angle = Random.Range(0, 360) * Mathf.Deg2Rad;
        movementDirection = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0).normalized;
    }
    private Vector3 GeneratePositionWithGA()
    {
        int populationSize = 20;
        Vector3[] population = new Vector3[populationSize];
        Vector3 playerPos = PlayerController.Instance.transform.position;

        // Tạo quần thể ngẫu nhiên
        for (int i = 0; i < populationSize; i++)
        {
            float x = Random.Range(-5f, 5f);
            float y = Random.Range(-5f, 5f);
            population[i] = new Vector3(x, y, 0);
        }

        // Chọn cá thể có fitness cao nhất, aka xa player nhất
        float bestScore = float.MinValue;
        Vector3 bestPos = Vector3.zero;

        foreach (Vector3 pos in population)
        {
            float score = FitnessFunction(pos, playerPos);
            if (score > bestScore)
            {
                bestScore = score;
                bestPos = pos;
            }
        }

        return bestPos;
    }
    private float FitnessFunction(Vector3 candidate, Vector3 player)
    {
        float distance = Vector3.Distance(candidate, player);
        return distance;
    }
}
