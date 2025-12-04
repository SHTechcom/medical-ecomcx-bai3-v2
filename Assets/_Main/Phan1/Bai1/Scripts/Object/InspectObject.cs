using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

namespace _Main.Phan1.Bai1.Scripts
{
    public class InspectObject : MonoBehaviour
    {
        public Transform inspectPoint;
        public float moveTime = 0.4f;
        
        public UnityEvent onInspect;
        public UnityEvent onBack;

        Color baseColor;

        Transform oldParent;
        Vector3 oldPos;
        Quaternion oldRot;
        public bool IsInspected { get; private set; }

        private void Awake()
        {
            CaptureOriginal();
        }

        private void OnMouseDown()
        {
            MoveToInspect();
            InspectUI.Instance.Show(this);
        }

        private void CaptureOriginal()
        {
            oldParent = transform.parent;
            oldPos = transform.position;
            oldRot = transform.rotation;
        }

        public void MoveToInspect()
        {
            IsInspected = true;
            onInspect?.Invoke();
            transform.SetParent(inspectPoint);
            transform.DOLocalMove(Vector3.zero, moveTime);
            transform.DOLocalRotateQuaternion(Quaternion.identity, moveTime);
        }

        public void Back()
        {
            IsInspected = false;
            onBack?.Invoke();
            transform.SetParent(oldParent);
            transform.DOMove(oldPos, moveTime);
            transform.DORotateQuaternion(oldRot, moveTime);
        }
    }
}