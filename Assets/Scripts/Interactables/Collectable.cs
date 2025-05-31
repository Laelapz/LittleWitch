using UnityEngine;

public class Collectable : MonoBehaviour, IInteractable<PlayerController>
{
    [SerializeField] private InventoryItemData _itemData;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    public void Start()
    {
        _spriteRenderer.sprite = _itemData.icon;
    }

    public void Interact(PlayerController character)
    {
        character.GetItem(_itemData);
        Destroy(this.gameObject);
    }
}
