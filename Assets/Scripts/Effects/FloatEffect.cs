using UnityEngine;

public class FloatEffect : MonoBehaviour
{
    public float amplitude = 0.3f; // Độ cao dao động
    public float frequency = 1f;   // Tốc độ dao động

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float offsetY = Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = startPos + new Vector3(0f, offsetY, 0f);
    }
}
