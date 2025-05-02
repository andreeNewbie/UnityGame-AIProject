using UnityEngine;

public class DestroyWhen : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();  
        Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length); //destroy player after current animation finish playing 
    }
}
