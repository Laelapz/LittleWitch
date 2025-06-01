using System;
using UnityEngine;


[Serializable]
public class InventoryItem
{
    public InventoryItemData data;
    public int stackSize;
    public int slotIndex = -1;

    public InventoryItem(InventoryItemData source)
    {
        this.data = source;
        AddToStack();
    }

    public void AddToStack()
    {
        stackSize++;
    }

    public void RemoveFromStack()
    {
        stackSize--;
    }

    public void SetSlotIndex(int index)
    {
        slotIndex = index;
    }
}
