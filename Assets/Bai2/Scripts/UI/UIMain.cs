using System;
using UnityEngine;
using UnityEngine.UI;

public class UIMain : BaseView
{
    [SerializeField] private Button settingBtn;
    private UISetting UISetting => GameViewManager.Instance.GetView<UISetting>();

    private void Start()
    {
        OnClickedSetting(() =>
        {
            UISetting.Show();
        });
    }

    public void OnClickedSetting(Action action)
    {
        settingBtn.onClick.RemoveAllListeners();
        settingBtn.onClick.AddListener(() => { action?.Invoke(); });
    }
}