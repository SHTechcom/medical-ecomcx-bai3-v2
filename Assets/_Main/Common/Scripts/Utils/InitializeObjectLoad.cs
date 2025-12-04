using System;
using UnityEngine;

public class InitializeObjectLoad : MonoBehaviour
{
    [SerializeField] private GameObject[] objs;
    [SerializeField] private GameObject[] preloadObjs;

    private void Awake()
    {
        foreach (var obj in objs)
        {
            Instantiate(obj);
        }

        foreach (var preloadObj in preloadObjs)
        {
            Instantiate(preloadObj);
            preloadObj.SetActive(false);
        }
    }
}