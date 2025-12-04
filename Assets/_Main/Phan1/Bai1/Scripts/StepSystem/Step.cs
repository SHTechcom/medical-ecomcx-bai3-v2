using System;
using UnityEngine;

namespace _Main.Phan1.Bai1.StepSystem
{
    public abstract class Step : MonoBehaviour
    {
        public event Action OnStartStep;
        public event Action OnEndStep;

        public virtual void InitStep()
        {
        }

        public virtual void StartStep()
        {
            Debug.Log($"Start {name}");
            OnStartStep?.Invoke();
        }

        public virtual void EndStep()
        {
            Debug.Log($"End {name}");
            OnEndStep?.Invoke();
        }
    }
}