using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameInitializerHelper : MonoBehaviour
{
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _startButton;
    private IDataService _dataService = new JsonDataService();
    private readonly string _inventorySavePath = "/SaveGame/Inventory.json";
    
    void Awake()
    {
        try
        {
        }
        catch (Exception ex)
        {
            Debug.LogError($"Sem save. Botão de Continuar deve ficar inativo: {ex.Message}");
        }
    }

    private T LoadGame<T>()
    {
        return _dataService.LoadData<T>(_inventorySavePath, false);
    }

    public void DeleteProgress()
    {
        _dataService.DeleteData(_inventorySavePath);
    }
}
