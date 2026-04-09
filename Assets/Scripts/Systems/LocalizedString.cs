using UnityEngine;

[System.Serializable]
public class LocalizedString
{
    [TextArea] public string english;
    [TextArea] public string serbian;

    public string Get()
    {
        switch (LanguageManager.CurrentLanguage)
        {
            case Language.Serbian:
                return serbian;
            default:
                return english;
        }
    }
}