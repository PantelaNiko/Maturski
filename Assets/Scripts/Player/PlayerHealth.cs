using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("UI")]
    public GameObject healthBar;
    private RectMask2D healthBarMask;
    private float healthBarWidth;
    [SerializeField] PlayerStats stats;

    void Start()
    {
        healthBarMask = healthBar.GetComponent<RectMask2D>();
        healthBarWidth = healthBar.GetComponent<RectTransform>().rect.width;
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();
    }
    void UpdateHealthUI()
    {
        if (currentHealth < maxHealth)
        {
            healthBar.SetActive(true);
            float ratio = (float)currentHealth / maxHealth;
            healthBarMask.padding = new Vector4(0, 0, healthBarWidth - ratio * healthBarWidth, 0);
        }
        else
        {
            healthBar.SetActive(false);
        }
    }

    void Die()
    {
        Debug.Log("Player has died!");
    }
    void OnEnable()
    {
        stats.OnStatChanged += HandleStatChanged;
    }

    void OnDisable()
    {
        stats.OnStatChanged -= HandleStatChanged;
    }

    void HandleStatChanged(StatType type, float value)
    {
        if (type == StatType.Health)
        {
            maxHealth = 100 + (int)value;
        }
    }
}