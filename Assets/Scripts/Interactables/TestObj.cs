using UnityEngine;

public class TestObj : MonoBehaviour, IInteractable<PlayerController>
{
    [SerializeField] private GameObject _messageToShow;

    public void Interact(PlayerController character)
    {
        _messageToShow.SetActive(true);
    }
}
