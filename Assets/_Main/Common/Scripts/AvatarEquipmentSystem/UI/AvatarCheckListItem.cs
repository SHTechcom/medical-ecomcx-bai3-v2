using System;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

namespace _Main.Common.Scripts.Avatar.UI
{
    public class AvatarCheckListItem : MonoBehaviour
    {
        [SerializeField] private Image checkImage;
        [SerializeField] private TMP_Text itemName;
        [SerializeField] private Button itemButton;

        private AvatarEquipment _avatarEquipment;

        public bool IsEquipped
        {
            get
            {
                if (_avatarEquipment == null) return false;
                return AvatarEquipmentSystem.IsEquipped(_avatarEquipment);
            }
        }

        private void Start()
        {
            itemButton.onClick.AddListener(OnItemClick);
        }

        public void RefreshInfo(AvatarEquipment equipment)
        {
            _avatarEquipment = equipment;
            UpdateVisual();
        }

        public void AddToList(AvatarEquipment equipment)
        {
            RefreshInfo(equipment);
        }

        public void RemoveFromList()
        {
            _avatarEquipment = null;
        }

        public void UpdateVisual()
        {
            if (_avatarEquipment == null) return;

            itemName.text = _avatarEquipment.equipmentName;
            var localizedContent = new LocalizedString("Bai3", _avatarEquipment.equipmentName);
            localizedContent.StringChanged += value =>
            {
                itemName.text = value;
            };
            checkImage.color = IsEquipped ? Color.green : Color.white;
        }

        public void OnEquip(AvatarEquipment equip)
        {
            if (equip == _avatarEquipment)
                UpdateVisual();
        }

        public void OnUnEquip(AvatarEquipment equip)
        {
            if (equip == _avatarEquipment)
                UpdateVisual();
        }

        public void OnItemClick()
        {
            if (_avatarEquipment == null) return;
            AvatarEquipmentControl.Instance.SpawnObject(_avatarEquipment);
        }
    }
}