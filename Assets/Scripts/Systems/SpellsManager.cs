using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpellsManager : MonoBehaviour
{
    [Header("Spell Pools")]
    public List<SpellData> commonSpells;
    public List<SpellData> epicSpells;
    public List<SpellData> legendarySpells;

    [Header("Prefabs")]
    [SerializeField] GameObject commonPrefab;
    [SerializeField] GameObject epicPrefab;
    [SerializeField] GameObject legendaryPrefab;

    [Header("UI")]
    [SerializeField] GameObject spellSelector;

    [Header("References")]
    [SerializeField] PlayerStats playerStats;
    [SerializeField] PlayerCombat playerCombat;
    [SerializeField] WaveManager waveManager;

    [Header("Rarity Weights")]
    [SerializeField] float commonWeight = 70f;
    [SerializeField] float epicWeight = 25f;
    [SerializeField] float legendaryWeight = 5f;

    [Header("Scaling")]

    private LocalizedString boughtText = new LocalizedString();

    void Awake()
    {
        boughtText.english = "BOUGHT";
        boughtText.serbian = "KUPLJENO";
    }
    public void RollSpells()
    {
        spellSelector.SetActive(true);
        foreach (Transform slot in spellSelector.transform)
        {
            RollIntoSlot(slot);
        }
    }

    void RollIntoSlot(Transform slot)
    {
        SpellRarity rarity = RollRarity();
        SpellData spell = GetRandomSpell(rarity);

        if (spell == null) return;

        GameObject prefab = GetPrefab(rarity);
        GameObject spellUI = Instantiate(prefab, slot);

        Transform title = spellUI.transform.Find("Title");
        Transform image = spellUI.transform.Find("Image");
        Transform priceFrame = spellUI.transform.Find("Price");

        title.GetComponent<TextMeshProUGUI>().text = spell.name.Get();


        TextMeshProUGUI priceText = priceFrame.GetComponent<TextMeshProUGUI>();
        priceText.text = spell.price + " G";

        image.GetComponent<Image>().sprite = spell.icon;

        Button button = priceFrame.GetComponent<Button>();
        button.onClick.AddListener(() => OnSpellBought(spell, button, priceText));
    }

    SpellRarity RollRarity()
    {
        float total = commonWeight + epicWeight + legendaryWeight;
        float roll = Random.Range(0, total);

        if (roll < commonWeight)
            return SpellRarity.Common;
        else if (roll < commonWeight + epicWeight)
            return SpellRarity.Epic;
        else
            return SpellRarity.Legendary;
    }

    SpellData GetRandomSpell(SpellRarity rarity)
    {
        List<SpellData> pool = rarity switch
        {
            SpellRarity.Common => commonSpells,
            SpellRarity.Epic => epicSpells,
            SpellRarity.Legendary => legendarySpells,
            _ => commonSpells
        };

        if (pool.Count == 0) return null;

        return pool[Random.Range(0, pool.Count)];
    }

    GameObject GetPrefab(SpellRarity rarity)
    {
        return rarity switch
        {
            SpellRarity.Common => commonPrefab,
            SpellRarity.Epic => epicPrefab,
            SpellRarity.Legendary => legendaryPrefab,
            _ => commonPrefab
        };
    }

    void OnSpellBought(SpellData spell, Button btn, TextMeshProUGUI priceText)
    {
        if (playerStats.gold < spell.price)
        return;

        playerStats.AddGold(-spell.price);
        playerCombat.AddSpell(spell);

        btn.onClick.RemoveAllListeners();
        btn.interactable = false;

        priceText.text = boughtText.Get();
    }

    public void ClearSpells()
    {
        spellSelector.SetActive(false);
        foreach (Transform slot in spellSelector.transform)
        {
            foreach (Transform child in slot)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
public enum SpellRarity
{
    Common,
    Epic,
    Legendary
}