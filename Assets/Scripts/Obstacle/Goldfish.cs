using UnityEngine;
public class GoldFish : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        transform.position = GeneratePositionWithGA(); 
    }

    private void Update()
    {        
        // floating effect
        float move_y = Mathf.Sin(Time.time) * 0.3f * Time.deltaTime * GameManager.Instance.worldSpeed;        
        float move_x = GameManager.Instance.worldSpeed * Time.deltaTime;

        transform.position += new Vector3(-move_x, move_y); // Di chuyển đối tượng về phía trước

        if (Mathf.Abs(transform.position.x) > 11f){
            Destroy(gameObject); 
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
