using UnityEngine;

namespace TowerDefence.ExtensionMethods
{
    public static class Vector3Extensions
    {
        public static Vector3 WithY(this Vector3 vector, float value)
        {
            return new Vector3(vector.x, value, vector.z);
        }

        public static Vector2 ToVector2XZ(this Vector3 vector)
        {
            return new Vector2(vector.x, vector.z);
        }
    }
}
