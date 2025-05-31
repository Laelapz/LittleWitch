using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour, IPointerClickHandler
{
    Image itemIcon;
    public CanvasGroup canvasGroup { get; private set; }

    public InventoryItem myItem;
    public InventoryUISlots activeSlot;


    private void Awake()
    {
        GetReferences();
    }

    private void GetReferences()
    {
        if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();
        if (itemIcon == null) itemIcon = GetComponent<Image>();
    }

    public void Initialize(InventoryItem item, InventoryUISlots parent)
    {
        GetReferences();

        activeSlot = parent;
        activeSlot.myItem = this;
        myItem = item;
        itemIcon.sprite = item.data.icon;

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            InventoryUIHandler.Instance.SetCarriedItem(this);
        }
    }
}
