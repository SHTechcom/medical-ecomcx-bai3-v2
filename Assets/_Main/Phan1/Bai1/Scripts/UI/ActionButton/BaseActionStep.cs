using System;
using _Main.Phan1.Bai1.Scripts.UI;
using _Main.Phan1.Bai1.Scripts.UI.WarningUI;
using _Main.Phan1.Bai1.StepSystem;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace _Main.Phan1.Bai1.Scripts.TaskSystem
{
    [System.Serializable]
    public class RequiredStep
    {
        public ConditionStep requireStep;
        public string warningText;

        public void Deconstruct(out ConditionStep requireStep, out string warningText)
        {
            requireStep = this.requireStep;
            warningText = this.warningText;
        }
    }

    public class BaseActionStep : ConditionStep
    {
        [Title("ACTION STEP")] [SerializeField] private RequiredStep[] requireSteps;

        [SerializeField] private bool addActionToUI = true;
        [SerializeField] private bool isButtonAction = true;
        [SerializeField] private bool isCountCondition = true;

        [Tooltip("True => OnEnable(); False => StartStep()")] [SerializeField] private bool addActionOnEnable;
        [SerializeField] private string actionName;
        [SerializeField] private UnityEvent onCompleteAction;

        private ActionButtonItem _uiItem;

        private void OnEnable()
        {
            if (addActionOnEnable && addActionToUI) AddAction();
        }

        public override void StartStep()
        {
            base.StartStep();
            if (!addActionOnEnable && addActionToUI && conditionCount > 0) AddAction();
        }

        public override void EndStep()
        {
            base.EndStep();
        }

        public void AddAction()
        {
            _uiItem = ActionButtonSpawner.Instance.Add(actionName, CanDoAction, () =>
            {
                if (isButtonAction)
                    CompleteAction();
            });
        }

        public bool CanDoAction()
        {
            if (!isButtonAction) return false;
            if (requireSteps == null || requireSteps.Length == 0) return true;

            foreach (var (step, warningText) in requireSteps)
            {
                if (!step.IsStepCompleted())
                {
                    WarningUI.Instance?.Show(warningText);
                    Debug.Log($"{step.name}: {warningText}");
                    return false;
                }
            }


            return true;
        }

        [Button]
        public virtual void CompleteAction()
        {
            if (isCountCondition) StepConditionController.MinusConditionCount();
            onCompleteAction?.Invoke();
            if (_uiItem != null) ActionButtonSpawner.Instance.Remove(_uiItem);
        }
    }
}