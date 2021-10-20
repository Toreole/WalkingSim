using UnityEngine;

public static class Utility
{
    public static void AlignWith(this Transform t, Transform other)
    {
        t.position = other.position;
        t.rotation = other.rotation;
    }
}