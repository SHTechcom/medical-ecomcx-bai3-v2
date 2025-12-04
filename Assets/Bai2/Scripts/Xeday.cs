using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class Xeday : MonoBehaviour
{
    public Vector3 targetPos;
    public UnityEvent OnCompleted;
    private Tween tween;

    public void MoveToGate()
    {
        tween = transform.DOLocalMove(targetPos, 1).OnComplete(() =>
        {
            OnCompleted?.Invoke();
        });
    }

    public void Skip()
    {
        tween?.Complete();
    }
}
