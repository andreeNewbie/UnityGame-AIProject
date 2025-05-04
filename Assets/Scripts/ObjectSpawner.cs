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

    // int min= 60;
    // // Update is called once per frame
    // void Update()
    // {
    //     waves[waveNumber].spawnTimer -= Time.deltaTime * GameManager.Instance.worldSpeed;
    //     if (waves[waveNumber].spawnTimer <= 0){
    //         waves[waveNumber].spawnTimer += waves[waveNumber].spawnInterval;
    //         SpawnObject();
    //     }
    //     if (waves[waveNumber].spawnedObjectCount >= waves[waveNumber].objectsPerWave){
    //         waves[waveNumber].spawnedObjectCount = 0;
    //         waveNumber++;
    //         if (waveNumber >= waves.Count){
    //             waveNumber = 0;
    //         }
    //     }
        
    //     waves[waveNumber].spawnInterval -= 0.01f * Time.deltaTime;
    //     if (waves[waveNumber].spawnInterval < 0.4f)
    //         waves[waveNumber].spawnInterval = 0.4f;

    //     if (Mathf.RoundToInt(Time.time / min) == 1){
    //         waves[waveNumber].objectsPerWave += 1; // Ensure the spawn interval doesn't go below the minimum value
    //         min += 60;
    //     }
    // // }

    private float nextIncreaseTime = 60f;
    private float intervalDecayRate = 0.01f;
    private float minInterval = 0.4f;
    float waveElapsedTime = 0f;
    void Update()
    {
        Wave currentWave = waves[waveNumber];

        // Giảm spawnTimer
        currentWave.spawnTimer -= Time.deltaTime * GameManager.Instance.worldSpeed;
        waveElapsedTime += Time.deltaTime * GameManager.Instance.worldSpeed;

        // Đã spawn đủ trong wave hiện tại?
        if (currentWave.spawnedObjectCount >= currentWave.objectsPerWave || waveElapsedTime >= 5f)
        {
            currentWave.spawnedObjectCount = 0;
            currentWave.spawnTimer = 0;

            waveNumber++;
            if (waveNumber >= waves.Count)
                waveNumber = 0;

            waveElapsedTime = 0f;
            return;
        }

        // Spawn nếu đến giờ
        if (currentWave.spawnTimer <= 0)
        {
            currentWave.spawnTimer = currentWave.spawnInterval;
            SpawnObject();
        }

        // 🟡 Giảm dần spawnInterval cho wave hiện tại
        currentWave.spawnInterval -= intervalDecayRate * Time.deltaTime;
        currentWave.spawnInterval = Mathf.Max(currentWave.spawnInterval, minInterval);

        // 🔺 Tăng objectsPerWave của TẤT CẢ waves mỗi 60 giây
        if (Time.time >= nextIncreaseTime)
        {
            foreach (var wave in waves)
            {
                wave.objectsPerWave += 1;
            }
            nextIncreaseTime += 60f;
        }
    }


    private void SpawnObject()
    {
        Instantiate(waves[waveNumber].prefab, RandomSpawnerPoint(), transform.rotation);
        waves[waveNumber].spawnedObjectCount++; 

    }

    private Vector2 RandomSpawnerPoint()
    {
        float halfHeight = GetPrefabHalfHeight(waves[waveNumber].prefab);

        float yMin = minPos.position.y + halfHeight;
        float yMax = maxPos.position.y - halfHeight;

        float y = Random.Range(yMin, yMax);
        float x = minPos.position.x;

        return new Vector2(x, y);
    }

    private float GetPrefabHalfHeight(GameObject prefab)
    {
        SpriteRenderer sr = prefab.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            return sr.bounds.extents.y; // = 1/2 chiều cao
        }

        return 0.5f; // fallback
    }


}
