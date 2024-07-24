using System;
using UnityEngine;

// Use action set asset instead of lose InputActions directly on component.
public class MovementController : MonoBehaviour
{
    public Transform CameraTransform;

    public float runSpeed;
    public float rotateSpeed;
    public bool cursorLocked = true;
    ActionInput _actionInput;

    [SerializeField] protected float _jumpForce = 5;


    private Rigidbody _rigidbody;
    private Vector2 m_Rotation;

    public ClassController ClassController
    {
        get;
        set;
    }

    [Header("Player Grounded")]
    [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
    public bool Grounded = true;

    private bool _hasAnimator = false;
    [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
    public float GroundedRadius = 0.28f;

    private Animator _animator;

    // animation IDs
    private int _animIDSpeed;
    private int _animIDGrounded;
    private int _animIDJump;
    private int _animIDFreeFall;
    private int _animIDMotionSpeed;


    private const float _threshold = 0.01f;
    [Tooltip("For locking the camera position on all axis")]
    public bool LockCameraPosition = true;

    [SerializeField] private float BottomClamp = -5;
    [SerializeField] private float TopClamp = 25;

    [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
    public float CameraAngleOverride = 0.0f;

    // timeout deltatime
    private float _jumpTimeoutDelta;
    private float _fallTimeoutDelta;

    [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
    public float FallTimeout = 0.15f;

    [Space(10)]
    [Tooltip("The height the player can jump")]
    public float JumpHeight = 1.2f;

    [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
    public float Gravity = -15.0f;

    [Space(10)]
    [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
    public float JumpTimeout = 0.50f;

    public void OnEnable()
    {
        enabled = true;
        _actionInput = GetComponent<ActionInput>();
        this.CameraTransform = GetComponentInChildren<Camera>(true).transform;
        Cursor.lockState = CursorLockMode.Locked;
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void SetAnimator(Animator animator)
    {
        if (animator == null)
        {
            _hasAnimator = false;
            _animator = null;
            return;
        }
        _animator = animator;
        _hasAnimator = true;
        AssignAnimationIDs();
    }

    private void AssignAnimationIDs()
    {
        _animIDSpeed = Animator.StringToHash("Speed");
        _animIDGrounded = Animator.StringToHash("Grounded");
        _animIDJump = Animator.StringToHash("Jump");
        _animIDFreeFall = Animator.StringToHash("FreeFall");
        _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
    }

    private float _animationBlend;

    private float _targetRotation = 0.0f;
    private float _rotationVelocity;
    [Tooltip("How fast the character turns to face movement direction")]
    [Range(0.0f, 0.3f)]
    public float RotationSmoothTime = 0.12f;

    private void Move()
    {
        var move = _actionInput.move;
        // set target speed based on move speed, sprint speed and if sprint is pressed
        var speed = (move == Vector2.zero) ? 0.0f : runSpeed;
        // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon
       
        // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        // if there is no input, set the target speed to 0
        float inputMagnitude = 1f;
         
        _animationBlend = speed;
        if (_animationBlend < 0.01f) _animationBlend = 0f;

        // normalise input direction
        Vector3 inputDirection = new Vector3(move.x, 0.0f, move.y).normalized;

        // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        // if there is a move input rotate player when the player is moving
        if (move != Vector2.zero)
        {
            var bodyTransform = ClassController.Body.transform;
            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                              CameraTransform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(bodyTransform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                RotationSmoothTime);
            // rotate to face input direction relative to camera position
            bodyTransform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }

        Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
        // move the player
        _rigidbody.MovePosition(transform.position + (targetDirection.normalized * (speed * Time.fixedDeltaTime)));
        // update animator if using character
        if (_hasAnimator)
        {
            _animator.SetFloat(_animIDSpeed, _animationBlend);
            _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
        }
    }

    private void Update()
    {
        GroundCheck();
        Look();
        Jump();
        Move();
    }

    protected void Look()
    {
        if(!cursorLocked)
        {
            return;
        }
        var look = _actionInput.look;
        if (look.sqrMagnitude < 0.01)
            return;
        var scaledRotateSpeed = rotateSpeed * Time.deltaTime;
        m_Rotation.y += look.x * scaledRotateSpeed;
        m_Rotation.x -= look.y * scaledRotateSpeed;
        m_Rotation.x = Mathf.Clamp(m_Rotation.x, BottomClamp, TopClamp);
        CameraTransform.localEulerAngles = new Vector3(m_Rotation.x, CameraTransform.localEulerAngles.y, CameraTransform.localEulerAngles.z);
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, m_Rotation.y, transform.localEulerAngles.z);
    }

    public void Jump()
    {
        if (Grounded)
        {
            if (_hasAnimator)
            {
                _animator.SetBool(_animIDJump, false);
                _animator.SetBool(_animIDFreeFall, false);
            }
            if (_actionInput.jump)
            {
                _rigidbody.AddForce(Vector3.up * Mathf.Clamp(_jumpForce * _rigidbody.mass, 1, 800), ForceMode.Impulse);
                _actionInput.jump = false;
                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDJump, true);
                }
            }
            return;
        }
        if (_hasAnimator)
        {
            _animator.SetBool(_animIDFreeFall, true);
        }
    }

    private void GroundCheck()
    {
        // set sphere position, with offset
        
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - ClassController.GroundedOffset + (GroundedRadius /2),
            transform.position.z);
        DrawSphere(spherePosition, GroundedRadius, Color.red);
        Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, ~LayerMask.GetMask("Player"));
        // update animator if using character
        if (_hasAnimator)
        {
            _animator.SetBool(_animIDGrounded, Grounded);
        }
    }
    public static void DrawSphere(Vector4 pos, float radius, Color color)
    {
        Vector4[] v = s_UnitSphere;
        int len = s_UnitSphere.Length / 3;
        for (int i = 0; i < len; i++)
        {
            var sX = pos + radius * v[0 * len + i];
            var eX = pos + radius * v[0 * len + (i + 1) % len];
            var sY = pos + radius * v[1 * len + i];
            var eY = pos + radius * v[1 * len + (i + 1) % len];
            var sZ = pos + radius * v[2 * len + i];
            var eZ = pos + radius * v[2 * len + (i + 1) % len];
            Debug.DrawLine(sX, eX, color);
            Debug.DrawLine(sY, eY, color);
            Debug.DrawLine(sZ, eZ, color);
        }
    }
    // Sphere with radius of 1
    private static readonly Vector4[] s_UnitSphere = MakeUnitSphere(16);

    private static Vector4[] MakeUnitSphere(int len)
    {
        Debug.Assert(len > 2);
        var v = new Vector4[len * 3];
        for (int i = 0; i < len; i++)
        {
            var f = i / (float)len;
            float c = Mathf.Cos(f * (float)(Math.PI * 2.0));
            float s = Mathf.Sin(f * (float)(Math.PI * 2.0));
            v[0 * len + i] = new Vector4(c, s, 0, 1);
            v[1 * len + i] = new Vector4(0, c, s, 1);
            v[2 * len + i] = new Vector4(s, 0, c, 1);
        }
        return v;
    }
    private void OnApplicationFocus(bool hasFocus)
    {
        SetCursorState(cursorLocked);
    }

    private void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }

    private void OnFootstep(AnimationEvent animationEvent)
    {
       // if (animationEvent.animatorClipInfo.weight > 0.5f)
       // {
       //     if (FootstepAudioClips.Length > 0)
       //     {
       //         var index = Random.Range(0, FootstepAudioClips.Length);
       //         // MTT CHANGE START (for simplicity's sake, playing at the transform's position suffices)
       //         AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.position, FootstepAudioVolume);
       //         // MTT CHANGE END
       //     }
       // }
    }

    private void OnLand(AnimationEvent animationEvent)
    {
       // if (animationEvent.animatorClipInfo.weight > 0.5f)
       // {
       //     // MTT CHANGE START (for simplicity's sake, playing at the transform's position suffices)
       //     AudioSource.PlayClipAtPoint(LandingAudioClip, transform.position, FootstepAudioVolume);
       //     // MTT CHANGE END
       // }
    }
}
