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
            AudioManager.Instance.PlaySound(AudioManager.Instance.collectGoldfish);
            Destroy(gameObject); 
        }
    }
    private Vector3 GeneratePositionWithGA()
    {
        
        int populationSize = 20;
        Vector3[] population = new Vector3[populationSize];
        Vector3 playerPos = PlayerController.Instance.transform.position;

        // Create initial population
        for (int i = 0; i < populationSize; i++)
        {
            float x = Random.Range(-5f, 5f);
            float y = Random.Range(-5f, 5f);
            population[i] = new Vector3(x, y, 0);
        }

        // Select top 50% candidates based on fitness function
        int numToSelect = populationSize / 2;
        System.Array.Sort(population, (a, b) => FitnessFunction(b, playerPos).CompareTo(FitnessFunction(a, playerPos)));

        // Crossover 
        for (int i = numToSelect; i < populationSize; i++)
        {
                Vector3 parent1 = population[Random.Range(0, numToSelect)];
                Vector3 parent2 = population[Random.Range(0, numToSelect)];
                float x = (parent1.x + parent2.x) / 2;
                float y = (parent1.y + parent2.y) / 2;
                population[i] = new Vector3(x, y, 0);
        }

        // Mutation
        for (int i = 0; i < populationSize ; i++)
        {
            if (Random.value < 0.1f) // 10% chance to mutate
            {
                float mutationX = Random.Range(-1f, 1f);
                float mutationY = Random.Range(-1f, 1f);
                population[i] += new Vector3(mutationX, mutationY, 0);
            }
        }

        // Choose best candidate
        System.Array.Sort(population, (a, b) => FitnessFunction(b, playerPos).CompareTo(FitnessFunction(a, playerPos)));
        Vector3 bestCandidate = population[0];

        return bestCandidate;
    }
    private float FitnessFunction(Vector3 candidate, Vector3 player)
    {
        float distance = Vector3.Distance(candidate, player);
        return distance;
    }
}