using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerDetection : MonoBehaviour
{
    [Header("Element")]
    [SerializeField] private CircleCollider2D collectableCollider;
    /*private void FixedUpdate()
    {
        Collider2D[] candyColliders = Physics2D.OverlapCircleAll(
            (Vector2)transform.position + daveCollider.offset,
            daveCollider.radius);

        foreach(Collider2D collider in candyColliders)
        {
            if (collider.TryGetComponent(out Candy candy))
            {
                Debug.Log("Collected : " + candy.name);
                Destroy(candy);
            }
        }
    }*/
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out ICollectable collectable))
        {
            if (!collision.IsTouching(collectableCollider))
                return;

            collectable.Collect(GetComponent<Player>());
        }
    }
}
