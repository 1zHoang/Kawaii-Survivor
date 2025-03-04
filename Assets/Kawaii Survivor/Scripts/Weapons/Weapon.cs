using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] protected int damage;
    [SerializeField] protected float attackDelay;
    [SerializeField] protected Animator animator;

    protected float attackTimer;

    [Header("Settings")]
    [SerializeField] private float range;
    [SerializeField] protected LayerMask ememyMask;

    [Header("Animations")]
    [SerializeField] protected float aimLerp;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    protected Enemy GetCloestEnemy()
    {
        Enemy cloestEnemy = null;

        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, range, ememyMask);

        if (enemies.Length <= 0)
            return null;

        float minDistance = range;

        for (int i = 0; i < enemies.Length; i++)
        {
            Enemy enemyChecked = enemies[i].GetComponent<Enemy>();

            float distanceToEnemy = Vector2.Distance(transform.position, enemyChecked.transform.position);

            if (distanceToEnemy < minDistance)
            {
                cloestEnemy = enemyChecked;
                minDistance = distanceToEnemy;
            }
        }
        return cloestEnemy;
    }

    protected int GetDamage(out bool isCriticalHit)
    {
        isCriticalHit = false;

        if(UnityEngine.Random.Range(0, 101) <= 50)
        {
            isCriticalHit = true;
            return damage * 2;
        }

        return damage;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
