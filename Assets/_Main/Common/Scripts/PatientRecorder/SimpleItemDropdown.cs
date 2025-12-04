using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SimpleItemDropdown : MonoBehaviour
{
    public Button btn;
    public TMP_Text txt;
    private string value;

    public SimpleItemDropdown Init()
    {
        return this;
    }

    public SimpleItemDropdown OnClicked(Action<string> action)
    {
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() => { action?.Invoke(value); });
        return this;
    }

    public SimpleItemDropdown SetText(string content)
    {
        txt?.SetText(content);
        value = content;
        return this;
    }
}
