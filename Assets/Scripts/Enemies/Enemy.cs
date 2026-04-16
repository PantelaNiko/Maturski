using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected float maxHealth = 100f;
    protected float currentHealth;
    protected float goldReward;
    protected float expReward;

    protected virtual void Awake()
    {
        currentHealth = maxHealth;
    }

    public abstract float TakeDamage(float damage);
    public void SetRewards(float gold, float exp)
    {
        goldReward = gold;
        expReward = exp;
    }

    protected void RewardPlayer()
    {
        FindFirstObjectByType<PlayerStats>()?.AddGold(goldReward);
        FindFirstObjectByType<PlayerStats>()?.AddExp(expReward);
    }
}