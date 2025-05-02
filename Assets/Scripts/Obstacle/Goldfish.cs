using UnityEngine;
public class GoldFish : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    public float floatStrength = 0.5f; // Tốc độ trôi
    private Vector3 startPos;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        startPos = GeneratePositionWithGA();
    }

    private void Update()
    {
        // floating up down ~~
        transform.position = startPos + new Vector3(0, Mathf.Sin(Time.time) * floatStrength, 0);

        // floating in random direction
        Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0).normalized;
        transform.position += randomDirection * floatStrength * Time.deltaTime; 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Goldfish is destroyed!");
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
