using System;
using UnityEngine;

public class DropManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Candy candyPrefab;

    private void Awake()
    {
        Enemy.onPassedAway += EnemyPassedAwayCallBack;
    }
    private void OnDestroy()
    {
        Enemy.onPassedAway += EnemyPassedAwayCallBack;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void EnemyPassedAwayCallBack(Vector2 enemyPosition)
    {
        Instantiate(candyPrefab,enemyPosition, Quaternion.identity, transform);
    }
}
