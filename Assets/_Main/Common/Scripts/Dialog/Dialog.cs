using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Dialog : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text contentText;
    public Button btn;

    public void OnClicked(Action action)
    {
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() =>
        {
            action?.Invoke();
        });
    }

    public void Set(string name, string content)
    {
        nameText.text = name;
        contentText.text = content;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
