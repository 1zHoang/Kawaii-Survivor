using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    enum State
    {
        Idle,
        Attack
    }

    private State state;

    [Header("Elements")]
    [SerializeField] private Transform hitDetectionTransform;
    [SerializeField] private BoxCollider2D hitCollider;

    [Header("Settings")]
    private List<Enemy> damgedEnemies = new List<Enemy>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        state = State.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Idle:
                AutoAim();
                return;

            case State.Attack:
                Attacking();
                return;
        }
    }
    private void AutoAim()
    {
        Enemy cloestEnemy = GetCloestEnemy();

        Vector2 targetUpVector = Vector3.up;

        if (cloestEnemy != null)
        {
            targetUpVector = (cloestEnemy.transform.position - transform.position).normalized;
            transform.up = targetUpVector;
            ManageAttack();
        }

        transform.up = Vector3.Lerp(transform.up, targetUpVector, Time.deltaTime * aimLerp);

        IncrementAttackTimer();
    }

    private void ManageAttack()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer > attackDelay)
        {
            attackTimer = 0;
            StartAttack();
        }
    }

    private void IncrementAttackTimer()
    {
        attackTimer += Time.deltaTime;
    }
    [NaughtyAttributes.Button]
    private void StartAttack()
    {
        animator.Play("Attack");
        state = State.Attack;

        damgedEnemies.Clear();

        animator.speed = 1f / attackDelay;
    }

    private void Attacking()
    {
        Attack();
    }

    private void StopAttack()
    {
        state = State.Idle;

        // Clear the attacked enemies
        damgedEnemies.Clear();
        // Damage enemies list
    }

    private void Attack()
    {
        //Collider2D[] enemies = Physics2D.OverlapCircleAll(hitDetectionTransform.position, hitDetectionRadius, ememyMask);
        Collider2D[] enemies = Physics2D.OverlapBoxAll(hitDetectionTransform.position,
            hitCollider.bounds.size,
            hitDetectionTransform.localEulerAngles.z,
            ememyMask);

        for (int i = 0; i < enemies.Length; i++)
        {
            Enemy enemy = enemies[i].GetComponent<Enemy>();

            // 1. Is the enemy inside of the list
            if (!damgedEnemies.Contains(enemy))
            {
                int damage = GetDamage(out bool isCriticalHit);

                enemy.TakeDamage(damage, isCriticalHit);
                damgedEnemies.Add(enemy);
            }
            // 2. If no, let's attack him, and add him to the list

            // 3. If yes, let's continue, check the next enemy

        }
    }
}
