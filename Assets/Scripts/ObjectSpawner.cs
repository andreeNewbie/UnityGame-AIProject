using System.Collections.Generic; //List<>
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private Transform minPos;
    [SerializeField] private Transform maxPos;

    [SerializeField] private int waveNumber;
    [SerializeField] private List<Wave> waves;
    

    [System.Serializable] 
    public class Wave
    {    
        public GameObject prefab;
        public float spawnTimer;
        public float spawnInterval;
        public int objectsPerWave;
        public int spawnedObjectCount;
    }

    // Update is called once per frame
    void Update()
    {
        waves[waveNumber].spawnTimer -= Time.deltaTime * GameManager.Instance.worldSpeed;
        if (waves[waveNumber].spawnTimer <= 0){
            waves[waveNumber].spawnTimer += waves[waveNumber].spawnInterval;
            SpawnObject();
        }
        if (waves[waveNumber].spawnedObjectCount >= waves[waveNumber].objectsPerWave){
            waves[waveNumber].spawnedObjectCount = 0;
            waveNumber++;
            if (waveNumber >= waves.Count){
                waveNumber = 0;
            }
        }
    }

    private void SpawnObject()
    {
        Instantiate(waves[waveNumber].prefab, RandomSpawnerPoint(), transform.rotation);
        waves[waveNumber].spawnedObjectCount++; 

    }

    private Vector2 RandomSpawnerPoint()
    {
        Vector2 spawnPoint;
        spawnPoint.x = minPos.position.x;
        spawnPoint.y = Random.Range(minPos.position.y, maxPos.position.y); // Randomly select a y position within the bounds
        
        return spawnPoint;
    }

}
