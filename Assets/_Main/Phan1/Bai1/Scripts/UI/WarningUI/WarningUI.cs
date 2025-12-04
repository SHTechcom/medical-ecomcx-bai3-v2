using DG.Tweening;
using Frank;
using TMPro;
using UnityEngine;

namespace _Main.Phan1.Bai1.Scripts.UI.WarningUI
{
    public class WarningUI : Singleton<WarningUI>
    {
        public GameObject textParent;
        public TMP_Text text;

        private Tween warningTween;

        public void Show(string warningText, Color? color = null)
        {
            warningTween?.Kill();
            textParent.SetActive(true);

            text.text = warningText;
            text.color = color ?? Color.red;

            warningTween = DOVirtual.DelayedCall(2f, () => textParent.SetActive(false));
        }
    }
}