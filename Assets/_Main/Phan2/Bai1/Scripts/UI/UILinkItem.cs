using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILinkItem : MonoBehaviour
{
    public Button selectButton;
    public TMP_Text labelText;
    public TMP_Text linkText;

    public void OnClickedSelectLink(Action callback)
    {
        selectButton.onClick.RemoveAllListeners();
        selectButton.onClick.AddListener(() => { callback?.Invoke(); });
    }

    public void Set(string label, string link)
    {
        labelText.text = label;
        linkText.text = link;
    }
}
