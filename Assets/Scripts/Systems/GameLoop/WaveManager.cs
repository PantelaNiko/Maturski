using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;

public enum GamePhase
{
    Wave,
    Talent,
    Spell
}

public class WaveManager : MonoBehaviour
{
    [Header("Wave Settings")]
    public float waveDuration = 60f;
    public int currentWave = 1;

    public GamePhase currentPhase;

    [Header("Enemy System")]
    public List<EnemyData> enemyPool;

    [Header("References")]
    [SerializeField] TalentsManager talentsManager;
    [SerializeField] SpellsManager spellsManager;
    [SerializeField] GameObject nextButton;
    [SerializeField] GameObject enemies;
    [SerializeField] TextMeshProUGUI waveNumberUI;
    [SerializeField] TextMeshProUGUI waveTimerUI;

    [SerializeField] Vector2 spawnMin;
    [SerializeField] Vector2 spawnMax;
    [SerializeField] float minDistanceFromPlayer = 5f;
    [SerializeField] Transform playerTransform;
    [SerializeField] PlayerHealth playerHealth;
    [SerializeField] PlayerCombat playerCombat;
    [SerializeField] DeathScreen deathScreen;

    private LocalizedString waveName = new LocalizedString();

    private bool phaseComplete = false;

    private Coroutine gameLoopCoroutine;

    void Start()
    {
        waveName.english = "WAVE";
        waveName.serbian = "TALAS";
        gameLoopCoroutine = StartCoroutine(GameLoop());
    }

    IEnumerator GameLoop()
    {
        while (true)
        {
            playerCombat.castingEnabled = true;
            currentPhase = GamePhase.Wave;
            nextButton.SetActive(false);
            spellsManager.ClearSpells();
            yield return RunWave();

            currentPhase = GamePhase.Talent;
            ClearEnemies();
            playerCombat.castingEnabled = false;
            nextButton.SetActive(true);
            phaseComplete = false;
            talentsManager.PickTalent();

            yield return new WaitUntil(() => phaseComplete);
            talentsManager.ClearTalents();

            currentPhase = GamePhase.Spell;
            phaseComplete = false;
            spellsManager.RollSpells();

            yield return new WaitUntil(() => phaseComplete);
        }
    }

    IEnumerator RunWave()
    {
        waveDuration += 5;
        playerHealth.Heal(playerHealth.maxHealth);
        playerCombat.currentSpell = 0;
        playerCombat.UpdateSpellPool();
        waveNumberUI.text = waveName.Get() + " " + currentWave.ToString();
        float spawnInterval = 4f / currentWave;
        float spawnTimer = 0f;
        float timer = 0f;

        while (timer < waveDuration)
        {
            float dt = Time.deltaTime;
            timer += dt;
            spawnTimer += dt;

            UpdateTimerDisplay(waveDuration - timer);

            if (spawnTimer >= spawnInterval)
            {
                SpawnEnemy();
                spawnTimer = 0f;
            }

            yield return null; // every frame
        }

        currentWave++;
        yield return new WaitForSeconds(0f);
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

        GameObject enemy = Instantiate(chosen.prefab, spawnPos, Quaternion.identity, enemies.transform);

        Enemy enemyComp = enemy.GetComponent<Enemy>();
        enemyComp?.SetRewards(chosen.goldReward, chosen.expReward);
    }

    void ClearEnemies()
    {
        foreach(Transform child in enemies.transform)
        {
            Destroy(child.gameObject);
        }
    }

    Vector3 GetRandomSpawnPosition()
    {
        Vector3 pos;

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
    public void NextPhase()
    {
        phaseComplete = true;
    }

    void UpdateTimerDisplay(float time)
    {
        time = Mathf.Max(time, 0f);
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        int milliseconds = Mathf.FloorToInt((time * 1000) % 1000) / 10;

        waveTimerUI.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
    }

    public void StopGame()
    {
        StopCoroutine(gameLoopCoroutine);
        ClearEnemies();
        playerCombat.castingEnabled = false;
        deathScreen.Appear();
    }
}