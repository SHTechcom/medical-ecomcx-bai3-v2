using UnityEngine;

public class TimeSpeed : MonoBehaviour
{
    public float speed = 1;

    public void TangToc()
    {
        if (speed >= 3) return;
        speed += 0.5f;
        Time.timeScale = speed;
    }

    public void GiamToc()
    {
        if (speed <= 0) return;
        speed -= 0.5f;
        Time.timeScale = speed;
    }
}
