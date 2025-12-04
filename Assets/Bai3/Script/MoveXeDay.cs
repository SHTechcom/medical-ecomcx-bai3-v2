using UnityEngine;
using DG.Tweening;
public class MoveXeDay : MonoBehaviour
{
    public GameObject nextButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void MoveXe()
    {
        transform.DOMove(new Vector3(1.5f, 0, -1.485f), 3).OnComplete(() =>
        {
            transform.DOMove(new Vector3(1.218f, 0, 2), 3);
            nextButton.SetActive(true);
        });
        transform.DORotate(new Vector3(-90, 0, 0), 6);
    }
}
