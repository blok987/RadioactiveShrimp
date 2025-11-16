using UnityEngine;
using UnityEngine.Animations;

public static class VectorExtension
{
    public static Vector3 WithAxis(this Vector3 vector, Axis axis, float value)
    {
        return new Vector3(axis == Axis.X ? value : vector.x, axis == Axis.Y ? value : vector.y, axis == Axis.Z ? value : vector.z);
    }
}
