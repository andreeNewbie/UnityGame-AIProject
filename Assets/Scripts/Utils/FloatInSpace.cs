using UnityEngine;

public class FloatInSpace : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        float moveX = GameManager.Instance.worldSpeed * Time.deltaTime; // Tốc độ di chuyển
        transform.position += new Vector3(-moveX, 0f); // Di chuyển đối tượng về phía trước
    }
}
