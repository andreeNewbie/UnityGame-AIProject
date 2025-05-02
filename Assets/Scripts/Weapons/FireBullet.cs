using UnityEngine;

public class FireBullet : MonoBehaviour
{
    
    void Update()
    {
        transform.position += new Vector3(FireWeapon.Instance.speed * Time.deltaTime, 0f);
        if (transform.position.x > 9){
            gameObject.SetActive(false);
        }
    }

    void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("Boss")){
            gameObject.SetActive(false);
        }
    }
}
