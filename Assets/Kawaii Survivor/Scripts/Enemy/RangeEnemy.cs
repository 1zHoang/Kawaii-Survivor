using UnityEngine;
using System;
using TMPro;

[RequireComponent(typeof(EnemyMovement),typeof(RangeEnemyAttack))]
public class RangeEnemy : MonoBehaviour
{
    [Header("Components")]
    private EnemyMovement movement;
    private RangeEnemyAttack attack;

    [Header("Elements")]
    private Player player;

    [Header("Health")]
    [SerializeField] private int maxHealth;
    private int health;

    [Header("Spawn")]
    [SerializeField] private SpriteRenderer renderer1;
    [SerializeField] private SpriteRenderer sqawnPositon;
    [SerializeField] private Collider2D collider;
    private bool hasSpawned;

    [Header("Attack")]
    [SerializeField] private float playerDetectionRadius;


    [Header("Actions")]
    public static Action<int, Vector2> onDamageTaken;

    [Header("Debug")]
    [SerializeField] private bool gizmos;

    [Header("Effects")]
    [SerializeField] private ParticleSystem passAwayParticles;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;

        movement = GetComponent<EnemyMovement>();
        attack = GetComponent<RangeEnemyAttack>();

        player = FindFirstObjectByType<Player>();

        attack.StorePlayer(player);

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
    void Update()
    {
        if (!renderer1.enabled)
            return;

        ManageAttack();
    }
    private void ManageAttack()
    {
        float distancePlayer = Vector2.Distance(transform.position, player.transform.position);

        if (distancePlayer > playerDetectionRadius)
            movement.FollowPlayer();
        else
            TryAttack();
    }
    private void TryAttack()
    {
        attack.AutoAim();
    }

    public void TakeDamage(int damage)
    {
        int realDamage = Mathf.Min(damage, health);
        health -= realDamage;

        onDamageTaken?.Invoke(damage, transform.position);

        if (health <= 0)
            PassAway();
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
