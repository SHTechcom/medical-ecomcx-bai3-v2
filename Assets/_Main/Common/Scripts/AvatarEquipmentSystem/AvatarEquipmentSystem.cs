using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Main.Common.Scripts.Avatar
{
    public class AvatarEquipmentSystem
    {
        public static List<AvatarEquipment> CurrentClothsEquipments { get; private set; } = new();
        public static List<AvatarEquipment> CurrentToolsAndMedicinesEquipments { get; private set; } = new();

        // Đăng ký sự kiện cần phải gọi hủy sự kiện tránh lỗi
        public static Action<AvatarEquipment> OnEquipItem;
        public static Action<AvatarEquipment> OnUnEquipItem;

        public static bool ShowLog = false;

        public AvatarEquipmentSystem()
        {
            Init();
        }

        public AvatarEquipmentSystem(AvatarEquipmentPreset preset)
        {
            Init();
            QuickApplyPreset(preset);
        }

        public static void Init()
        {
            CurrentClothsEquipments = new List<AvatarEquipment>();
            CurrentToolsAndMedicinesEquipments = new List<AvatarEquipment>();
        }

        public static void Init(EquipmentType type)
        {
            if (type == EquipmentType.Cloth) CurrentClothsEquipments = new List<AvatarEquipment>();
            if (type == EquipmentType.ToolAndMedicine) CurrentToolsAndMedicinesEquipments = new List<AvatarEquipment>();
        }

        /// <summary>
        /// Equip item, có thể gọi lần nữa để un equip
        /// </summary>
        /// <param name="item">so item</param>
        public static void Equip(AvatarEquipment item)
        {
            if (item == null)
            {
                LogWarning($"Equip failed: item null");
            }

            switch (item.type)
            {
                case EquipmentType.Cloth:
                    InternalEquip(item, CurrentClothsEquipments);
                    break;
                case EquipmentType.ToolAndMedicine:
                    InternalEquip(item, CurrentToolsAndMedicinesEquipments);
                    break;
                default:
                    LogWarning($"Equip failed: unknown type {item.type}");
                    break;
            }
        }

        /// <summary>
        /// Un equip item
        /// </summary>
        /// <param name="item">so item</param>
        public static void UnEquip(AvatarEquipment item)
        {
            if (item == null)
            {
                LogWarning("UnEquip failed: item null");
                return;
            }

            switch (item.type)
            {
                case EquipmentType.Cloth:
                    InternalUnEquip(item, CurrentClothsEquipments);
                    break;
                case EquipmentType.ToolAndMedicine:
                    InternalUnEquip(item, CurrentToolsAndMedicinesEquipments);
                    break;
            }
        }

        /// <summary>
        /// Kiểm tra xem item có đang được equip hay ko
        /// </summary>
        /// <param name="item">so item</param>
        /// <returns>true/false</returns>
        public static bool IsEquipped(AvatarEquipment item)
        {
            switch (item.type)
            {
                case EquipmentType.Cloth:
                    return CurrentClothsEquipments.Contains(item);
                case EquipmentType.ToolAndMedicine:
                    return CurrentToolsAndMedicinesEquipments.Contains(item);
            }

            return false;
        }

        /// <summary>
        /// Check valid tất cả các equip type so với preset
        /// </summary>
        /// <param name="preset">so preset</param>
        /// <returns>true/false</returns>
        public static bool CheckValidAllPreset(AvatarEquipmentPreset preset)
        {
            if (preset == null) return true;

            bool clothValid = CheckValidPreset(EquipmentType.Cloth, preset);
            bool toolValid = CheckValidPreset(EquipmentType.ToolAndMedicine, preset);

            bool allValid = clothValid && toolValid;

            if (allValid) Log("Preset matches current equipment");
            else Log("Preset does not match current equipment");

            return allValid;
        }

        /// <summary>
        /// Check valid các đồ theo type so với preset
        /// Trả về false nếu có đồ thừa hoặc thiếu, các trường hợp còn lại trả về true (bao gồm cả trường hợp preset rỗng)
        /// </summary>
        /// <param name="type">EquipmentType</param>
        /// <param name="preset">so preset</param>
        /// <returns>true/false</returns>
        public static bool CheckValidPreset(EquipmentType type, AvatarEquipmentPreset preset)
        {
            if (preset == null) return false;

            switch (type)
            {
                case EquipmentType.Cloth:
                    return CheckListMatch("Cloths", preset.cloths, CurrentClothsEquipments);
                case EquipmentType.ToolAndMedicine:
                    return CheckListMatch("Tools And Medicines", preset.toolsAndMedicines, CurrentToolsAndMedicinesEquipments);
            }

            return false;
        }


        /// <summary>
        /// Check valid các đồ theo type so với preset
        /// Trả về false nếu có ko đủ đồ trong preset, các trường hợp còn lại trả về true (bao gồm cả trường hợp preset rỗng, có đồ thừa)
        /// </summary>
        /// <param name="type">EquipmentType</param>
        /// <param name="preset">so preset</param>
        /// <returns>true/false</returns>
        public static bool CheckContainPreset(EquipmentType type, AvatarEquipmentPreset preset)
        {
            if (preset == null) return false;

            switch (type)
            {
                case EquipmentType.Cloth:
                    return CheckListContain("Cloths", preset.cloths, CurrentClothsEquipments);
                case EquipmentType.ToolAndMedicine:
                    return CheckListContain("Tools And Medicines", preset.toolsAndMedicines, CurrentToolsAndMedicinesEquipments);
            }

            return false;
        }

        /// <summary>
        /// Apply nhanh preset (Force equip)
        /// </summary>
        /// <param name="preset">so preset</param>
        public static void QuickApplyPreset(AvatarEquipmentPreset preset)
        {
            if (preset == null) return;

            ClearEquipment();

            if (preset.cloths != null)
            {
                foreach (var item in preset.cloths) Equip(item);
            }

            if (preset.toolsAndMedicines != null)
            {
                foreach (var item in preset.toolsAndMedicines) Equip(item);
            }

            Log("Preset applied.");
        }

        /// <summary>
        /// Lấy danh sách item thiếu theo type và so với preset, nếu preset null trả về null
        /// </summary>
        /// <param name="type">EquipmentType</param>
        /// <param name="preset">so preset</param>
        /// <returns>Danh sách đồ thiếu</returns>
        public static List<AvatarEquipment> GetMissingItems(EquipmentType type, AvatarEquipmentPreset preset)
        {
            if (preset == null) return null;

            switch (type)
            {
                case EquipmentType.Cloth:
                    return GetMissingItems(preset.cloths, CurrentClothsEquipments);
                case EquipmentType.ToolAndMedicine:
                    return GetMissingItems(preset.toolsAndMedicines, CurrentToolsAndMedicinesEquipments);
            }

            return null;
        }

        /// <summary>
        /// Lấy danh sách item thừa theo type và so với preset, nếu preset null trả về null
        /// </summary>
        /// <param name="type">EquipmentType</param>
        /// <param name="preset">so preset</param>
        /// <returns>Danh sách đồ thừa</returns>
        public static List<AvatarEquipment> GetExtraItems(EquipmentType type, AvatarEquipmentPreset preset)
        {
            if (preset == null) return null;

            switch (type)
            {
                case EquipmentType.Cloth:
                    return GetExtraItems(preset.cloths, CurrentClothsEquipments);
                case EquipmentType.ToolAndMedicine:
                    return GetExtraItems(preset.toolsAndMedicines, CurrentToolsAndMedicinesEquipments);
            }

            return null;
        }

        /// <summary>
        /// Clear danh sách item đang equip
        /// </summary>
        public static void ClearEquipment()
        {
            CurrentClothsEquipments.Clear();
            CurrentToolsAndMedicinesEquipments.Clear();
        }

        private static void InternalEquip(AvatarEquipment item, List<AvatarEquipment> list)
        {
            if (item == null) return;

            if (!list.Contains(item))
            {
                list.Add(item);
                OnEquipItem?.Invoke(item);
                Log($"Equipped {item.equipmentName}");
            }
        }

        private static void InternalUnEquip(AvatarEquipment item, List<AvatarEquipment> list)
        {
            if (item == null) return;
            if (list.Remove(item))
            {
                OnUnEquipItem?.Invoke(item);
                Log($"UnEquipped {item.equipmentName}");
            }
        }

        private static bool CheckListMatch(string label, List<AvatarEquipment> presetList, List<AvatarEquipment> currentList)
        {
            var missingItems = GetMissingItems(presetList, currentList);

            var extraItems = GetExtraItems(presetList, currentList);

            if (missingItems.Count == 0 && extraItems.Count == 0)
            {
                Log($"{label}: Match");
                return true;
            }

            if (missingItems.Count > 0) LogWarning($"{label} missing: {string.Join(", ", missingItems)}");
            if (extraItems.Count > 0) LogWarning($"{label} extra: {string.Join(", ", extraItems)}");

            return false;
        }

        private static bool CheckListContain(string label, List<AvatarEquipment> presetList, List<AvatarEquipment> currentList)
        {
            foreach (var item in presetList)
            {
                if (!currentList.Contains(item)) return false;
            }

            return true;
        }

        private static List<AvatarEquipment> GetMissingItems(List<AvatarEquipment> presetList, List<AvatarEquipment> currentList)
        {
            var missingItems = new List<AvatarEquipment>();
            foreach (var item in presetList)
            {
                if (!currentList.Contains(item))
                    missingItems.Add(item);
            }

            return missingItems;
        }

        private static List<AvatarEquipment> GetExtraItems(List<AvatarEquipment> presetList, List<AvatarEquipment> currentList)
        {
            var extraItems = new List<AvatarEquipment>();
            foreach (var item in currentList)
            {
                if (!presetList.Contains(item))
                    extraItems.Add(item);
            }

            return extraItems;
        }

        #region HELPER

        private static void Log(object o)
        {
            if (!ShowLog) return;
            Debug.Log($"[AVATAR EQUIPMENT SYSTEM]: {o}");
        }

        private static void LogWarning(object o)
        {
            if (!ShowLog) return;
            Debug.LogWarning($"[AVATAR EQUIPMENT SYSTEM]: {o}");
        }

        private static void LogError(object o)
        {
            if (!ShowLog) return;
            Debug.LogError($"[AVATAR EQUIPMENT SYSTEM]: {o}");
        }

        private static void Log(object o, UnityEngine.Object context)
        {
            if (!ShowLog) return;
            Debug.Log($"[AVATAR EQUIPMENT SYSTEM]: {o}", context);
        }

        private static void LogWarning(object o, UnityEngine.Object context)
        {
            if (!ShowLog) return;
            Debug.LogWarning($"[AVATAR EQUIPMENT SYSTEM]: {o}", context);
        }

        private static void LogError(object o, UnityEngine.Object context)
        {
            if (!ShowLog) return;
            Debug.LogError($"[AVATAR EQUIPMENT SYSTEM]: {o}", context);
        }

        #endregion
    }
}