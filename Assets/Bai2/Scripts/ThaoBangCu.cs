using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ThaoBangCu : MonoBehaviour
{
    public Animator animator;
    public RuntimeAnimatorController animThaoBangController;
    private float cacheSpeed = 1;
    public UnityEvent OnPlay1NuaCompleted;
    public UnityEvent OnPlayCompleted;

    private void OnEnable()
    {
        animator.speed = 1;
    }

    public void Play1Nua()
    {
        animator.runtimeAnimatorController = animThaoBangController;
        StartCoroutine(WaitToTimeInAnim(11));
    }

    private IEnumerator WaitToTimeInAnim(float targetTime)
    {
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);

        // đợi đến khi animator thực sự vào state mới
        while (!info.IsName("thaobangcu"))
        {
            yield return null;
            info = animator.GetCurrentAnimatorStateInfo(0);
        }

        // đợi đúng tới khi normalizedTime * length >= targetTime
        while (info.normalizedTime * info.length < targetTime)
        {
            yield return null;
            info = animator.GetCurrentAnimatorStateInfo(0);
        }
        cacheSpeed = animator.speed;
        animator.speed = 0;
        Debug.Log("Đã đến giây thứ 11!");
        OnPlay1NuaCompleted?.Invoke();
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

    private IEnumerator DelayCall(float duration, Action onCompleted)
    {
        yield return new WaitForSeconds(duration);
        cacheSpeed = animator.speed;
        animator.speed = 0;
        onCompleted?.Invoke();
    }

    public void PlayTiep()
    {
        animator.speed = cacheSpeed;
        StartCoroutine(CheckAnimCompleted());
    }
}
