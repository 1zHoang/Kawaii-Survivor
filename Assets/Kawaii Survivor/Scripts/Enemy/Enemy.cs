using UnityEngine;
using TMPro;
using System;

[RequireComponent (typeof(EnemyMovement))]
public class Enemy : MonoBehaviour
{
    [Header("Components")]
    private EnemyMovement movement;

    [Header("Elements")]
    private Player player;

    [Header("Health")]
    [SerializeField] private int maxHealth;
    private int health;
    [SerializeField] private TextMeshPro healthText;

    [Header("Spawn")]
    [SerializeField] private SpriteRenderer renderer1;
    [SerializeField] private SpriteRenderer sqawnPositon;
    [SerializeField] private Collider2D collider;
    private bool hasSpawned;

    [Header("Attack")]
    [SerializeField] private int damage;
    [SerializeField] private float attackFrequence;
    [SerializeField] private float playerDetectionRadius;
    private float attackDelay;
    private float attackTimer;

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
        healthText.text = health.ToString();

        movement = GetComponent<EnemyMovement>();

        player = FindFirstObjectByType<Player>();
        if (player == null)
        {
            Debug.Log("no player");
            Destroy(gameObject);
        }
        
        StartSpawnSequence();

        attackDelay = 1f / attackFrequence;
        Debug.Log("attack delay: " + attackDelay);
        //Show the enemy after ... seconds
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
        sqawnPositon.enabled= !check;
    }

    // Update is called once per frame
    void Update()
    {
        if (attackTimer > attackDelay)
            TryAttack();
        else
            Wait();
    }
    private void TryAttack()
    {
        float distancePlayer = Vector2.Distance(transform.position, player.transform.position);

        if (distancePlayer <= playerDetectionRadius)
        {
            //PassAway();
            Attack();
        }
    }
    private void Wait()
    {
        attackTimer += Time.deltaTime;
    }

    private void Attack()
    {
        attackTimer = 0;
        player.TakeDamage(damage);
    }

    public void TakeDamage(int damage)
    {
        int realDamage = Mathf.Min(damage, health);
        health -= realDamage;

        healthText.text = health.ToString();

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
