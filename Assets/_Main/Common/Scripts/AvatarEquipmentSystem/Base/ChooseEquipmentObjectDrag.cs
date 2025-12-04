using UnityEngine;
using UnityEngine.Rendering;

namespace _Main.Common.Scripts.Avatar
{
    public class ChooseEquipmentObjectDrag : MonoBehaviour
    {
        [SerializeField] private Transform modalParent;
        [SerializeField] private FitBoxColliderMeshes fitBoxColliderMeshes;

        public AvatarEquipment Equipment { get; private set; }

        public void Setup(AvatarEquipment equipment)
        {
            ClearChild();
            Equipment = equipment;

            var modalSpawn = SimplePool.Spawn(equipment.modal);
            modalSpawn.transform.SetParent(modalParent, false);
            modalSpawn.transform.localScale = Vector3.one;
            modalParent.localScale = equipment.scaleOnUI * Vector3.one;
            modalParent.localEulerAngles = equipment.rotate;
            modalSpawn.transform.position -= equipment.errorPos;
            fitBoxColliderMeshes.GetFitBoxCollider(2);

            MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
            foreach (var r in renderers)
            {
                r.shadowCastingMode = ShadowCastingMode.Off;
            }
        }

        private void ClearChild()
        {
            if (modalParent.childCount > 0)
            {
                for (int i = modalParent.childCount - 1; i >= 0; i--)
                {
                    Destroy(modalParent.GetChild(i).gameObject);
                }
            }
        }
    }
}