using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float range;
    [SerializeField] private LayerMask ememyMask;

    [Header("Animations")]
    [SerializeField] private float aimLerp;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        AutoAim();
    }

    private void AutoAim()
    {
        Enemy cloestEnemy = GetCloestEnemy();

        Vector2 targetUpVector = Vector3.up;

        if (cloestEnemy != null)
            targetUpVector = (cloestEnemy.transform.position - transform.position).normalized;

        transform.up = Vector3.Lerp(transform.up, targetUpVector, Time.deltaTime * aimLerp);
    }

    private Enemy GetCloestEnemy()
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
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
