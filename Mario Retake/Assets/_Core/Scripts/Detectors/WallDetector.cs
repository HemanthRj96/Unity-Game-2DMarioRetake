using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WallDetector : MonoBehaviour
{
    // Fields

    public Collider2D frontWallCollider;
    public Collider2D backWallCollider;
    public LayerMask wallLayer;
    public float raycastDistance;

    ContactFilter2D _contactFilter;
    List<RaycastHit2D> _frontWallHits = new List<RaycastHit2D>();
    List<RaycastHit2D> _backWallHits = new List<RaycastHit2D>();


    // Properties

    public bool IsHittingFront { get; private set; }
    public bool IsHittingBack { get; private set; }
    public List<RaycastHit2D> FrontWallHits { get { return _frontWallHits; } }
    public List<RaycastHit2D> BackWallHits { get { return _backWallHits; } }


    // Lifecycle methods

    private void Awake()
    {
        _contactFilter.SetLayerMask(wallLayer);
    }

    private void Update()
    {
        updateCollision();
    }


    // Private methods

    private void updateCollision()
    {
        if (frontWallCollider != null)
            IsHittingFront = frontWallCollider.Cast
                (
                    transform.right,
                    _contactFilter,
                    _frontWallHits,
                    raycastDistance
                ) > 0;
        else
        {
            IsHittingFront = false;
            _frontWallHits = null;
        }

        if (backWallCollider != null)
            IsHittingBack = backWallCollider.Cast
                (
                    transform.right * -1, 
                    _contactFilter, 
                    _backWallHits, 
                    raycastDistance
                ) > 0;
        else
        {
            IsHittingBack = false;
            _backWallHits = null;
        }
    }
}
