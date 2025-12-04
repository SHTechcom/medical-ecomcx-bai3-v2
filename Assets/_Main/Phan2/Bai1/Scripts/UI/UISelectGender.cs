using System;
using Bai11;
using UnityEngine.UI;

public class UISelectGender : BaseView
{
    public Button select1Button;
    public Button select2Button;

    public void OnClickSelect1(Action callback)
    {
        select1Button.onClick.RemoveAllListeners();
        select1Button.onClick.AddListener(() => { callback?.Invoke(); });
    }

    public void OnClickSelect2(Action callback)
    {
        select2Button.onClick.RemoveAllListeners();
        select2Button.onClick.AddListener(() => { callback?.Invoke(); });
    }
}
