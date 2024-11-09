using UnityEngine;

public class SpriteSorter : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private SpriteRenderer spriteRenderer;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        spriteRenderer.sortingOrder = -(int)(transform.position.y * 10);
    }
}
