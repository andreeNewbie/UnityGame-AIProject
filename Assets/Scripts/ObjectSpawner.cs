using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private Transform minPos;
    [SerializeField] private Transform maxPos;
    public GameObject prefab;
    public float spawnTimer;
    public float spawnInterval;
    public float spawnDelta;
    // Update is called once per frame
    void Update()
    {
        spawnInterval = Mathf.Max(spawnInterval - Time.deltaTime * PlayerController.Instance.boost / spawnDelta, 0.7f); // Increase the spawn interval based on player boost
        spawnTimer += Time.deltaTime * PlayerController.Instance.boost; // Increase the spawn timer based on player boost
        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0;
            SpawnObject();
        }
    }

    private void SpawnObject()
    {
        Instantiate(prefab, RandomSpawnerPoint(), transform.rotation);
    }

    private Vector2 RandomSpawnerPoint()
    {
        Vector2 spawnPoint;
        spawnPoint.x = minPos.position.x;
        spawnPoint.y = Random.Range(minPos.position.y, maxPos.position.y); // Randomly select a y position within the bounds
        
        return spawnPoint;
    }

}
