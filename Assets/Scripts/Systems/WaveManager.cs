using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum GamePhase
{
    Wave,
    Talent,
    Spell
}

public class WaveManager : MonoBehaviour
{
    [Header("Wave Settings")]
    public float waveDuration = 10f;
    public int currentWave = 1;

    public GamePhase currentPhase;

    [Header("Enemy System")]
    public List<EnemyData> enemyPool;

    [Header("References")]
    [SerializeField] TalentsManager talentsManager;
    [SerializeField] GameObject nextButton;

    [SerializeField] Vector2 spawnMin;
    [SerializeField] Vector2 spawnMax;
    [SerializeField] float minDistanceFromPlayer = 5f;
    [SerializeField] Transform playerTransform;

    private bool phaseComplete = false;

    void Start()
    {
        StartCoroutine(GameLoop());
    }

    IEnumerator GameLoop()
    {
        while (true)
        {
            currentPhase = GamePhase.Wave;
            nextButton.SetActive(false);
            yield return RunWave();

            currentPhase = GamePhase.Talent;
            nextButton.SetActive(true);
            phaseComplete = false;
            talentsManager.PickTalent();

            yield return new WaitUntil(() => phaseComplete);
            currentPhase = GamePhase.Spell;
            phaseComplete = false;

            yield return new WaitUntil(() => phaseComplete);
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
        foreach (var e in available)
            totalWeight += e.spawnWeight;

        float r = Random.Range(0f, totalWeight);
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

        Enemy enemyComp = enemy.GetComponent<Enemy>();
        enemyComp?.SetRewards(chosen.goldReward, chosen.expReward);
    }

    Vector3 GetRandomSpawnPosition()
    {
        Vector3 pos;

        while (true)
        {
            float x = Random.Range(spawnMin.x, spawnMax.x);
            float z = Random.Range(spawnMin.y, spawnMax.y);

            pos = new Vector3(x, 0f, z);

            if (Vector3.Distance(pos, playerTransform.position) >= minDistanceFromPlayer)
                break;
        }

        return pos;
    }
    public void NextPhase()
    {
        phaseComplete = true;
    }
}