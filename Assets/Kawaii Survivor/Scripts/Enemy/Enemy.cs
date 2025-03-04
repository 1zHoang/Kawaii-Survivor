using System;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [Header("Components")]
    protected EnemyMovement movement;

    [Header("Health")]
    [SerializeField] protected int maxHealth;
    protected int health;

    [Header("Elements")]
    protected Player player;

    [Header("Spawn")]
    [SerializeField] protected SpriteRenderer renderer1;
    [SerializeField] protected SpriteRenderer sqawnPositon;
    [SerializeField] protected Collider2D collider;
    protected bool hasSpawned;

    [Header("Effects")]
    [SerializeField] protected ParticleSystem passAwayParticles;

    [Header("Attack")]
    [SerializeField] protected float playerDetectionRadius;

    [Header("Actions")]
    public static Action<int, Vector2, bool> onDamageTaken;
    public static Action<Vector2> onPassedAway;

    [Header("Debug")]
    [SerializeField] protected bool gizmos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        health = maxHealth;

        movement = GetComponent<EnemyMovement>();

        player = FindFirstObjectByType<Player>();

        if (player == null)
        {
            Debug.Log("no player");
            Destroy(gameObject);
        }

        StartSpawnSequence();
    }
    private void StartSpawnSequence()
    {
        //Hide the renderer
        //Show the spawn position
        CheckSpawn(false);
        //Scale up and down the position spawn
        Vector3 targetScale = sqawnPositon.transform.localScale * 1.2f;
        LeanTween.scale(sqawnPositon.gameObject, targetScale, .3f)
            .setLoopPingPong(4)
            .setOnComplete(SpawnSequenceComplete);
    }

    private void SpawnSequenceComplete()
    {
        //Hide the renderer
        //Show the spawn position
        CheckSpawn(true);
        hasSpawned = true;

        collider.enabled = true;

        movement.StorePlayer(player);
    }

    private void CheckSpawn(bool check)
    {
        renderer1.enabled = check;
        sqawnPositon.enabled = !check;
    }

    // Update is called once per frame
    protected bool CanAttack()
    {
        return renderer1.enabled;
    }
    public void TakeDamage(int damage, bool isCriticalHit)
    {
        int realDamage = Mathf.Min(damage, health);
        health -= realDamage;

        onDamageTaken?.Invoke(damage, transform.position, isCriticalHit);

        if (health <= 0)
            PassAway();
    }
    private void PassAway()
    {
        onPassedAway?.Invoke(transform.position);

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
