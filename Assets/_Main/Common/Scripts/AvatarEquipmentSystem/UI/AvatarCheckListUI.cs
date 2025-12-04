using System;
using System.Collections.Generic;
using Frank;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Main.Common.Scripts.Avatar.UI
{
    public class AvatarCheckListUI : Singleton<AvatarCheckListUI>
    {
        [SerializeField] private AvatarCheckListItem itemPrefab;
        [SerializeField] private TMP_Text title;
        [SerializeField] private TMP_Text checkListNote;
        [SerializeField] private RectTransform content;
        [SerializeField] private Button continueButton;

        private readonly List<AvatarCheckListItem> AvatarCheckListItems = new();

        private AvatarEquipmentPreset _preset;
        private EquipmentType _currentCheckListType;

        private void Start()
        {
            continueButton.onClick.AddListener(OnContinueClick);
        }

        private void OnEnable()
        {
            AvatarEquipmentSystem.OnEquipItem += OnEquip;
            AvatarEquipmentSystem.OnUnEquipItem += OnUnEquip;
        }

        private void OnDisable()
        {
            AvatarEquipmentSystem.OnEquipItem -= OnEquip;
            AvatarEquipmentSystem.OnUnEquipItem -= OnUnEquip;
        }

        private void OnEquip(AvatarEquipment equip)
        {
            foreach (var item in AvatarCheckListItems)
                item.OnEquip(equip);

            UpdateEquipments();
        }

        private void OnUnEquip(AvatarEquipment equip)
        {
            foreach (var item in AvatarCheckListItems)
                item.OnUnEquip(equip);

            UpdateEquipments();
        }

        public void Show(EquipmentType type, AvatarEquipmentPreset preset, AvatarEquipmentDatabase database)
        {
            if (database == null)
            {
                Show(type, preset);
                return;
            }

            _preset = preset;
            _currentCheckListType = type;

            gameObject.SetActive(true);

            switch (type)
            {
                case EquipmentType.Cloth:
                    Setup(database.cloths);
                    break;
                case EquipmentType.ToolAndMedicine:
                    Setup(database.toolsAndMedicines);
                    break;
            }

            UpdateEquipments();
        }

        public void Show(EquipmentType type, AvatarEquipmentPreset preset)
        {
            _preset = preset;
            _currentCheckListType = type;

            gameObject.SetActive(true);

            checkListNote.text = string.Empty;

            switch (type)
            {
                case EquipmentType.Cloth:
                    Setup(preset.cloths);
                    break;
                case EquipmentType.ToolAndMedicine:
                    Setup(preset.toolsAndMedicines);
                    break;
            }

            UpdateEquipments();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
        
        public void UpdateEquipments()
        {
            if (_preset == null) return;

            var missing = AvatarEquipmentSystem.GetMissingItems(_currentCheckListType, _preset);
            var extra = AvatarEquipmentSystem.GetExtraItems(_currentCheckListType, _preset);

            string message = _currentCheckListType switch
            {
                EquipmentType.Cloth when missing.Count > 0 => _preset.missingClothWarning,
                EquipmentType.ToolAndMedicine when missing.Count > 0 => _preset.missingToolAndMedicineWarning,

                EquipmentType.Cloth when extra.Count > 0 => _preset.extraClothWarning,
                EquipmentType.ToolAndMedicine when extra.Count > 0 => _preset.extraToolAndMedicineWarning,

                EquipmentType.Cloth => _preset.exactlyClothWarning,
                EquipmentType.ToolAndMedicine => _preset.exactlyToolAndMedicineWarning,

                _ => string.Empty
            };
            
            if(missing.Count == 0 && extra.Count == 0) continueButton.gameObject.SetActive(true);

            checkListNote.text = message;
        }

        private void Setup(List<AvatarEquipment> data)
        {
            int need = data.Count;
            int have = AvatarCheckListItems.Count;

            for (int i = have; i < need; i++)
            {
                var it = SimplePool.Spawn(itemPrefab);
                it.transform.SetParent(content, false);
                AvatarCheckListItems.Add(it);
            }

            for (int i = 0; i < need; i++)
            {
                AvatarCheckListItems[i].gameObject.SetActive(true);
                AvatarCheckListItems[i].AddToList(data[i]);
            }

            for (int i = need; i < have; i++)
            {
                AvatarCheckListItems[i].gameObject.SetActive(false);
                AvatarCheckListItems[i].RemoveFromList();
                SimplePool.Despawn(AvatarCheckListItems[i].gameObject);
            }

            if (have > need)
            {
                AvatarCheckListItems.RemoveRange(need, have - need);
            }
        }

        private void ClearList()
        {
            foreach (var item in AvatarCheckListItems)
            {
                item.RemoveFromList();
                SimplePool.Despawn(item.gameObject);
            }

            AvatarCheckListItems.Clear();
        }

        private void OnContinueClick()
        {
            AvatarEquipmentControl.Instance?.OnContinueClick();
        }
    }
}