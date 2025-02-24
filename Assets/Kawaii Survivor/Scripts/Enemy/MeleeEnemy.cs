using UnityEngine;
using TMPro;
using System;

[RequireComponent (typeof(EnemyMovement))]
public class MeleeEnemy : Enemy
{
    [Header("Attack")]
    [SerializeField] private int damage;
    [SerializeField] private float attackFrequence;
    private float attackDelay;
    private float attackTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();

        attackDelay = 1f / attackFrequence;
        Debug.Log("attack delay: " + attackDelay);
        //Show the enemy after ... seconds
    }

    // Update is called once per frame
    void Update()
    {
        if(!CanAttack())
            return;

        if (attackTimer > attackDelay)
            TryAttack();
        else
            Wait();

        movement.FollowPlayer();
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
}
