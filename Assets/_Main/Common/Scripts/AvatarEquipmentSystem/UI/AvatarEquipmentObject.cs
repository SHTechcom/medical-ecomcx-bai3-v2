using System;
using UnityEngine;

namespace _Main.Common.Scripts.Avatar
{
    public class AvatarEquipmentObject : MonoBehaviour, IAvatarEquipmentObject
    {
        [SerializeField] private AvatarEquipment equipmentInfo;
        [SerializeField] private bool autoHide = true;
        
        public string EquipmentName
        {
            get
            {
                if (equipmentInfo == null) return "NULL INFO OBJECT";
                return equipmentInfo.equipmentName;
            }
        }

        public EquipmentType Type
        {
            get
            {
                if (equipmentInfo == null) return EquipmentType.None;
                return equipmentInfo.type;
            }
        }

        public bool IsEquipped
        {
            get
            {
                if (equipmentInfo == null) return false;
                return AvatarEquipmentSystem.IsEquipped(equipmentInfo);
            }
        }

        private void Awake()
        {
            AvatarEquipmentSystem.OnEquipItem += OnEquip;
            AvatarEquipmentSystem.OnUnEquipItem += OnUnEquip;
        }

        private void Start()
        {
            if(autoHide) gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            AvatarEquipmentSystem.OnEquipItem -= OnEquip;
            AvatarEquipmentSystem.OnUnEquipItem -= OnUnEquip;
        }

        public void OnEquip(AvatarEquipment equipment)
        {
            if (equipment == equipmentInfo)
            {
                gameObject.SetActive(true);
            }
        }

        public void OnUnEquip(AvatarEquipment equipment)
        {
            if (equipment == equipmentInfo)
            {
                gameObject.SetActive(false);
            }
        }
    }
}