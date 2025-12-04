using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class RotDungDichNacl : MonoBehaviour
{
    public GameObject chaiNaCLDD;
    public GameObject chaiCaCLDDSetupw;
    [Header("========")]
    public GameObject chaiBetadin;
    public GameObject chaiBetadinSetup;
    [Header("========")]
    public Bat batNacl;
    public Bat batBatadin;
    [Header("========")]
    public UnityEvent OnSkipEvent;
    [Header("========")]
    public GameObject tay;
    public GameObject chaiNaclInHand;
    private List<Tween> _tweens = new List<Tween>();
    public Transform target;

    public void Play()
    {
        StartCoroutine(Anim2());
    }

    private IEnumerator Anim2()
    {
        var tween = tay.transform.DOLocalMoveX(1, 1);
        _tweens.Add(tween);
        yield return new WaitForSeconds(1);
        chaiCaCLDDSetupw.SetActive(false);
        chaiNaCLDD.SetActive(false);
        chaiNaclInHand.SetActive(true);
        yield return null;
        var tween1 = tay.transform.DOMove(target.position, 1);
        _tweens.Add(tween1);
        var tween2 = tay.transform.DORotate(target.eulerAngles, 1);
        _tweens.Add(tween2);
        yield return new WaitForSeconds(0.5f);
        batNacl.ShowDD(true);
    }

    private IEnumerator Anim()
    {
        chaiNaCLDD.SetActive(false);
        chaiCaCLDDSetupw.SetActive(true);
        yield return new WaitForSeconds(2);
        batNacl.ShowDD(true);
        chaiCaCLDDSetupw.SetActive(false);
        chaiNaCLDD.SetActive(true);
        yield return new WaitForSeconds(1);
        chaiBetadin.SetActive(false);
        chaiBetadinSetup.SetActive(true);
        yield return new WaitForSeconds(2);
        batBatadin.ShowDD(true);
        chaiBetadin.SetActive(true);
        chaiBetadinSetup.SetActive(false);
    }

    public void Skip()
    {
        OnSkipEvent?.Invoke();
        StopAllCoroutines();
        _tweens.ForEach(i => i.Complete());
    }
}
