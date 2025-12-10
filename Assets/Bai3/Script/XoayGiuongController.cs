using System.Collections;
using UnityEngine;

public class XoayGiuongController : MonoBehaviour
{
    Animation anim;
    public GameObject nextBtn;
    void Start()
    {
        anim = GetComponent<Animation>();
    }

    public void PlayForward()
    {
        //anim["Take 001"].speed = 1f;
        anim.Play("Bai03_anim_dieuchinhgiuongnanglen_tradinhkhang");
        StartCoroutine(waitActiveNextBtn());
    }

    IEnumerator waitActiveNextBtn()
    {
        yield return new WaitForSeconds(3);
        nextBtn.SetActive(true);
    }
    void PlayReverse()
    {
        anim["Run"].speed = -1f;
        anim["Run"].time = anim["Run"].length;
        anim.Play("Run");
    }
}
