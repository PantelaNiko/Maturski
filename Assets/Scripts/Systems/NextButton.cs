using UnityEngine;
using UnityEngine.UI;

public class NextButton : MonoBehaviour
{
    [SerializeField] WaveManager waveManager;
    [SerializeField] TalentsManager talentsManager;
    [SerializeField] Button button;

    void Awake()
    {
        button.onClick.AddListener(OnButtonClicked);
    }

    void OnButtonClicked()
    {
        if (waveManager.currentPhase == GamePhase.Talent)
        {
            talentsManager.ClearTalents();
        }
        waveManager.NextPhase();
    }
}
