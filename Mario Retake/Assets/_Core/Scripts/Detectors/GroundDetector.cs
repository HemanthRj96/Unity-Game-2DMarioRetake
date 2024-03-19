using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    // Fields

    public Collider2D groundCollider;
    public LayerMask groundLayer;
    public float raycastDistance;

    ContactFilter2D _contactFilter;
    List<RaycastHit2D> _groundHits = new List<RaycastHit2D>();


    // Properties

    public bool IsGrounded { get; private set; }
    public List<RaycastHit2D> GroundHits { get { return _groundHits; } }


    // Lifecycle methods

    private void Awake()
    {
        _contactFilter.SetLayerMask(groundLayer);
    }

    private void Update()
    {
        updateCollision();
    }


    // Private methods

    private void updateCollision()
    {
        if (groundCollider != null)
            IsGrounded = groundCollider.Cast
                (
                    transform.right,
                    _contactFilter,
                    _groundHits,
                    raycastDistance
                ) > 0;
        else
        {
            IsGrounded = false;
            _groundHits = null;
        }
    }
}
