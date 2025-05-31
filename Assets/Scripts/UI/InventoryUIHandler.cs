using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIHandler : MonoBehaviour
{
    public static InventoryUIHandler Instance;
    public static ItemUI CarriedItem;

    [SerializeField] private InventoryUISlots[] _inventorySlots;

    [SerializeField] private Transform _draggablesTransform;
    [SerializeField] private ItemUI _itemPrefab;

    [Header("Item List")]
    [SerializeField] InventoryItem[] _itensToSpawn;

    [Header("Debug")]
    [SerializeField] private Button _giveItemBtn;

    private void Awake()
    {
        Instance = this;
        //_giveItemBtn.onClick.AddListener(delegate { SpawnInventoryItem(); });
    }

    public void SpawnInventoryItem(InventoryItem item = null)
    {
        InventoryItem _item = item;
        if(_item == null)
        {
            int random = UnityEngine.Random.Range(0,  _itensToSpawn.Length);
            _item = _itensToSpawn[random];
        }
    
        for (int i = 0; i < _inventorySlots.Length; i++)
        {
            if (_inventorySlots[i].myItem == null)
            {
                Instantiate<ItemUI>(_itemPrefab, _inventorySlots[i].transform).Initialize(_item, _inventorySlots[i]);
                break;
            }
        }
    }

    private void Update()
    {
        if (CarriedItem == null) return;

        CarriedItem.transform.position = Input.mousePosition;
    }

    public void SetCarriedItem(ItemUI item)
    {
        if(CarriedItem != null)
        {
            item.activeSlot.SetItem(CarriedItem);
        }

        CarriedItem = item;
        CarriedItem.canvasGroup.blocksRaycasts = false;
        item.transform.SetParent(_draggablesTransform);
    }
}
