using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ChooseItem : MonoBehaviour
{
    [System.Serializable]
    public class Item
    {
        public string name;
        public GameObject obj;
        public ToggleItem item;

        public void Init()
        {
            item.SetLabel(name);
            item.OnChange((value) => { obj.gameObject.SetActive(value); });
        }
    }

    public List<Item> items = new List<Item>();
    public UnityEvent OnNext;

    private void Start()
    {
        items.ForEach(i => i.Init());
    }

    public void Next()
    {
        if (items.All(i => i.item.toggle.isOn))
        {
            OnNext?.Invoke();
        }
    }
}
