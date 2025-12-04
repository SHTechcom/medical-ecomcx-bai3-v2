using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DanhGiaVetThuong : MonoBehaviour
{
    [SerializeField] private List<Toggle> items = new List<Toggle>();
    public UnityEvent OnNextEvent;

    public void Next()
    {
        if (items.All(i => i.isOn))
        {
            OnNextEvent?.Invoke();
        }
    }
}
