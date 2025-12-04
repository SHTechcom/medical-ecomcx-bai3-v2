using System.Collections.Generic;
using _Main.Common.Scripts.Avatar.UI;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Main.Common.Scripts.Avatar
{
    public class AvatarEquipmentCallExample : MonoBehaviour
    {
        public AvatarEquipmentPreset preset;

        [ShowInInspector]
        public List<AvatarEquipment> CurrentClothsEquipments
            => AvatarEquipmentSystem.CurrentClothsEquipments;

        [ShowInInspector]
        public List<AvatarEquipment> CurrentToolAndMedicinesEquipments
            => AvatarEquipmentSystem.CurrentToolsAndMedicinesEquipments;
        
        [Button]
        public void Init()
        {
            AvatarEquipmentSystem.Init();
        }
        
        [Button]
        public void ShowUI(EquipmentType type)
        {
            if(preset == null) return;
            AvatarCheckListUI.Instance.Show(type, preset);
        }

        [Button]
        public void Equip(AvatarEquipment equipment)
        {
            AvatarEquipmentSystem.Equip(equipment);
        }

        [Button]
        public void UnEquip(AvatarEquipment equipment)
        {
            AvatarEquipmentSystem.UnEquip(equipment);
        }
    }
}