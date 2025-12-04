using System;
using System.Collections.Generic;
using Frank;
using UnityEngine;

namespace _Main.Phan1.Bai1.Scripts.UI
{
    public class ActionButtonSpawner : Singleton<ActionButtonSpawner>
    {
        [SerializeField] private Transform container;
        [SerializeField] private ActionButtonItem prefab;

        public List<ActionButtonItem> Items = new();

        public ActionButtonItem Add(string name, Func<bool> allow, Action execute)
        {
            var ui = Instantiate(prefab, container);
            ui.Setup(name, allow, execute);
            Items.Add(ui);
            return ui;
        }

        public void Remove(ActionButtonItem item)
        {
            Items.Remove(item);
            item.Remove();
        }

        public void Clear()
        {
            foreach (Transform c in container) Destroy(c.gameObject);
        }
    }
}