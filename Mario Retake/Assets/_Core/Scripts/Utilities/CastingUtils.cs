using UnityEngine;


public static class CastUtils
{
    public static TType CastRay<TType>(Vector3 origin, Vector2 direction, float distance, LayerMask layerMask)
    {
        var hit = Physics2D.Raycast(origin, direction, distance, layerMask);
        if (hit.collider != null && hit.collider.TryGetComponent(out TType obj))
            return obj;
        return default(TType);
    }

    public static RaycastHit2D[] CastCollider(Collider2D collider, Vector2 direction, float distance, LayerMask layerMask)
    {
        ContactFilter2D contactFilter = new ContactFilter2D();
        RaycastHit2D[] hits = new RaycastHit2D[] { };
        contactFilter.SetLayerMask(layerMask);

        int hitCount = collider.Cast(direction, contactFilter, hits, distance);

        if (hitCount == 0)
            return null;
        else
            return hits;
    }
}