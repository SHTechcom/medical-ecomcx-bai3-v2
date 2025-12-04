using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace _Main.Phan1.Bai1.Scripts.SpiralPoint
{
    public class DisinfectionPoint : MonoBehaviour
    {
        public SphereCollider col;
        public Outline outline;

        [HideInInspector] public bool isChecked = false;

        private void Reset()
        {
            if (col == null) col = GetComponent<SphereCollider>();
            if (outline == null) outline = GetComponent<Outline>();
        }

        public void SetHighlight(bool value)
        {
            if (outline != null) outline.enabled = value;
        }

        public void Check()
        {
            isChecked = true;
            outline.OutlineColor = Color.green;
        }

        public void ResetPoint()
        {
            outline.OutlineColor = Color.yellow;
            isChecked = false;
            SetHighlight(false);
        }
    }
}