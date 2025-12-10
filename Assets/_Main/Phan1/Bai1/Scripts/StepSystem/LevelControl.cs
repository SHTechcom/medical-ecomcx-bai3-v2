using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace _Main.Phan1.Bai1.StepSystem
{
    [RequireComponent(typeof(StepConditionController))]
    public class LevelControl : MonoBehaviour
    {
        [SerializeField] protected UnityEvent onEndLevel;
        [SerializeField] protected List<Step> stepList;
        protected int CurrentStepIndex;

        private Tween _tween;

        private void OnDisable()
        {
            if (CurrentStepIndex >= 0 && CurrentStepIndex < stepList.Count)
            {
                stepList[CurrentStepIndex].OnEndStep -= NextStep;
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

        public void Replay()
        {
            SceneManager.LoadScene(0);
        }    

        public virtual void StartLevel()
        {
            CurrentStepIndex = 0;
            SubscribeCurrentStep();
            stepList[CurrentStepIndex].StartStep();
        }

        public virtual void NextStep()
        {
            UnsubscribeCurrentStep();

            if (CurrentStepIndex == stepList.Count - 1)
            {
                EndLevel();
                return;
            }

            CurrentStepIndex++;
            SubscribeCurrentStep();
            stepList[CurrentStepIndex].StartStep();
        }

        private void SubscribeCurrentStep()
        {
            if (CurrentStepIndex >= 0 && CurrentStepIndex < stepList.Count)
            {
                stepList[CurrentStepIndex].OnEndStep += NextStep;
            }
        }

        private void UnsubscribeCurrentStep()
        {
            if (CurrentStepIndex >= 0 && CurrentStepIndex < stepList.Count)
            {
                stepList[CurrentStepIndex].OnEndStep -= NextStep;
            }
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