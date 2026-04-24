using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] PlayerStats stats;
    [SerializeField] GameObject spellUI;
    [SerializeField] GameObject spellPrefab;
    [SerializeField] List<SpellData> spellPool;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Animator animator;

    [SerializeField] float rechargeCooldown;

    [SerializeField] float castCooldown;

    float castSpeedReduction;
    float rechargeSpeedReduction;
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

        Transform selectedUI = spellUI.transform.GetChild(currentSpell).GetChild(0);
        selectedUI.gameObject.SetActive(false);

        currentSpell++;

        canCast = false;
        if (currentSpell >= spellPool.Count)
        {
            cooldownTimer = Mathf.Max(0.05f, rechargeCooldown - rechargeSpeedReduction);
            currentSpell = 0;
        }
        else
        {
            cooldownTimer = Mathf.Max(0.05f, castCooldown - castSpeedReduction);  
        }
        Transform nextSelectedUI = spellUI.transform.GetChild(currentSpell).GetChild(0);
        nextSelectedUI.gameObject.SetActive(true);

        Invoke("StopStun", 1f / 3f);
    }

    public void AddSpell(SpellData newSpell)
    {
        spellPool.Add(newSpell);
        GameObject spellIcon = Instantiate(spellPrefab, spellUI.transform);
        spellIcon.GetComponent<Image>().sprite = newSpell.icon;
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
        if (type == StatType.CastSpeed)
            castSpeedReduction = value;

        if (type == StatType.RechargeSpeed)
            rechargeSpeedReduction = value;
    }
}
