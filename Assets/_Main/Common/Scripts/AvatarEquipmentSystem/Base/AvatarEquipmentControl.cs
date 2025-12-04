using System;
using System.Collections.Generic;
using _Main.Common.Scripts.Avatar.UI;
using Frank;
using UnityEngine;
using UnityEngine.Events;

namespace _Main.Common.Scripts.Avatar
{
    public class AvatarEquipmentControl : Singleton<AvatarEquipmentControl>
    {
        [Header("Avatar Equipment")] [SerializeField]
        private AvatarEquipmentDatabase database;

        [SerializeField] private AvatarEquipmentPreset preset;
        [SerializeField] private bool acceptMissing = true;
        [SerializeField] private bool acceptExtra = true;
        [SerializeField] private bool showLog = true;
        [SerializeField] private bool startWithCloth = true;

        [Header("REF")] [SerializeField] private Transform spawnTransform;
        [SerializeField] private AvatarCheckListUI avatarCheckListUI;
        [SerializeField] private ChooseEquipmentDragUI chooseEquipmentDragUI;
        [SerializeField] private ChooseEquipmentObjectDrag objectPrefab;

        [SerializeField] private UnityEvent onCompleteChooseEquipment;
        [SerializeField] private UnityEvent onCompleteWearClothes;

        public Action<int> OnMissingCloth;
        public Action<int> OnMissingToolsAndMedicines;
        public Action<int> OnExtraCloth;
        public Action<int> OnExtraToolsAndMedicines;

        private EquipmentType _currentEquipmentTypeChoose;

        public AvatarEquipment CurrentObjectEquipment { get; private set; }

        private Dictionary<AvatarEquipment, ChooseEquipmentObjectDrag> SpawnedObjects = new();

        public Transform GetSpawnTransform() => spawnTransform;

        public List<ColliderPerfect> colliderPerfects = new();

        private void Start()
        {
            ResetAvatarEquipmentSystem();
            AvatarEquipmentSystem.ShowLog = showLog;
            StartChooseFlow();
            foreach (var col in colliderPerfects)
            {
                col.collider.enabled = false;
            }
        }

        public Collider GetCollider(AvatarEquipment equipment)
        {
            foreach (var col in colliderPerfects)
            {
                col.collider.enabled = false;
            }
            return colliderPerfects.Find(x => x.equipment == equipment).collider;
        }

        public void StartChooseFlow()
        {
            if (startWithCloth) ShowClothChooseUI();
            else ShowToolAndMedicineChooseUI();
        }

        public void ResetAvatarEquipmentSystem()
        {
            AvatarEquipmentSystem.Init();
        }

        public void ShowClothChooseUI()
        {
            _currentEquipmentTypeChoose = EquipmentType.Cloth;

            if (AvatarEquipmentSystem.CheckContainPreset(EquipmentType.Cloth, preset))
            {
                OnContinueClick();
                return;
            }

            gameObject.SetActive(true);
            SetupAndShowUI(EquipmentType.Cloth);
        }

        public void ShowToolAndMedicineChooseUI()
        {
            _currentEquipmentTypeChoose = EquipmentType.ToolAndMedicine;

            if (AvatarEquipmentSystem.CheckContainPreset(EquipmentType.ToolAndMedicine, preset))
            {
                CompleteChooseEquipmentPhase();
                return;
            }

            gameObject.SetActive(true);
            SetupAndShowUI(EquipmentType.ToolAndMedicine);
        }

        public void HideChooseEquipmentUI()
        {
            avatarCheckListUI.Hide();
            gameObject.SetActive(false);
        }

        public void SpawnObject(AvatarEquipment equipment)
        {
            if (equipment == null) return;

            if (equipment.modal == null)
            {
                Debug.Log($"ERROR NO MODAL SETUP: {equipment.name}");
                return;
            }

            if (AvatarEquipmentSystem.IsEquipped(equipment)) return;


            if (CurrentObjectEquipment != null)
            {
                DropOut(CurrentObjectEquipment);
                if (CurrentObjectEquipment == equipment)
                {
                    CurrentObjectEquipment = null;
                    return;
                }
            }

            var item = Instantiate(objectPrefab, spawnTransform.parent, false);
            item.transform.position = spawnTransform.position;
            item.Setup(equipment);
            var col = GetCollider(equipment);
            if (col != null) col.enabled = true;
            chooseEquipmentDragUI.SetDropArea(col);
            

            CurrentObjectEquipment = equipment;

            SpawnedObjects[equipment] = item;
        }

        public void DropOut(AvatarEquipment equipment)
        {
            if (SpawnedObjects.ContainsKey(equipment))
            {
                AvatarEquipmentSystem.UnEquip(equipment);
                if (SpawnedObjects[equipment] != null)
                    Destroy(SpawnedObjects[equipment].gameObject);
                SpawnedObjects[equipment] = null;
            }
        }

        public void Equip(AvatarEquipment equipment)
        {
            if (_currentEquipmentTypeChoose == EquipmentType.Cloth)
            {
                if (SpawnedObjects[equipment] != null)
                    Destroy(SpawnedObjects[equipment].gameObject);
                SpawnedObjects[equipment] = null;
            }

            AvatarEquipmentSystem.Equip(equipment);
            CurrentObjectEquipment = null;
        }

        public void UnEquip(AvatarEquipment equipment)
        {
            AvatarEquipmentSystem.UnEquip(equipment);
            CurrentObjectEquipment = equipment;
        }

        public void OnContinueClick()
        {
            switch (_currentEquipmentTypeChoose)
            {
                case EquipmentType.Cloth:
                    HandleEquipmentItems(EquipmentType.Cloth, startWithCloth ? ShowToolAndMedicineChooseUI : CompleteChooseEquipmentPhase,
                        OnMissingCloth, OnExtraCloth);
                    break;

                case EquipmentType.ToolAndMedicine:
                    HandleEquipmentItems(EquipmentType.ToolAndMedicine, startWithCloth ? CompleteChooseEquipmentPhase : ShowClothChooseUI,
                        OnMissingToolsAndMedicines, OnExtraToolsAndMedicines);
                    break;
            }
        }

        private void HandleEquipmentItems(EquipmentType type, Action onContinue, Action<int> onMissing, Action<int> onExtra)
        {
            if (acceptMissing && acceptExtra) onContinue?.Invoke();
            else if (AvatarEquipmentSystem.CheckContainPreset(type, preset))
            {
                onContinue?.Invoke();
                return;
            }

            var missing = AvatarEquipmentSystem.GetMissingItems(type, preset);
            var extra = AvatarEquipmentSystem.GetExtraItems(type, preset);

            if (missing.Count > 0) onMissing?.Invoke(missing.Count);
            if (extra.Count > 0) onExtra?.Invoke(extra.Count);
        }

        private void SetupAndShowUI(EquipmentType type)
        {
            AvatarEquipmentSystem.Init(type);

            avatarCheckListUI.Show(type, preset, database);
            chooseEquipmentDragUI.SetupList(type);
        }

        public void CompleteChooseEquipmentPhase()
        {
            onCompleteChooseEquipment?.Invoke();
            HideChooseEquipmentUI();
        }
    }

    [Serializable]
    public class ColliderPerfect
    {
        public AvatarEquipment equipment;
        public Collider collider;
    }
}