using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using Sirenix.OdinInspector;
 
public class LanguageSwitch : MonoBehaviour
{
    [TabGroup("Stats"), SerializeField] int hp;
    [TabGroup("Stats")] int mana;
    [TabGroup("Inventory")] List<string> items;

    public void SetLocale(int index)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
    }                                   

    public Sprite imageVN; // icon hiển thị khi đang là tiếng Việt
    public Sprite imageEN; // icon hiển thị khi đang là tiếng Anh

    [SerializeField] Button button;
    [SerializeField] Image image;

    private IEnumerator Start()
    {
        yield return LocalizationSettings.InitializationOperation;
        SetLocale(1); // Mặc định là tiếng Việt
        image.sprite = imageVN;
        button.onClick.AddListener(ChangeLanguage);
    }

    private void ChangeLanguage()
    {
        if (LocalizationSettings.SelectedLocale.Identifier.Code == "en")
        {
            SetLocale(1);
            image.sprite = imageVN;
        }
        else
        {
            SetLocale(0);
            image.sprite = imageEN;
        }
        Debug.Log("Current Locale: " + LocalizationSettings.SelectedLocale.Identifier.Code);
    }
}
