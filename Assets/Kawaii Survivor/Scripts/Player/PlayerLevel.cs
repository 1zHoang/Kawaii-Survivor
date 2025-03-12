using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLevel : MonoBehaviour
{
    [Header("Settings")]
    private int requiredXp;
    private int currentXp;
    private int level;
    private int levelsEarnedThisWave;

    [Header("Visuals")]
    [SerializeField] private Slider xpBar;
    [SerializeField] private TextMeshProUGUI levelText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        Candy.onCollected += CandyCollectedCallBack;
    }
    private void OnDestroy()
    {
        Candy.onCollected -= CandyCollectedCallBack;
    }
    void Start()
    {
        UpdateRequiredXp();
        UpdateVisuals();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void UpdateRequiredXp()
    {
        requiredXp = (level + 1) * 5;
    }
    private void UpdateVisuals()
    {
        xpBar.value = (float)currentXp/requiredXp;
        levelText.text = "Lv" + (level+1);
    }
    private void CandyCollectedCallBack(Candy candy)
    {
        currentXp++;

        if(currentXp > requiredXp)
            LevelUp();

        UpdateVisuals();
    }
    private void LevelUp()
    {
        level++;
        levelsEarnedThisWave++;
        currentXp = 0;
        UpdateRequiredXp();
    }
    public bool HasLeveledUp()
    {
        if(levelsEarnedThisWave > 0)
        {
            levelsEarnedThisWave--;
            return true;
        }
        return false;
    }
}
