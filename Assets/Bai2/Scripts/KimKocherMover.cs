using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class KimKocherMover : MonoBehaviour
{
    public Transform target;
    public float yNhungs;
    public Tween _tween;

    public UnityEvent OnNhungsCompleted;
    public UnityEvent OnNhungSkip;
    private Vector3 originalPos;

    private void Start()
    {
        originalPos = transform.position;
    }

    public void PlayAnimNhungs()
    {
        _tween = transform.DOMove(target.position, 1).OnComplete(() =>
        {
            transform.DOMoveY(yNhungs, 0.5f).SetLoops(2, LoopType.Yoyo).OnComplete(() =>
            {
                OnNhungsCompleted?.Invoke();
            });
        });
    }

    public void SkipAnimNhung()
    {
        _tween?.Kill();
        transform.position = originalPos;
        OnNhungsCompleted?.Invoke();
    }
}
