using System;
using System.Collections.Generic;
using Frank;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Main.Phan1.Bai1.Scripts.UI
{
    public class CheckListUI : Singleton<CheckListUI>
    {
        [SerializeField] private GameObject mainCanvas;
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private Transform container;
        [SerializeField] private ChecklistItemUI prefab;
        [SerializeField] private Button checkButton;

        private readonly List<ChecklistItemUI> _spawned = new();
        private Action<bool> _onCheck;

        private void Start()
        {
            mainCanvas.SetActive(false);
        }

        public void Show(CheckListData data, Action<bool> onCheck)
        {
            Show(data.Title, data.CheckListItemDatas, onCheck);
        }

        public void Show(string title, List<CheckListItemData> items, Action<bool> onCheck)
        {
            mainCanvas.SetActive(true);
            
            titleText.text = title;
            
            foreach (Transform c in container) Destroy(c.gameObject);
            _spawned.Clear();

            foreach (var item in items)
            {
                var ui = Instantiate(prefab, container);
                ui.Setup(item);
                _spawned.Add(ui);
            }

            
            _onCheck = onCheck;
            checkButton.onClick.RemoveAllListeners();
            checkButton.onClick.AddListener(OnCheck);
        }

        public void Hide()
        {
            mainCanvas.SetActive(false);
        }
        
        private void OnCheck()
        {
            foreach (var i in _spawned)
            {
                if (!i.IsRight())
                {
                    //TODO: SHOW WRONG
                    _onCheck?.Invoke(false);
                    return;
                }
            }

            //TODO: SHOW TRUE
            _onCheck?.Invoke(true);
        }
    }
}