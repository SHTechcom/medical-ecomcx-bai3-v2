using UnityEngine;

namespace _Main.Phan1.Bai1.Scripts
{
    public class InspectPointControl : MonoBehaviour
    {
        public float rotateSpeed = 400f;
        public float scaleSpeed = 1f;
        public float minScale = 0.3f;
        public float maxScale = 2f;
        public float minY = -40f;
        public float maxY = 40f;

        float pitch;

        private void Start()
        {
            pitch = transform.localEulerAngles.x;
        }

        private void Update()
        {
            Rotate();
            Scale();
        }

        private void Rotate()
        {
            if (!Input.GetMouseButton(1)) return;

            float dx = -Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime;
            float dy = Input.GetAxis("Mouse Y") * rotateSpeed * Time.deltaTime;

            transform.Rotate(Vector3.up, dx, Space.World);

            pitch += dy;
            pitch = Mathf.Clamp(pitch, minY, maxY);

            Vector3 e = transform.eulerAngles;
            transform.rotation = Quaternion.Euler(pitch, e.y, 0f);
        }

        private void Scale()
        {
            float scroll = Input.mouseScrollDelta.y;
            if (scroll == 0f) return;

            float newScale = transform.localScale.x + scroll * scaleSpeed * Time.deltaTime;
            newScale = Mathf.Clamp(newScale, minScale, maxScale);

            transform.localScale = Vector3.one * newScale;
        }
    }
}