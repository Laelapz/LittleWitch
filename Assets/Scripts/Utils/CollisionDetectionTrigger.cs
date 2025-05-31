using UnityEngine;
using UnityEngine.Events;

public class CollisionDetectionTrigger : MonoBehaviour
{
    [Header("Filters")]
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private string LimitByTag = "";

    [Space(10f)]
    public UnityEvent<Collider> CallOnTriggerEnter;
    public UnityEvent<Collider> CallOnTriggerExit;
    public UnityEvent<GameObject> CallOnTriggerEnterWithObject;
    public UnityEvent<GameObject> CallOnTriggerExitWithObject;

    private void OnTriggerEnter(Collider other)
    {
        CallEvent(CallOnTriggerEnter, other);
        CallEvent(CallOnTriggerEnterWithObject, other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        CallEvent(CallOnTriggerExit, other);
        CallEvent(CallOnTriggerExitWithObject, other.gameObject);
    }

    private void CallEvent(UnityEvent<Collider> unityEvent, Collider parameter)
    {
        if (_layerMask != 0 && !_layerMask.Includes(parameter.gameObject.layer)) return;
        if (LimitByTag != "" && parameter.tag != LimitByTag) return;
        unityEvent?.Invoke(parameter);
    }

    private void CallEvent(UnityEvent<GameObject> unityEvent, GameObject parameter)
    {
        if (_layerMask != 0 && !_layerMask.Includes(parameter.gameObject.layer)) return;
        if (LimitByTag != "" && parameter.tag != LimitByTag) return;
        unityEvent?.Invoke(parameter);
    }
}