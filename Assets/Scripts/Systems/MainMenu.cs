using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class MainMenu : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI playText, settingsText, quitText, musicText, languageText;

    [SerializeField] LocalizedString playString = new LocalizedString();
    [SerializeField] LocalizedString settingsString = new LocalizedString();
    [SerializeField] LocalizedString quitString = new LocalizedString();
    [SerializeField] LocalizedString musicString = new LocalizedString();
    [SerializeField] LocalizedString languageString = new LocalizedString();

    [SerializeField] GameObject settingsFrame;

    [SerializeField] Slider volumeSlider;
    [SerializeField] TMP_InputField musicInputText;

    private int musicVolume;
    private void UpdateLanguage()
    {
        playText.text = playString.Get();
        settingsText.text = settingsString.Get();
        quitText.text = quitString.Get();
        musicText.text = musicString.Get();
        languageText.text = languageString.Get();
    }


    public void SetSerbian()
    {
        LanguageManager.CurrentLanguage = Language.Serbian;
        UpdateLanguage();
    }

    public void SetEnglish()
    {
        LanguageManager.CurrentLanguage = Language.English;
        UpdateLanguage();
    }

    public void SettingsButton()
    {
        settingsFrame.SetActive(!settingsFrame.activeSelf);
    }

    public void Play()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void OnSliderChanged(float value)
    {
        musicVolume = Mathf.RoundToInt(value * 100f);
        musicInputText.text = musicVolume.ToString();
        MusicPlayer.Instance.SetVolume(musicVolume);
    }

    public void OnTextChanged(string value)
    {
        if (float.TryParse(value, out float result))
        {
            musicVolume = (int)result;
            volumeSlider.value = result / 100;
            MusicPlayer.Instance.SetVolume(musicVolume);
        }
    }
}
