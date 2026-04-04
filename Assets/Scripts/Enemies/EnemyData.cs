using UnityEngine;

[System.Serializable]
public class EnemyData
{
    [Header("Prefab")]
    public GameObject prefab;

    [Header("Spawn Settings")]
    public int unlockWave = 1;
    public float spawnWeight = 1f;
}