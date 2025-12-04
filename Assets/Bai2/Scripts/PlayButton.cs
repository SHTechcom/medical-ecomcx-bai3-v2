using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    [SerializeField] private Button btn;
    [SerializeField] private Sprite iconPlay;
    [SerializeField] private Sprite iconPause;
    [SerializeField] private Image icon;

    public UnityEvent onclicked;

    public void SetIcon(bool isPlaying)
    {

    }
}
