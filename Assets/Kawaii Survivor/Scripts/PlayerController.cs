using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb= GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.right;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
