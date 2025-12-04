using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace _Main.Phan1.Bai1.StepSystem
{
    public class ConditionStep : Step
    {
        public static ConditionStep Current;

        [Title("STEP SYSTEM")]
        [SerializeField] protected bool autoEnableWhenStartStep = true;
        [SerializeField] protected bool autoDisableWhenEndStep;
        [SerializeField] protected float delayToNextStep;

        public int conditionCount = 1;
        public UnityEvent onStepStarted;
        public UnityEvent onStepEnded;

        protected bool _isStepCompleted = false;

        protected Tween _tween;

        private void OnDestroy()
        {
            _tween?.Kill();
        }

        #region PUBLIC METHOD

        public bool IsStepCompleted() => _isStepCompleted;

        public void CompletedStep()
        {
            _tween?.Kill();
            delayToNextStep = 0;
            Current = this;
            EndStepAndGoToNextStep();
        }

        public override void StartStep()
        {
            if(gameObject == null)
            {
                CompletedStep();
                return;
            }
            if (autoEnableWhenStartStep) gameObject.SetActive(true);
            base.StartStep();
            Current = this;
            StepConditionController.SetConditionCountAction?.Invoke(conditionCount);
            onStepStarted?.Invoke();

            if (conditionCount == 0) EndStepAndGoToNextStep();
        }

        public override void EndStep()
        {
            onStepEnded?.Invoke();

            if (delayToNextStep > 0)
            {
                _tween = DOVirtual.DelayedCall(delayToNextStep, CallEndStep);
            }
            else CallEndStep();

            void CallEndStep()
            {
                base.EndStep();
                if (autoDisableWhenEndStep) gameObject.SetActive(false);
            }
        }

        public void EndStepAndGoToNextStep()
        {
            _isStepCompleted = true;
            EndStep();
        }

        #endregion
    }
}