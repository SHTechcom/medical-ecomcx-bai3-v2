using System;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

namespace _Main.Phan1.Bai1.Scripts.UI
{
    public class ActionButtonItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text label;
        [SerializeField] private Button btn;
        string tableName = "Bai3";
        private Func<bool> _allow;
        private Action _execute;

        public void Setup(string txt, Func<bool> allow, Action execute)
        {
            label.text = txt;
            var localizedContent = new LocalizedString(tableName, txt);
            localizedContent.StringChanged += value =>
            {
                label.text = value;
            };
            _allow = allow;
            _execute = execute;

            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            if (_allow != null && !_allow())
            {
                return;
            }

            _execute?.Invoke();
        }

        public void Remove()
        {
            Destroy(gameObject);
        }
    }
}