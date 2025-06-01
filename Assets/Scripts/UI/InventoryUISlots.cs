using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryUISlots : MonoBehaviour, IPointerClickHandler
{
    public ItemUI myItem;
    public int slotIndex;


    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (InventoryUIHandler.CarriedItem == null) return;
            SetItem(InventoryUIHandler.CarriedItem);
        }
    }

    public void SetItem(ItemUI item)
    {
        InventoryUIHandler.CarriedItem = null;

        item.activeSlot.myItem = null;
        item.myItem.slotIndex = slotIndex;

        myItem = item;
        myItem.activeSlot = this;
        myItem.transform.SetParent(transform);
        myItem.canvasGroup.blocksRaycasts = true;
    }
}

