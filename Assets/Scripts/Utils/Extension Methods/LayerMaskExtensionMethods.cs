using UnityEngine;

public static class LayerMaskExtensionMethods
{
    public static bool Includes(this LayerMask mask, int layer)
    {
        return (mask.value & 1 << layer) > 0;
    }
}