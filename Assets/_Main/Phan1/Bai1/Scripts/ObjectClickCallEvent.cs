using System;
using UnityEngine;
using UnityEngine.Events;

namespace _Main.Phan1.Bai1.Scripts
{
    public class ObjectClickCallEvent : MonoBehaviour
    {
        public Camera cam;
        public Collider targetCol;
        public bool autoDisable;

        public UnityEvent onInteract;

        RaycastHit[] hitBuffer = new RaycastHit[8];

        private void Awake()
        {
            if (targetCol == null) targetCol = GetComponent<Collider>();
            if (targetCol == null) gameObject.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                TryInteract();
            }
        }

        private void TryInteract()
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            int count = Physics.RaycastNonAlloc(ray, hitBuffer);

            for (int i = 0; i < count; i++)
            {
                if (hitBuffer[i].collider == targetCol)
                {
                    Interact();
                    break;
                }
            }
        }

        private void Interact()
        {
            onInteract?.Invoke();
            if (autoDisable) gameObject.SetActive(false);
        }

        private void OnDrawGizmos()
        {
            if (cam == null) return;
            if (targetCol != null)
            {
                Gizmos.color = Color.red;
                Vector3 dir = transform.position - cam.transform.position;
                Gizmos.DrawRay(cam.transform.position, dir);
            }
        }
    }
}