using System.Collections.Generic;
using UnityEngine;

public class FireballSpell : Spell
{
    [Header("Projectile")]
    [SerializeField] float speed = 12f;
    [SerializeField] float lifetime = 5f;
    [SerializeField] float damage = 30f;

    [Header("Burn")]
    [SerializeField] float burnDamage = 5f;
    [SerializeField] float burnDuration = 3f;
    [SerializeField] GameObject burnVfxPrefab;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip castClip;

    float magicMultiplier = 0f;
    float fireMultiplier = 0f;
    PlayerStats stats;

    Vector3 direction;
    bool initialized = false;

    HashSet<Enemy> hitEnemies = new HashSet<Enemy>();

    public override void Cast(Vector3 mouseClickPos)
    {
        direction = (mouseClickPos - transform.position).normalized;
        initialized = true;
        audioSource.clip = castClip;
        audioSource.Play();

        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        if (!initialized) return;

        transform.position += direction * speed * Time.deltaTime;
        transform.right = direction;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy == null) return;

        if (hitEnemies.Contains(enemy)) return;
        hitEnemies.Add(enemy);

        float finalDamage = damage * (1f + magicMultiplier + fireMultiplier);
        enemy.TakeDamage(finalDamage);

        BurnEffect burn = enemy.GetComponent<BurnEffect>();
        if (burn == null)
            burn = enemy.gameObject.AddComponent<BurnEffect>();

        burn.Apply(burnDamage * (1f + fireMultiplier), burnDuration, burnVfxPrefab);
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
            magicMultiplier = value * 0.5f;

        if (type == StatType.FireDamage)
            fireMultiplier = value * 0.8f;
    }
}