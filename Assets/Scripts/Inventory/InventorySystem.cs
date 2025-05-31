using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    private Dictionary<InventoryItemData, InventoryItem> m_itemDictionary;
    public List<InventoryItem> inventory;

    private void Awake()
    {
        inventory = new List<InventoryItem>();
        m_itemDictionary = new Dictionary<InventoryItemData, InventoryItem>();
    }

    public InventoryItem Get(InventoryItemData itemData)
    {
        if (m_itemDictionary.TryGetValue(itemData, out InventoryItem value))
        {
            return value;
        }

        return null;
    }

    public void Add(InventoryItemData itemData)
    {
        if(m_itemDictionary.TryGetValue(itemData, out InventoryItem value)){
            value.AddToStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(itemData);
            inventory.Add(newItem);
            m_itemDictionary.Add(itemData, newItem);
        }
    }

    public void Remove(InventoryItemData itemData)
    {
        if (m_itemDictionary.TryGetValue(itemData, out InventoryItem value))
        {
            value.RemoveFromStack();
        
            if(value.stackSize <= 0)
            {
                inventory.Remove(value);
                m_itemDictionary.Remove(itemData);
            }
        }
    }
}
