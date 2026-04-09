using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum GamePhase
{
    Wave,
    Reward
}

public class WaveManager : MonoBehaviour
{
    public float waveDuration ;
    public GamePhase currentPhase;
    public int currentWave = 1;

    public List<EnemyData> enemyPool;
    
    [SerializeField] TalentsManager talentsManager;
    [SerializeField] Vector2 spawnMin;
    [SerializeField] Vector2 spawnMax;
    [SerializeField] float minDistanceFromPlayer = 5f;
    [SerializeField] Transform playerTransform;

    void Start()
    {
        StartCoroutine(GameLoop());
    }

    IEnumerator GameLoop()
    {
        while (true)
        {
            currentPhase = GamePhase.Wave;
            yield return StartCoroutine(RunWave());

            currentPhase = GamePhase.Reward;
            yield return StartCoroutine(ShowRewards());
        }
    }

    IEnumerator RunWave()
    {
        float spawnInterval = 2f / Mathf.Sqrt(currentWave);
        float timer = 0f;

        while (timer < waveDuration)
        {
            SpawnEnemy();
            timer += spawnInterval;
            yield return new WaitForSeconds(spawnInterval);
        }

        currentWave++;
        yield return new WaitForSeconds(1f);
    }

    void SpawnEnemy()
    {
        List<EnemyData> available = new List<EnemyData>();
        foreach (var e in enemyPool)
        {
            if (currentWave >= e.unlockWave)
                available.Add(e);
        }

        if (available.Count == 0) return;

        float totalWeight = 0f;
        foreach (var e in available) totalWeight += e.spawnWeight;

        float r = UnityEngine.Random.Range(0f, totalWeight);
        EnemyData chosen = null;

        foreach (var e in available)
        {
            if (r < e.spawnWeight)
            {
                chosen = e;
                break;
            }
            r -= e.spawnWeight;
        }

        if (chosen == null) chosen = available[0];
        Vector3 spawnPos = GetRandomSpawnPosition();

        GameObject enemy = Instantiate(chosen.prefab, spawnPos, Quaternion.identity);
    }

    Vector3 GetRandomSpawnPosition()
    {
        Vector3 pos = Vector3.zero;

        while (true)
        {
            float x = UnityEngine.Random.Range(spawnMin.x, spawnMax.x);
            float z = UnityEngine.Random.Range(spawnMin.y, spawnMax.y);
            pos = new Vector3(x, 0f, z);

            if (Vector3.Distance(pos, playerTransform.position) >= minDistanceFromPlayer)
                break;
        }

        return pos;
    }

    IEnumerator ShowRewards()
    {
        Debug.Log("Phase 2");
        talentsManager.PickTalent();
        yield return new WaitForSeconds(2f);
    }
}