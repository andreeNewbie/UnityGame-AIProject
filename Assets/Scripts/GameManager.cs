using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static GameManager Instance;
    public float worldSpeed;
    void Awake(){
        if (Instance != null){
            Destroy(gameObject);
        } else{
            Instance = this;
        }
    }
};
