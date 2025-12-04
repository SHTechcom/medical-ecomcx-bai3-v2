using System;
using UnityEngine;
using UnityEngine.Events;

public class DragAndInteract : MonoBehaviour
{
    public Transform anchor;
    public Collider targetCol;
    public Camera cam;
    public bool autoDisable;
    public UnityEvent onInteract;
    RaycastHit[] hitBuffer = new RaycastHit[8];

    Collider col;
    bool dragging;
    Vector3 dragOffset;

    private void Awake()
    {
        col = GetComponent<Collider>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (HitSelf())
            {
                Plane plane = new Plane(cam.transform.forward, anchor.position);
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);

                if (plane.Raycast(ray, out float enter))
                    dragOffset = transform.position - ray.GetPoint(enter);

                dragging = true;
            }
        }

        if (dragging && Input.GetMouseButton(0))
        {
            Plane plane = new Plane(cam.transform.forward, anchor.position);
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (plane.Raycast(ray, out float enter))
                transform.position = ray.GetPoint(enter) + dragOffset;
        }

        if (Input.GetMouseButtonUp(0) && dragging)
        {
            dragging = false;
            TryInteract();
        }
    }

    bool HitSelf()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        int count = Physics.RaycastNonAlloc(ray, hitBuffer);

        for (int i = 0; i < count; i++)
            if (hitBuffer[i].transform == transform)
                return true;

        return false;
    }

    private void TryInteract()
    {
        Vector3 dir = transform.position - cam.transform.position;
        Ray ray = new Ray(cam.transform.position, dir.normalized);

        RaycastHit[] hits = new RaycastHit[8];
        int count = Physics.RaycastNonAlloc(ray, hits);

        for (int i = 0; i < count; i++)
        {
            if (hits[i].collider == col) continue;

            if (hits[i].collider == targetCol)
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