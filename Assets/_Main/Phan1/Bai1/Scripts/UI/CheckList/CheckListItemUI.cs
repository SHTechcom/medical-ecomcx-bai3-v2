using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace _Main.Phan1.Bai1.Scripts.UI
{
    public class ChecklistItemUI : MonoBehaviour
    {
        public Toggle toggle;
        public TMP_Text label;

        [ShowInInspector, ReadOnly] private CheckListItemData _data;

        public void Setup(CheckListItemData data)
        {
            _data = data;
            label.text = data.label;
            toggle.isOn = false;
        }

        public bool IsRight() => toggle.isOn == _data.isCorrect;
    }
}