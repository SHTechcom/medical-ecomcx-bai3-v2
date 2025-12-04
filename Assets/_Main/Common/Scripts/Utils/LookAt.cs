using System;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    public Transform target;

    private void LateUpdate()
    {
        if (!target) return;
        transform.LookAt(target);
    }

    private void OnValidate()
    {
        if (target != null)
        {
            transform.LookAt(target);
        }
    }
}