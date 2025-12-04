using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Ruavathaobangcu : MonoBehaviour
{
    public Animator animator;
    public RuntimeAnimatorController animCtrl;
    public UnityEvent OnPlayCompleted;

    public void Play()
    {
        animator.runtimeAnimatorController = animCtrl;
        StartCoroutine(CheckAnimCompleted());
    }

    private IEnumerator CheckAnimCompleted()
    {
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        while (info.normalizedTime < 1f)
        {
            yield return null;
        }
        OnPlayCompleted?.Invoke();
    }
}
