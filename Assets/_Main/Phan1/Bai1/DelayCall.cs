using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace _Main.Phan1.Bai1
{
    public class DelayCall : MonoBehaviour
    {
        public float delay;
        public bool autoCallOnEnable;
        public UnityEvent action;

        private Tween _tween;

        private void OnEnable()
        {
            if (autoCallOnEnable)
            {
                Call(delay);
            }
        }

        private void OnDisable()
        {
            _tween?.Kill();
        }

        public void Call(float delay)
        {
            _tween = DOVirtual.DelayedCall(delay, () => action?.Invoke());
        }
    }
}