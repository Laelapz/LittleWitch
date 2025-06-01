using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIHandler : MonoBehaviour, ISaveable
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

    private IDataService _dataService = new JsonDataService();
    private readonly string _inventorySavePath = "/SaveGame/Inventory.json";
    private InventoryData inventoryData = null;


    private void Awake()
    {
        Instance = this;
        //_giveItemBtn.onClick.AddListener(delegate { SpawnInventoryItem(); });
        SetInventorySlots();
        inventoryData = LoadGame<InventoryData>();
        
        foreach(var item in inventoryData.itens)
        {
            SpawnInventoryItem(item);
        }
    }

    private void SetInventorySlots()
    {
        for (int i = 0; i < _inventorySlots.Length; i++)
        {
            _inventorySlots[i].slotIndex = i;
        }
    }

    public void SpawnInventoryItem(InventoryItem item = null)
    {
        InventoryItem _item = item;
        if(_item == null)
        {
            int random = UnityEngine.Random.Range(0,  _itensToSpawn.Length);
            _item = _itensToSpawn[random];
        }
        
        if(item.slotIndex != -1)
        {
            ItemUI spawnedItem = Instantiate<ItemUI>(_itemPrefab, _inventorySlots[item.slotIndex].transform);
            spawnedItem.Initialize(_item, _inventorySlots[item.slotIndex]);
            return;
        }

        for (int i = 0; i < _inventorySlots.Length; i++)
        {
            if (_inventorySlots[i].myItem == null)
            {
                ItemUI spawnedItem = Instantiate<ItemUI>(_itemPrefab, _inventorySlots[i].transform);
                spawnedItem.Initialize(_item, _inventorySlots[i]);
                spawnedItem.myItem.SetSlotIndex(i);
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

    public void SaveGame()
    {
        List<InventoryItem> itensToSave = new List<InventoryItem>();
        foreach(var inventarySlot in _inventorySlots)
        {
            if(inventarySlot.myItem != null)
            {
                itensToSave.Add(inventarySlot.myItem.myItem);
            }
        }

        inventoryData = new InventoryData();
        inventoryData.itens = itensToSave;
        _dataService.SaveData(_inventorySavePath, inventoryData, false);
    }

    public T LoadGame<T>()
    {
        return _dataService.LoadData<T>(_inventorySavePath, false);
    }

    public T LoadGameWithPath<T>(string path)
    {
        throw new NotImplementedException();
    }

    public void DeleteProgress()
    {
        throw new NotImplementedException();
    }

    [Serializable]
    public class InventoryData
    {
        public InventoryData()
        {
            itens = new List<InventoryItem>();
        }

        public List<InventoryItem> itens;
    }
}
