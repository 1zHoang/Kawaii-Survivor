using UnityEngine;
using System;
using TMPro;

[RequireComponent(typeof(EnemyMovement),typeof(RangeEnemyAttack))]
public class RangeEnemy : Enemy
{
    private RangeEnemyAttack attack;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();

        attack = GetComponent<RangeEnemyAttack>();

        attack.StorePlayer(player);
    }

    // Update is called once per frame
    void Update()
    {
        if (!CanAttack())
            return;

        ManageAttack();

        transform.localScale = player.transform.position.x > transform.position.x ? Vector3.one : Vector3.one.With(x: -1);
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
}
