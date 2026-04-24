using System.Collections;
using UnityEngine;

public class BurnEffect : MonoBehaviour
{
    [Header("Damage")]
    [SerializeField] float tickDamage = 5f;
    [SerializeField] float duration = 3f;
    [SerializeField] float tickRate = 0.5f;

    [Header("VFX")]
    [SerializeField] GameObject burnVfxPrefab;

    Enemy enemy;
    Coroutine burnRoutine;
    GameObject vfxInstance;

    void Awake()
    {
        enemy = GetComponent<Enemy>();
    }

    public void Apply(float newTickDamage, float newDuration, GameObject vfxPrefab)
    {
        tickDamage = newTickDamage;
        duration = newDuration;
        burnVfxPrefab = vfxPrefab;

        if (burnRoutine != null)
            StopCoroutine(burnRoutine);

        if (vfxInstance == null && burnVfxPrefab != null)
        {
            vfxInstance = Instantiate(burnVfxPrefab, enemy.transform);
            FollowTarget follow = vfxInstance.GetComponent<FollowTarget>();
            if (follow != null)
                follow.Init(transform);
        }

        burnRoutine = StartCoroutine(Burn());
    }

    IEnumerator Burn()
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            enemy.TakeDamage(tickDamage);

            yield return new WaitForSeconds(tickRate);
            elapsed += tickRate;
        }

        if (vfxInstance != null)
            Destroy(vfxInstance);

        Destroy(this);
    }
}