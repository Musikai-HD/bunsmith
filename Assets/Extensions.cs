using UnityEngine;

public static class Extensions
{
    public static int EulerSign(float eulerAngle)
    {
        if (eulerAngle >= -90f && eulerAngle < 90f) return 1;
        if (eulerAngle >= 90f || eulerAngle < -90f) return -1;
        return 0;
    }
}
