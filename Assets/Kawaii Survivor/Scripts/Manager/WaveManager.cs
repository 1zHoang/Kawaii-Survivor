using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.UIElements;
using System;
using Random = UnityEngine.Random;

[RequireComponent(typeof(WaveManagerUI))]
public class WaveManager : MonoBehaviour,IGameStateListener
{
    [Header("Element")]
    [SerializeField] private Player player;
    private WaveManagerUI ui;

    [Header("Settings")]
    [SerializeField] private float waveDuration;
    private float timer;
    private bool isTimerOn;
    private int currentWaveIndex;

    [Header("Waves")]
    [SerializeField] private Wave[] waves;
    private List<float> localCounters = new List<float>();

    private void Awake()
    {
        ui = GetComponent<WaveManagerUI>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //StartWave(currentWaveIndex);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isTimerOn)
            return;

        if (timer < waveDuration)
        {
            ManageCurrentWave();

            string timerString = ((int)(waveDuration - timer)).ToString();
            ui.UpdateTimerText(timerString);
        }
        else
            StartWaveTranstion();
    }

    private void StartWave(int waveIndex)
    {
        Debug.Log("Start Wave " + waveIndex);

        ui.UpdateWaveText("Wave " + (currentWaveIndex + 1) + " / " + waves.Length);

        localCounters.Clear();
        foreach (WaveSegment segment in waves[waveIndex].segments)
            localCounters.Add(1);

        timer = 0;
        isTimerOn = true;
    }

    private void ManageCurrentWave()
    {
        Wave currentWave = waves[currentWaveIndex];

        for(int i = 0; i < currentWave.segments.Count; i++)
        {
            WaveSegment segment = currentWave.segments[i];

            float tStart = segment.tStartEnd.x / 100 * waveDuration;
            float tEnd = segment.tStartEnd.y / 100 * waveDuration;

            if (timer < tStart || timer > tEnd)
                continue;

            float timeSinceSegmentStart = timer - tStart;

            float spawnDelay = 1f / segment.spawnFrequence;

            if(timeSinceSegmentStart / spawnDelay > localCounters[i])
            {
                Instantiate(segment.prefab, GetSpawnPosition(), Quaternion.identity);
                localCounters[i]++;
            }
        }

        timer += Time.deltaTime;
    }
    private void StartWaveTranstion()
    {
        isTimerOn = false;

        DefeatAllEnemies();

        currentWaveIndex++;

        if (currentWaveIndex >= waves.Length)
        {
            ui.UpdateTimerText("");
            ui.UpdateWaveText("Stage Completed!");

            GameManager.instance.SetGameState(GameState.STAGECOMPLETE);
            //currentWaveIndex = 0;
        }
        else
            GameManager.instance.WaveCompletedCallBack();

    }
    private void StartNextWave()
    {
        if (currentWaveIndex < 0 || currentWaveIndex >= waves.Length)
        {
            Debug.LogError($"LỖI: currentWaveIndex {currentWaveIndex} không hợp lệ! waves.Length = {waves.Length}");
            return;
        }
        StartWave(currentWaveIndex);
    }
    private void DefeatAllEnemies()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            Destroy(enemy.gameObject);
        }
    }

    private Vector2 GetSpawnPosition()
    {
        Vector2 direction = Random.onUnitSphere;
        Vector2 offset = direction.normalized * Random.Range(6,10);
        Vector2 targetPosition = (Vector2)player.transform.position + offset;

        targetPosition.x = Mathf.Clamp(targetPosition.x, -18, 18);
        targetPosition.y = Mathf.Clamp(targetPosition.y, -8, 8);

        return targetPosition;
    }

    public void GameStateChangedCallBack(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.GAME:
                StartNextWave();
                break;

            case GameState.GAMEOVER:
                isTimerOn = false;
                DefeatAllEnemies();
                break;
        }
    }
}

[System.Serializable]
public struct Wave
{
    public string name;
    public List<WaveSegment> segments;
}

[System.Serializable]
public struct WaveSegment
{
    [MinMaxSlider(0, 100)] public Vector2 tStartEnd;
    public float spawnFrequence;
    public GameObject prefab;
}
