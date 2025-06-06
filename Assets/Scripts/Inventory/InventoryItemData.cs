using UnityEngine;

[CreateAssetMenu(menuName = "Inventory Item Data", order = 0)]
public class InventoryItemData : ScriptableObject
{
    public string id;
    public string displayName;
    public Sprite icon;
    public GameObject prefab;

}
