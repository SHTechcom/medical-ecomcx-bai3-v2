using UnityEngine;

namespace _Main.Common.Scripts.Avatar
{
    [CreateAssetMenu(menuName = "Project/Avatar/Equipment")]
    public class AvatarEquipment : ScriptableObject
    {
        public string key;
        public string equipmentName;
        public EquipmentType type;

        public GameObject modal;
        public float scaleOnUI = 40;
        public Vector3 rotate = new(-90f, 0, 0);
        public Vector3 errorPos = Vector3.zero;
    }

    public enum EquipmentType
    {
        None,
        Cloth,
        ToolAndMedicine,
    }
}