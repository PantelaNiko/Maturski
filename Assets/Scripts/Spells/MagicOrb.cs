using UnityEngine;

public class MagicOrb : Spell
{
    [Header("Orb Settings")]
    [SerializeField] float speed = 15f;
    [SerializeField] float damage = 50f;
    [SerializeField] float lifetime = 5f;

    private Vector3 direction;

    float magicMultiplier = 1f;
    PlayerStats stats;

    public override void Cast(Vector3 mouseClickPos)
    {
        direction = (mouseClickPos - transform.position).normalized;
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Enemy enemy = other.GetComponent<Enemy>();

        if (enemy != null)
        {
            enemy.TakeDamage(damage * magicMultiplier);
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        stats = FindFirstObjectByType<PlayerStats>();

        if (stats != null)
            stats.OnStatChanged += HandleStatChanged;
    }

    void OnDisable()
    {
        if (stats != null)
            stats.OnStatChanged -= HandleStatChanged;
    }

    void HandleStatChanged(StatType type, float value)
    {
        if (type == StatType.MagicDamage)
            magicMultiplier = 1f + (value * 0.15f);
    }
}