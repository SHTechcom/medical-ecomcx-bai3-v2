using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Main.Phan1.Bai1.Scripts
{
    public class InspectRotate : MonoBehaviour, IDragHandler
    {
        public enum RotateMode
        {
            Update,
            MouseDrag,
            IDragHandler
        }

        public RotateMode mode = RotateMode.Update;

        public float speed = 200f;
        public float minX = -40f;
        public float maxX = 40f;

        [Header("IF TRANSFORM = NULL => GET CURRENT")]
        public Transform targetTransform;

        private float rotX;
        private float rotY;

        private void Start()
        {
            if (targetTransform == null) targetTransform = transform;

            rotY = targetTransform.eulerAngles.y;
            rotX = targetTransform.eulerAngles.x;
        }

        private void Update()
        {
            if (!CanRotate()) return;
            if (mode != RotateMode.Update) return;

            float dx = Input.GetAxis("Mouse X");
            float dy = Input.GetAxis("Mouse Y");

            ApplyRotation(dx, dy);
        }

        private void OnMouseDrag()
        {
            if (!CanRotate()) return;
            if (mode != RotateMode.MouseDrag) return;

            float dx = Input.GetAxis("Mouse X");
            float dy = Input.GetAxis("Mouse Y");

            ApplyRotation(dx, dy);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!CanRotate()) return;
            if (mode != RotateMode.IDragHandler) return;

            float dx = eventData.delta.x * 0.01f;
            float dy = eventData.delta.y * 0.01f;

            ApplyRotation(dx, dy);
        }

        private bool CanRotate()
        {
            return targetTransform.gameObject.activeSelf;
        }

        private void ApplyRotation(float dx, float dy)
        {
            rotY -= dx * speed * Time.deltaTime;
            rotX -= dy * speed * Time.deltaTime;
            rotX = Mathf.Clamp(rotX, minX, maxX);

            targetTransform.rotation = Quaternion.Euler(rotX, rotY, 0f);
        }
    }
}