using UnityEngine;

public class Boom : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();  
        Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length); //destroy player after current animation finish playing 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
