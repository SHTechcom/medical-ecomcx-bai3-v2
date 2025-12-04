using System.Collections.Generic;
using Frank;
using UnityEngine;

public class GameViewManager : Singleton<GameViewManager>
{
    [SerializeField] private List<BaseView> views = new List<BaseView>();

    public T GetView<T>() where T : BaseView
    {
        foreach (var view in this.views)
        {
            if (view is T)
            {
                return view as T;
            }
        }

        return default(T);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}