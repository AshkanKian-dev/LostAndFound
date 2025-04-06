using UnityEngine;

[CreateAssetMenu(fileName = "New Language", menuName = "Localization/Language")]
public class LanguageData : ScriptableObject
{
    public string languageName;
    [TextArea] public string description;
    public Sprite symbol; // optional for UI
}
