using System.Collections.Generic;
using UnityEngine;

namespace _Main.Common.Scripts.Avatar
{
    [CreateAssetMenu(menuName = "Project/Avatar/Equipment_Database")]
    public class AvatarEquipmentDatabase : ScriptableObject
    {
        // Để spawn object ở phần chọn đồ
        public List<AvatarEquipment> cloths;
        public List<AvatarEquipment> toolsAndMedicines;

#if UNITY_EDITOR
        [Sirenix.OdinInspector.Button]
        public void QuickSetType()
        {
            if (cloths != null && cloths.Count > 0)
            {
                foreach (var item in cloths)
                {
                    item.type = EquipmentType.Cloth;
                }
            }

            if (toolsAndMedicines != null && toolsAndMedicines.Count > 0)
            {
                foreach (var item in toolsAndMedicines)
                {
                    item.type = EquipmentType.ToolAndMedicine;
                }
            }
        }

        [Sirenix.OdinInspector.Button]
        public void ImportFromPreset(AvatarEquipmentPreset preset)
        {
            cloths = preset.cloths;
            toolsAndMedicines = preset.toolsAndMedicines;
        }
#endif
    }
}