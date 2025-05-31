using UnityEngine;
using UnityEngine.Events;

public class CollisionDetectionNormal : MonoBehaviour
{
    [Header("Filters")]
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private string LimitByTag = "";

    public UnityEvent CallOnCollisionEnter;
    public UnityEvent<Collision> CallOnCollisionEnterWithCollider;
    public UnityEvent CallOnCollisionExit;
    public UnityEvent<Collision> CallOnCollisionExitWithCollider;

    private void OnCollisionEnter(Collision collision)
    {
        CallEvent(CallOnCollisionEnterWithCollider, collision);
        CallEvent(CallOnCollisionEnter, collision);
    }

    private void OnCollisionExit(Collision collision)
    {
        CallEvent(CallOnCollisionExitWithCollider, collision);
        CallEvent(CallOnCollisionExit, collision);
    }
    private void CallEvent(UnityEvent<Collision> unityEvent, Collision parameter)
    {
        if (_layerMask != 0 && !_layerMask.Includes(parameter.gameObject.layer)) return;
        if (LimitByTag != "" && parameter.collider.tag != LimitByTag) return;
        unityEvent?.Invoke(parameter);
    }

    private void CallEvent(UnityEvent unityEvent, Collision parameter)
    {
        if (_layerMask != 0 && !_layerMask.Includes(parameter.gameObject.layer)) return;
        if (LimitByTag != "" && parameter.collider.tag != LimitByTag) return;
        unityEvent?.Invoke();
    }
}
