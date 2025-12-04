using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine.Events;

namespace _Main.Phan1.Bai1.StepSystem
{
    [RequireComponent(typeof(StepConditionController))]
    public class LevelControl : MonoBehaviour
    {
        [SerializeField] protected UnityEvent onEndLevel;
        [SerializeField] protected List<Step> stepList;
        
        protected int CurrentStepIndex;

        private Tween _tween;

        private void OnEnable()
        {
            foreach (var step in stepList)
            {
                step.OnEndStep += NextStep;
            }
        }

        private void OnDisable()
        {
            foreach (var step in stepList)
            {
                step.OnEndStep -= NextStep;
            }
        }

        private void Start()
        {
            foreach (var step in stepList)
            {
                step.InitStep();
            }

            StartLevel();
        }

        public virtual void StartLevel()
        {
            CurrentStepIndex = 0;
            stepList[CurrentStepIndex].StartStep();
        }

        public virtual void NextStep()
        {
            if (CurrentStepIndex == stepList.Count - 1)
            {
                EndLevel();
                return;
            }

            CurrentStepIndex++;
            stepList[CurrentStepIndex].StartStep();
        }

        public virtual void EndLevel()
        {
            Debug.Log("End Level");
            onEndLevel?.Invoke();
        }

#if UNITY_EDITOR
        [Button("Get Step On Child")]
        public void AutoSetStep(bool includeInactive)
        {
            stepList = new List<Step>();
            var steps = GetComponentsInChildren<Step>(includeInactive);
            foreach (var step in steps)
            {
                stepList.Add(step);
            }
        }
#endif
    }
}