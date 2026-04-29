using UnityEngine;

[System.Serializable]
public class Talent
{
    public LocalizedString talentName;
    public LocalizedString description;

    public Sprite image;
    public float price;
    public StatType stat;
    public float value;
}