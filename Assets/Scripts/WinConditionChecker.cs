using System.Collections.Generic;
using UnityEngine;

public class WinConditionChecker : MonoBehaviour, IInteractable<PlayerController>
{
    [Tooltip("Lista dos itens necess�rios para ganhar o jogo")]
    [SerializeField] private List<InventoryItemData> requiredItems;

    [Tooltip("Refer�ncia ao invent�rio atual")]
    [SerializeField] private InventoryUIHandler inventoryUIHandler;

    [SerializeField] private GameObject victoryPanel;

    private bool hasWon = false;

    private bool HasAllRequiredItems()
    {
        var inventoryItems = inventoryUIHandler != null ? GetCurrentInventoryItems() : new List<InventoryItem>();

        foreach (var requiredItem in requiredItems)
        {
            bool found = false;

            foreach (var item in inventoryItems)
            {
                if (item.data.id == requiredItem.id && item.stackSize > 0)
                {
                    found = true;
                    break;
                }
            }

            if (!found)
                return false;
        }

        return true;
    }

    private List<InventoryItem> GetCurrentInventoryItems()
    {
        List<InventoryItem> items = new List<InventoryItem>();

        foreach (var slot in inventoryUIHandler.GetSlots())
        {
            if (slot.myItem != null && slot.myItem.myItem != null)
            {
                items.Add(slot.myItem.myItem);
            }
        }

        return items;
    }

    private void WinGame()
    {
        Debug.Log("Voc� ganhou o jogo! Parab�ns!");
        victoryPanel.gameObject.SetActive(true);
    }

    public void Interact(PlayerController character)
    {
        if (HasAllRequiredItems())
        {
            WinGame();
        }
        else
        {
            Debug.Log("Ainda n�o tem todos os itens necess�rios!");
        }
    }
}
