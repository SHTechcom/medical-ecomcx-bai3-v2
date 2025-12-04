using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class DialogArrowButton : MonoBehaviour
{
    public Button arrowBtn;
    public RectTransform content;
    private bool isShowing = true;
    private bool isAniming = false;
    public Transform icon;

    private void Start()
    {
        arrowBtn.onClick.AddListener(ShowHide);
    }

    private void ShowHide()
    {
        if (isAniming) return;
        isAniming = true;
        if (isShowing)
        {
            isShowing = !isShowing;
            //hide
            content.DOAnchorPosY(-250, 0.5f).OnComplete(() =>
            {
                isAniming = false;
                icon.localScale = new Vector3(-1, 1, 1);
            });
        }
        else
        {
            isShowing = !isShowing;
            //show
            content.DOAnchorPosY(0, 0.5f).OnComplete(() =>
            {
                isAniming = false;
                icon.localScale = new Vector3(1, 1, 1);
            });
        }
    }
}
