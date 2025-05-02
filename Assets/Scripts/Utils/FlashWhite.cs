using System.Collections;
using UnityEngine;

public class FlashWhite : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Material defaultMaterial;
    private Material whiteMaterial;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultMaterial = spriteRenderer.material;
        whiteMaterial = Resources.Load<Material>("Materials/mWhite"); // Load the white material from Resources folder

    }

    public void Flash()
    {
        spriteRenderer.material = whiteMaterial; // Change the material to white
        StartCoroutine(ResetMaterial()); // Start the coroutine to reset the material
    }

    IEnumerator ResetMaterial(){
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.material = defaultMaterial;
    }

    // Update is called once per frame
    
}
