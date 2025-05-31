using System;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    [SerializeField] private GameObject _inventoryMenu;
    [SerializeField] private InventoryUIHandler _inventoryUIHandler;
    private Dictionary<InventoryItemData, InventoryItem> m_itemDictionary;
    public List<InventoryItem> Inventory;

    private void Awake()
    {
        Inventory = new List<InventoryItem>();
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
            Inventory.Add(newItem);
            m_itemDictionary.Add(itemData, newItem);
            _inventoryUIHandler.SpawnInventoryItem(newItem);
        }
    }

    public void Remove(InventoryItemData itemData)
    {
        if (m_itemDictionary.TryGetValue(itemData, out InventoryItem value))
        {
            value.RemoveFromStack();
        
            if(value.stackSize <= 0)
            {
                Inventory.Remove(value);
                m_itemDictionary.Remove(itemData);
            }
        }
    }

    public void ToggleInventory()
    {
        _inventoryMenu.gameObject.SetActive(!_inventoryMenu.gameObject.activeSelf);
    }
}
