using System;
using System.Data.SqlTypes;
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

    Rigidbody2D _rb;
    WallDetector _wallDetector;
    GroundDetector _groundDetector;
    bool _jumpTrigger = false;
    bool _wasOnAir = false;


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
        _rb.gravityScale = gravityScale;
    }

    private void Update()
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



    private void FixedUpdate()
    {
        if (axis.x < 0 && !_wallDetector.IsHittingBack)
            updateTransform();
        else if (axis.x > 0 && !_wallDetector.IsHittingFront)
            updateTransform();

        if (jumpTrigger)
            _rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }


    // Private methods

    private void updateTransform()
    {
        if (_groundDetector.IsGrounded)
            transform.position += Vector3.right * axis.x * Time.fixedDeltaTime * moveSpeed;
        else
            transform.position += Vector3.right * axis.x * Time.fixedDeltaTime * moveSpeed * airControl;
    }

    private void tryCrouching()
    {
        var obj = CastUtils.CastRay<IObject>(transform.position, Vector2.down, 0.05f, objectInteractLayer);
        if (obj != null)
            obj.OnInteractBegin(new InteractionResult());
        _rb.gravityScale = 8 * gravityScale;
    }

    private void tryStomping()
    {
        if (_groundDetector.IsGrounded && wasOnAir)
        {

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
