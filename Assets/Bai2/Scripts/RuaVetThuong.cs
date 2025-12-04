using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class RuaVetThuong : MonoBehaviour
{
    public Transform rightHand;
    public UnityEvent OnSkipEvent;
    private Vector3 targetPos = new Vector3(-0.195700005f, -0.0562000014f, -0.00190000003f);
    private Tween _tween;

    public void Play()
    {
        _tween = rightHand.transform.DOLocalMove(targetPos, 1).SetLoops(6, LoopType.Yoyo);
    }

    public void Skip()
    {
        _tween?.Complete();
        OnSkipEvent?.Invoke();
    }
}
