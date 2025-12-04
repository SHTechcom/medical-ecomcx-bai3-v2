using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Main.Phan1.Bai1.StepSystem
{
    public class StepConditionController : MonoBehaviour
    {
        public static Action<int> SetConditionCountAction;
        public static Action OnConditionCountMinusAction;
        public static Action OnConditionCountPlusAction;

        [ShowInInspector, ReadOnly] int conditionCount;

        private Tween _tween;

        private void Awake()
        {
            SetConditionCountAction += SetConditionCount;
            OnConditionCountMinusAction += OnConditionCountMinus;
            OnConditionCountPlusAction += OnConditionCountPlus;
        }

        private void OnDestroy()
        {
            SetConditionCountAction -= SetConditionCount;
            OnConditionCountMinusAction -= OnConditionCountMinus;
            OnConditionCountPlusAction -= OnConditionCountPlus;
        }

        private void OnDisable()
        {
            _tween?.Kill();
        }

        private void Update()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.F1))
            {
                MinusConditionCount();
            }
            if (Input.GetKeyDown(KeyCode.F2))
            {
                PlusConditionCount();
            }
#endif
        }

        private void SetConditionCount(int count)
        {
            conditionCount = count;
        }

        private void OnConditionCountMinus()
        {
            conditionCount--;
            if (conditionCount == 0)
            {
                ConditionStep.Current?.EndStepAndGoToNextStep();
            }
        }

        private void OnConditionCountPlus()
        {
            conditionCount++;
        }

        private bool IsApplicationPlay() => Application.isPlaying;

        [Button, ShowIf("IsApplicationPlay")]
        public static void MinusConditionCount()
        {
            OnConditionCountMinusAction?.Invoke();
        }

        [Button, ShowIf("IsApplicationPlay")]
        public static void PlusConditionCount()
        {
            OnConditionCountPlusAction?.Invoke();
        }

        [Button, ShowIf("IsApplicationPlay")]
        public void MinusConditionCountAfter(float delay = 0f)
        {
            _tween = DOVirtual.DelayedCall(delay,
                () => OnConditionCountMinusAction?.Invoke());
        }

        [Button, ShowIf("IsApplicationPlay")]
        public void PlusConditionCountAfter(float delay = 0f)
        {
            _tween = DOVirtual.DelayedCall(delay,
                () => OnConditionCountPlusAction?.Invoke());
        }
    }
}