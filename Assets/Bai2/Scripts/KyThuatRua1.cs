using UnityEngine;
using UnityEngine.Events;

public class KyThuatRua1 : MonoBehaviour
{
    public KimKocherMover kimKnchor;

    public UnityEvent OnSkipEvent;

    public void Play()
    {
        kimKnchor.gameObject.SetActive(true);
        kimKnchor.PlayAnimNhungs();
    }

    public void Skip()
    {
        kimKnchor.gameObject.SetActive(true);
        OnSkipEvent?.Invoke();
        kimKnchor?.SkipAnimNhung();
    }
}
