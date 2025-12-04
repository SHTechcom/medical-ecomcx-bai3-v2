using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class RotDDBetamin : MonoBehaviour
{
    public GameObject chaiBetadin;
    public GameObject chaiBetadinSetup;
    public Bat batBatadin;
    [Header("========")]
    public UnityEvent OnSkipEvent;
    [Header("========")]
    public GameObject tay;
    public GameObject chaiInHand;
    private List<Tween> _tweens = new List<Tween>();
    public Transform target;

    public void Play()
    {
        StartCoroutine(Anim2());
    }

    private IEnumerator Anim2()
    {
        var tween = tay.transform.DOLocalMoveX(1.048f, 1);
        _tweens.Add(tween);
        yield return new WaitForSeconds(1);
        chaiBetadinSetup.SetActive(false);
        chaiBetadin.SetActive(false);
        chaiInHand.SetActive(true);
        yield return null;
        var tween1 = tay.transform.DOMove(target.position, 1);
        _tweens.Add(tween1);
        var tween2 = tay.transform.DORotate(target.eulerAngles, 1);
        _tweens.Add(tween2);
        yield return new WaitForSeconds(0.5f);
        batBatadin.ShowDD(true);
    }

    public void Skip()
    {
        OnSkipEvent?.Invoke();
        StopAllCoroutines();
        _tweens.ForEach(i => i.Complete());
    }
}
