using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] PlayerStats stats;
    [SerializeField] List<SpellData> spellPool;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Animator animator;

    [SerializeField] float recycleCooldown = 0.5f;

    [SerializeField] float castCooldown = 0f;
    private bool canCast = true;

    private float cooldownTimer = 0f;
    private int currentSpell = 0;

    PlayerMovement movementScript;

    void Start()
    {
        movementScript = GetComponent<PlayerMovement>();

    }
    void Update()
    {
        if (!canCast)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                canCast = true;
            }
        }

        if (Input.GetMouseButtonDown(0) && canCast)
        {
            CastCurrentSpell();
        }
    }
    void StopStun()
    {
        movementScript.moveSpeed = stats.GetStat(StatType.Speed);
    }

    void CastCurrentSpell()
    {
        Vector3 playerPosition = playerTransform.position;
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;
        playerPosition.z = 0f;

        Vector3 directionalVector = (mousePosition - playerPosition).normalized;

        movementScript.moveSpeed = 0;
        animator.SetFloat("MouseHorizontal", directionalVector.x);
        animator.SetFloat("MouseVertical", directionalVector.y);
        animator.SetTrigger("MouseButton1");

        SpellData spellData = spellPool[currentSpell];

        GameObject spellObj = Instantiate(spellData.spellPrefab, playerPosition, Quaternion.identity);
        Spell spell = spellObj.GetComponent<Spell>();

        if (spell != null)
            spell.Cast(mousePosition);

        currentSpell++;
        canCast = false;
        if (currentSpell >= spellPool.Count)
        {
            cooldownTimer = recycleCooldown;
            currentSpell = 0;
        }
        else
        {
            cooldownTimer = castCooldown;  
        }

        Invoke("StopStun", 1f / 3f);
    }

    public void AddSpell(SpellData newSpell)
    {
        spellPool.Add(newSpell);
    }
}
