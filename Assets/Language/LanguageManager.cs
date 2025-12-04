using Frank;
using Sirenix.Utilities;
using UnityEngine;

public class LanguageManager : Singleton<LanguageManager>
{
    public LanguageType currentType;

    public void AutoChange()
    {
        int count = System.Enum.GetValues(typeof(Language)).Length;
        currentType++;
        if((int)currentType > count)
        {
            currentType = 0;
        }
        ChangeLanguage(currentType);
    }

    public void ChangeLanguage(LanguageType type)
    {
        currentType = type;
        var items = FindObjectsOfType<LanguageItem>(true);
        items.ForEach(i => i.Change(currentType));
    }
}
