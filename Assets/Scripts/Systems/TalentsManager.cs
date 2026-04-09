using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TalentsManager : MonoBehaviour
{
    public List<Talent> allTalents;
    [SerializeField] PlayerStats playerStats;
    [SerializeField] GameObject talentSelector;
    [SerializeField] GameObject talentPrefab;
    [SerializeField] float talentsToPick;

    public List<Talent> GetRandomTalents(int count)
    {
        List<Talent> pool = new List<Talent>(allTalents);
        List<Talent> result = new List<Talent>();

        for (int i = 0; i < count && pool.Count > 0; i++)
        {
            int index = Random.Range(0, pool.Count);
            result.Add(pool[index]);
            pool.RemoveAt(index);
        }

        return result;
    }
    public void PickTalent()
    {
        talentSelector.SetActive(true);
        List<Talent> talents = GetRandomTalents(5);
        foreach(Talent talent in talents)
        {
            GameObject talentGUI = Instantiate(talentPrefab, talentSelector.transform);
            Transform iconTransform = talentGUI.transform.Find("Image");
            Transform titleTransform = talentGUI.transform.Find("Title");
            Transform descriptionTransform = talentGUI.transform.Find("Description");
            Transform buttonTransform = talentGUI.transform.Find("Button");
            Transform buttonTextTransform = buttonTransform.Find("Text");

            iconTransform.GetComponent<UnityEngine.UI.Image>().sprite = talent.image;
            titleTransform.GetComponent<TextMeshProUGUI>().text = talent.talentName.Get();
            descriptionTransform.GetComponent<TextMeshProUGUI>().text = talent.description.Get();
            buttonTextTransform.GetComponent<TextMeshProUGUI>().text = string.Concat(talent.price.ToString(), " E");

            Button buttonComponent = buttonTransform.GetComponent<Button>();
            buttonComponent.onClick.AddListener(() => OnButtonClick(talent));
        }
    }

    void OnButtonClick(Talent talent)
    {
        float experience = playerStats.experience;
        if (experience >= talent.price)
        {
            playerStats.ModifyStat(talent.stat, talent.value);
        }
    }
}
