using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float gold = 0f;
    public float experience = 0f;
    [SerializeField] float baseHP = 100f;
    [SerializeField] float baseSpeed = 10;

    [SerializeField] TextMeshProUGUI goldText;
    [SerializeField] TextMeshProUGUI expText;
    private Dictionary<StatType, float> stats = new Dictionary<StatType, float>();

    public event Action<StatType, float> OnStatChanged;

    void Awake()
    {
        stats[StatType.Health] = baseHP;
        stats[StatType.Speed] = baseSpeed;

        goldText.text = gold.ToString() + " G";
        expText.text = experience.ToString() + " E";
    }

    public float GetStat(StatType stat)
    {
        return stats[stat];
    }

    public void SetStat(StatType stat, float value)
    {
        stats[stat] = value;
        OnStatChanged?.Invoke(stat, value);
    }

    public void ModifyStat(StatType stat, float amount)
    {
        stats[stat] += amount;
        OnStatChanged?.Invoke(stat, stats[stat]);
    }

    public void AddGold(float reward)
    {
        gold += reward;
        goldText.text = gold.ToString() + " G";
    }

    public void AddExp(float reward)
    {
        experience += reward;
        expText.text = experience.ToString() + " E";
    }
}