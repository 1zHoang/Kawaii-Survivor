using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class PlayerHealth : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int maxHealth;
    private int health;

    [Header("Elements")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI healthText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamage(int damage)
    {
        int realDamage = Mathf.Min(damage, health);
        health -= realDamage;

        //Debug.Log(health);

        UpdateUI();

        if (health <= 0)
        {
            PassAway();
        }
    }

    private void PassAway()
    {
        GameManager.instance.SetGameState(GameState.GAMEOVER);
    }

    private void UpdateUI()
    {
        float healthBarValue = (float)health / maxHealth;
        healthSlider.value = healthBarValue;
        healthText.text = health + " / " + maxHealth;
        //Debug.Log($"Health: {health}, MaxHealth: {maxHealth}, HealthSlider: {healthSlider.value}");
    }
}
