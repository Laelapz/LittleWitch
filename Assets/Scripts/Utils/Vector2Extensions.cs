using UnityEngine;

namespace Utils
{
    public static class Vector2Extensions
    {
        public static Vector3 ToVector3(this Vector2 vector, float? x = null, float y = 0, float? z = null)
        {
            return new Vector3(x ?? vector.x, y, z ?? vector.y);
        }
    }
}