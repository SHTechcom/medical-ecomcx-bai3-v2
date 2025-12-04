using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class DatXoRac : MonoBehaviour
{
    public GameObject poseTayCamXoRac;
    public Transform target;
    public GameObject xorac;
    private Tween tween;
    public UnityEvent OnSkipEvent;
    public UnityEvent OnCompletedEvent;

    public void Play()
    {
        poseTayCamXoRac.gameObject.SetActive(true);
        tween = poseTayCamXoRac.transform.DOMove(target.position, 1).OnComplete(() =>
        {
            xorac.SetActive(true);
            poseTayCamXoRac.SetActive(false);
            OnCompletedEvent?.Invoke();
        });
    }

    public void Skip()
    {
        tween?.Complete();
        OnSkipEvent?.Invoke();
        OnCompletedEvent?.Invoke();
    }
}
