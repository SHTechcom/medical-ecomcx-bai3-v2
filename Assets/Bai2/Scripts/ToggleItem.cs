using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToggleItem : MonoBehaviour
{
    public Toggle toggle;
    public TMP_Text labelText;

    public void SetLabel(string label)
    {
        labelText.text = label;
    }

    public void OnChange(Action<bool> callback)
    {
        toggle.onValueChanged.RemoveAllListeners();
        toggle.onValueChanged.AddListener((value) =>
        {
            callback?.Invoke(value);
        });
    }
}
