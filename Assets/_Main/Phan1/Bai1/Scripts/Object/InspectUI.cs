using System;
using UnityEngine;
using UnityEngine.UI;

namespace _Main.Phan1.Bai1.Scripts
{
    public class InspectUI : MonoBehaviour
    {
        public static InspectUI Instance;

        public GameObject panel;
        public Button backBtn;
        InspectObject current;

        private bool canInspect = true;

        private void Awake()
        {
            Instance = this;
            backBtn.onClick.AddListener(OnBack);
        }

        private void Start()
        {
            panel.SetActive(false);
        }

        public void Show(InspectObject obj)
        {
            if (!canInspect) return;
            if (current == obj && current.IsInspected) return;
            if (current != null) current.Back();
            current = obj;
            panel.SetActive(true);
        }

        public void OnBack()
        {
            if (current == null) return;
            current.Back();
            panel.SetActive(false);
            current = null;
        }

        public void SetCanInspect(bool canInspect)
        {
            this.canInspect = canInspect;
        }
    }
}