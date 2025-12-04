using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class FitBoxColliderMeshes : MonoBehaviour
{
    public float outsideAdd = 1f;

    [Sirenix.OdinInspector.Button]
    [ContextMenu("GetFitBoxCollider")]
    public void GetFitBoxCollider()
    {
        BoxCollider box = GetComponent<BoxCollider>();

        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();

        if (renderers.Length == 0) return;

        Bounds combinedBounds = renderers[0].bounds;

        for (int i = 1; i < renderers.Length; i++)
        {
            combinedBounds.Encapsulate(renderers[i].bounds);
        }

        Vector3 localCenter = transform.InverseTransformPoint(combinedBounds.center);
        Vector3 localSize = transform.InverseTransformVector(combinedBounds.size);

        box.center = localCenter;
        box.size = localSize + outsideAdd * Vector3.one;
    }
    
    [Sirenix.OdinInspector.Button]
    [ContextMenu("GetFitBoxCollider")]
    public void GetFitBoxCollider(float outside)
    {
        BoxCollider box = GetComponent<BoxCollider>();

        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();

        if (renderers.Length == 0) return;

        Bounds combinedBounds = renderers[0].bounds;

        for (int i = 1; i < renderers.Length; i++)
        {
            combinedBounds.Encapsulate(renderers[i].bounds);
        }

        Vector3 localCenter = transform.InverseTransformPoint(combinedBounds.center);
        Vector3 localSize = transform.InverseTransformVector(combinedBounds.size);

        box.center = localCenter;
        box.size = localSize + outside * Vector3.one;
    }

    [Sirenix.OdinInspector.Button]
    [ContextMenu("Remove")]
    public void Remove()
    {
        if (Application.isPlaying)
        {
            Destroy(this);
        }
        else
        {
            DestroyImmediate(this);
        }
    }
}