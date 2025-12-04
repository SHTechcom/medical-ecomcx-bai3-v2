using UnityEngine;
using UnityEngine.UI;

public class LanguageButton : MonoBehaviour
{
    public Button btn;
    public Sprite[] icons;

    private void Awake()
    {
        btn.onClick.AddListener(() =>
        {
            LanguageManager.Instance.AutoChange();
            btn.image.sprite = icons[(int)LanguageManager.Instance.currentType];
        });
    }
}
