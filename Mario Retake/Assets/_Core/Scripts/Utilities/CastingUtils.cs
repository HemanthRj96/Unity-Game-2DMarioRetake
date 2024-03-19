using UnityEngine;


public static class CastUtils
{
    public static TType CastRay<TType>(Vector3 origin, Vector2 direction, float distance, LayerMask layerMask) where TType : ICore
    {
        var hit = Physics2D.Raycast(origin, direction, distance, layerMask);
        if (hit.collider != null && hit.collider.TryGetComponent(out TType obj))
            return obj;
        return default(TType);
    }
}