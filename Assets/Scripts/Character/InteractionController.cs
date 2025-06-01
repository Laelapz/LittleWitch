using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    private PlayerController _mediator;
    private CancellationTokenSource _interactionCancelToken;
    [SerializeField] private float _radius;
    private void Awake()
    {
        _mediator = GetComponent<PlayerController>();
    }

    public void Interact()
    {
        var objects = Physics2D.OverlapCircleAll(transform.position, _radius);
        foreach (var selected in objects)
        {
            if (selected.TryGetComponent(out IInteractable<PlayerController> interactable))
            {
                interactable.Interact(_mediator);
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, _radius);
    }

    private void OnDestroy()
    {
    }
}
