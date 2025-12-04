using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SimpleDropdown : MonoBehaviour
{
    public RectTransform scrollView;
    public SimpleItemDropdown itemPrefab;
    public Transform itemContainer;
    public Button dropdownButton;
    public TMP_Text dropdownText;
    [Header("Stat")]
    public Vector2 originalSize;
    public Vector2 showSize;

    //status
    private bool isAnimShowRuning;
    private bool isShowing;
    private List<SimpleItemDropdown> items = new List<SimpleItemDropdown>();

    private void Start()
    {
        dropdownButton.onClick.AddListener(ShowHideScrollView);
    }

    public void Init(string[] types, Action<string> onSelectOption)
    {
        //create new
        for (int i = 0; i < types.Length; i++)
        {
            var item = Instantiate(itemPrefab, itemContainer);
            items.Add(item);
            item.Init()
                .SetText($"{types}")
                .OnClicked(onSelectOption);
            item.gameObject.SetActive(true);
        }

        //hide if don't use
        if (items.Count > types.Length)
        {
            for (int i = types.Length; i < items.Count; i++)
            {
                items[i].gameObject.SetActive(false);
            }
        }
    }

    private void ShowHideScrollView()
    {
        if (isAnimShowRuning) return;
        isAnimShowRuning = true;
        if (isShowing)
        {
            scrollView.DOSizeDelta(originalSize, 1).OnComplete(() =>
            {
                isAnimShowRuning = false;
                isShowing = false;
            });
        }
        else
        {
            scrollView.DOSizeDelta(showSize, 1).OnComplete(() =>
            {
                isAnimShowRuning = false;
                isShowing = true;
            });
        }
    }
}
