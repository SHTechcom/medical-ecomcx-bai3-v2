using UnityEngine;
using UnityEngine.Events;

public class Destroyable : MonoBehaviour
{
    public GameObject[] targets;

    public void OnDestroy()
    {
        foreach (var item in targets)
        {
            Destroy(item.gameObject);
        }
    }
}
