using UnityEngine;

public class Bat : MonoBehaviour
{
    public GameObject dd;

    public void ShowDD(bool isShow)
    {
        dd.SetActive(isShow);
    }
}
