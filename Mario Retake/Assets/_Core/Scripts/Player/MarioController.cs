using System;
using Unity.VisualScripting;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class MarioController : MonoBehaviour, IPlayer
{
    // Fields

    public float moveSpeed;
    public float jumpForce;
    [Range(0, 1)]
    public float airControl;
    public float gravityScale;
    public LayerMask objectInteractLayer;
    public LayerMask entityInteractLayer;

    Rigidbody2D _rb;
    WallDetector _wallDetector;
    GroundDetector _groundDetector;
    bool _jumpTrigger = false;
    bool _wasOnAir = false;
    bool _velocityControlLatch = false;
    Vector3 _previousPosition;
    Vector3 _direction;


    // Properties

    public Vector2 axis { get; private set; }
    public bool jumpTrigger
    {
        get
        {
            if (_jumpTrigger)
            {
                _jumpTrigger = false;
                return true;
            }
            else return false;
        }
        private set
        {
            if (value)
                _jumpTrigger = true;
        }
    }
    public bool wasOnAir
    {
        get
        {
            if (_wasOnAir)
            {
                _wasOnAir = false;
                return true;
            }
            else return false;
        }

        set
        {
            if (value)
                _wasOnAir = true;
        }
    }


    // Lifecycle methods

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _wallDetector = GetComponent<WallDetector>();
        _groundDetector = GetComponent<GroundDetector>();
    }

    private void Start()
    {
        _previousPosition = transform.position;
        _rb.gravityScale = gravityScale;
    }

    private void Update()
    {
        updateInput();
    }


    private void FixedUpdate()
    {
        updateDirection();

        if (axis.x != 0)
        {
            if (!_wallDetector.IsHittingFront || !_wallDetector.IsHittingBack)
                updateTransform();
            _velocityControlLatch = true;
        }
        else
        {
            if (_velocityControlLatch)
            {
                float speed = 0;
                if (_rb.velocity.sqrMagnitude == 0)
                    speed = moveSpeed;
                else
                    speed = _rb.velocity.magnitude;

                _rb.velocity = _direction * speed;
                _velocityControlLatch = false;
            }
        }

        if (jumpTrigger)
            _rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }


    // Private methods

    private void updateDirection()
    {
        _direction = transform.position - _previousPosition;
        _direction.Normalize();
        _previousPosition = transform.position;
    }

    private void updateInput()
    {
        wasOnAir = !_groundDetector.IsGrounded;
        axis = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        jumpTrigger = Input.GetButtonDown("Jump") && _groundDetector.IsGrounded;

        if (axis.y < 0)
            tryCrouching();
        else if (axis.y == 0)
            _rb.gravityScale = gravityScale;

        tryStomping();
    }

    private void updateTransform()
    {
        transform.position += Vector3.right * axis.x * Time.fixedDeltaTime * moveSpeed;
    }

    private void tryCrouching()
    {
        print("Crouch");
        var hits = _groundDetector.GroundHits;

        foreach (var h in hits)
        {
            if (h.collider != null && h.collider.TryGetComponent(out IObject obj))
            {
                obj.OnInteractBegin(new InteractionResult());
            }
        }
        _rb.gravityScale = 8 * gravityScale;
    }

    private void tryStomping()
    {
        if (_groundDetector.IsGrounded && wasOnAir)
        {
            print("Stomp");
            bool shouldKnockback = false;
            var hits = _groundDetector.GroundHits;

            foreach (var h in hits)
            {
                if (h.collider != null && h.collider.TryGetComponent(out IEntity entity))
                {
                    entity.OnInteractBegin(new InteractionResult());
                    shouldKnockback = true;
                }
            }

            if (shouldKnockback)
            {
                _rb.velocity = Vector3.zero;
                _rb.AddForce(Vector2.up * jumpForce * 0.5f, ForceMode2D.Impulse);
            }
        }
    }


    // Interfaces

    public void OnInteractBegin(InteractionResult res)
    {

    }

    public void OnInteractEnd(InteractionResult res)
    {

    }
}
