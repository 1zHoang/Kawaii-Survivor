using System;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Elements")]
    private Player player;

    [Header("Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float playerDetectionRadius;

    [Header("Debug")]
    [SerializeField] private bool gizmos;

    [Header("Effects")]
    [SerializeField] private ParticleSystem passAwayParticles;

    [Header("Spawn")]
    [SerializeField] private SpriteRenderer renderer1;
    [SerializeField] private SpriteRenderer sqawnPositon;
    private bool hasSpawned;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindFirstObjectByType<Player>();  
        if (player == null)
        {
            Debug.Log("no player");
            Destroy(gameObject);
        }

        //Hide the renderer
        renderer1.enabled = false;
        //Show the spawn position
        sqawnPositon.enabled = true;
        //Scale up and down the position spawn
        Vector3 targetScale = sqawnPositon.transform.localScale * 1.2f;
        LeanTween.scale(sqawnPositon.gameObject, targetScale, .3f)
            .setLoopPingPong(4)
            .setOnComplete(SpawnSequenceComplete);
        //Show the enemy after ... seconds
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasSpawned)
            return;

        FollowPlayer();
        TryAttack();
    }

    private void SpawnSequenceComplete()
    {

        //Hide the renderer
        renderer1.enabled = true;
        //Show the spawn position
        sqawnPositon.enabled = false;

        hasSpawned = true;
    }

    private void FollowPlayer()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;
        Vector2 targetPosition = (Vector2)transform.position + direction * moveSpeed * Time.deltaTime;
        transform.position = targetPosition;
    }
    
    private void TryAttack()
    {
        float distancePlayer = Vector2.Distance(transform.position, player.transform.position);

        if (distancePlayer <= playerDetectionRadius)
        {
            PassAway();
            
        }
    }

    private void PassAway()
    {
        passAwayParticles.transform.SetParent(null);
        passAwayParticles.Play();

        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        if (!gizmos)
            return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, playerDetectionRadius);
    }
}
