using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainLightningSpell : Spell
{
    [Header("Movement")]
    [SerializeField] float speed = 25f;
    [SerializeField] float lifetime = 5f;

    [Header("Chain")]
    [SerializeField] float damage = 40f;
    [SerializeField] float chainRange = 6f;
    [SerializeField] int maxChains = 5;
    [SerializeField] float damageFalloff = 0.8f;

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip castClip;

    float magicMultiplier = 1f;
    float lightningMultiplier = 1f;

    PlayerStats stats;

    private Vector3 direction;
    private bool hasStarted = false;

    public override void Cast(Vector3 mouseClickPos)
    {
        direction = (mouseClickPos - transform.position).normalized;
        hasStarted = true;
        audioSource.clip = castClip;
        audioSource.Play();

        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        if (!hasStarted) return;

        transform.position += direction * speed * Time.deltaTime;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Enemy first = other.GetComponent<Enemy>();
        if (first == null) return;

        StartCoroutine(ChainRoutine(first));
    }

    IEnumerator ChainRoutine(Enemy start)
    {
        HashSet<Enemy> hit = new HashSet<Enemy>();

        Enemy current = start;
        float currentDamage = damage;

        for (int i = 0; i < maxChains; i++)
        {
            if (current == null) break;

            float finalDamage = currentDamage * (1f + magicMultiplier + lightningMultiplier);

            current.TakeDamage(finalDamage);
            hit.Add(current);

            Enemy next = FindNext(current.transform.position, hit);

            if (next == null)
                break;

            yield return MoveTo(next.transform.position);

            current = next;
            currentDamage *= damageFalloff;
        }

        Destroy(gameObject);
    }

    IEnumerator MoveTo(Vector3 target)
    {
        Vector3 start = transform.position;
        float t = 0f;
        float duration = Vector3.Distance(start, target) / speed;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;

            transform.position = Vector3.Lerp(start, target, t);

            Vector3 dir = (target - transform.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);

            yield return null;
        }
    }

    Enemy FindNext(Vector3 pos, HashSet<Enemy> excluded)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(pos, chainRange);

        float best = Mathf.Infinity;
        Enemy bestEnemy = null;

        foreach (var h in hits)
        {
            Enemy e = h.GetComponent<Enemy>();
            if (e == null || excluded.Contains(e)) continue;

            float d = Vector2.Distance(pos, e.transform.position);
            if (d < best)
            {
                best = d;
                bestEnemy = e;
            }
        }

        return bestEnemy;
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
            magicMultiplier = value * 0.1f;

        if (type == StatType.LightningDamage)
        {
            lightningMultiplier = value * 0.2f;
            chainRange = 6f + value;
            maxChains = 5 + (int)value;
        }

    }
}