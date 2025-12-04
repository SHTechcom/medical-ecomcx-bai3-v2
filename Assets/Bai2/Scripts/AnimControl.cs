using _Main.Phan1.Bai1.Scripts;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class AnimControl : MonoBehaviour
{
    public AnimationCaller caller;
    public Animator animatorCaller;
    public Button playBtn;
    public Button tangtocBtn;
    public Button giamtocBtn;
    public GameObject iconPause;
    public GameObject iconPlay;
    private float speedSaved = 1;
    private bool isPauseed;

    private void Start()
    {
        if (animatorCaller != null)
        {
            if (isPauseed)
            {
                speedSaved = animatorCaller.speed;
                isPauseed = false;
                animatorCaller.speed = 0;
            }
            else
            {
                isPauseed = true;
                animatorCaller.speed = speedSaved;
            }
            if (caller != null)
            {
                iconPause.SetActive(caller.isPaused);
                iconPlay.SetActive(!caller.isPaused);
            }
            if (animatorCaller != null)
            {
                iconPause.SetActive(animatorCaller.speed == 0);
                iconPlay.SetActive(animatorCaller.speed != 0);
            }
        }
        playBtn.onClick.AddListener(() =>
        {
            if (caller != null)
            {
                caller.Play();
                iconPause.SetActive(caller.isPaused);
                iconPlay.SetActive(!caller.isPaused);
            }
            if (animatorCaller != null)
            {
                animatorCaller.speed = speedSaved;
                iconPause.SetActive(animatorCaller.speed == 0);
                iconPlay.SetActive(animatorCaller.speed != 0);
            }
        });
        giamtocBtn.onClick.AddListener(() =>
        {
            if (animatorCaller != null)
            {
                speedSaved -= 0.2f;
                if (speedSaved < 0) speedSaved = 0.2f;
                animatorCaller.speed = speedSaved;
            }
            caller?.IncreaseSpeed();
        });
        tangtocBtn.onClick.AddListener(() =>
        {
            if (animatorCaller != null)
            {
                speedSaved -= 0.2f;
                if (speedSaved < 0) speedSaved = 0.2f;
                animatorCaller.speed = speedSaved;
            }
            caller?.DecreaseSpeed();
        });
    }
}
