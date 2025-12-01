using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Smooth submarine-style movement using velocity-based motion.
/// Fixes collision drifting, stabilizes rotation, and supports yaw/pitch trim.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour
{
    private const float RESET_THRESHOLD = 0.5f;

    [Header("Movement Settings")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _acceleration = 4f;
    [SerializeField] private float _deceleration = 2f;

    [Header("Rotation Settings")]
    [SerializeField] private float _rotationSpeed = 100f;
    [SerializeField] private float _rotationSmooth = 4f;

    [Header("Pitch Settings")]
    [SerializeField] private float _pitchSmooth = 4f;

    [Header("Stability")]
    [SerializeField] private float _angularDamping = 4f;

    [Header("View Reset")]
    [SerializeField] private float _resetSpeed = 5f;

    private PlayerInput _input;
    private Rigidbody _rigidbody;

    private bool _isResetting;

    private float _currentMove;
    private float _currentYaw;
    private float _currentPitch;

    private void Awake()
    {
        _input = new PlayerInput();
        _rigidbody = GetComponent<Rigidbody>();

        _rigidbody.useGravity = false;
        _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
    }

    private void OnEnable() => _input.Enable();
    private void OnDisable() => _input.Disable();

    private void Update()
    {
        HandleRotation();
        HandleViewReset();
        HandleMovementInput();
    }

    private void FixedUpdate()
    {
        ApplyMovementVelocity();
        DampAngularVelocity();
        RemoveSidewaysDrift();
    }

    // ---------------------------------------------------------
    // Movement (Velocity-based)
    // ---------------------------------------------------------

    private void HandleMovementInput()
    {
        float forward = _input.SteeringWheel.Forward.ReadValue<float>();
        float backward = _input.SteeringWheel.Backward.ReadValue<float>();
        float target = forward - backward;

        float smoothing = (Mathf.Abs(target) > 0.01f)
            ? _acceleration
            : _deceleration;

        _currentMove = Mathf.Lerp(_currentMove, target, Time.deltaTime * smoothing);
    }

    private void ApplyMovementVelocity()
    {
        Vector3 forwardVel = transform.forward * (_currentMove * _moveSpeed);

        Vector3 velocity = _rigidbody.velocity;

        // Keep only the forward component of velocity
        Vector3 forwardComponent = Vector3.Project(velocity, transform.forward);

        // Smoothly move toward the desired forward velocity
        Vector3 newForward = Vector3.Lerp(
            forwardComponent,
            forwardVel,
            Time.fixedDeltaTime * _acceleration
        );

        _rigidbody.velocity = newForward;
    }

    // ---------------------------------------------------------
    // Rotation
    // ---------------------------------------------------------

    private void HandleRotation()
    {
        float yaw = ComputeYaw();
        float pitch = ComputePitch();

        Quaternion deltaRot = Quaternion.Euler(pitch, yaw, 0f);
        _rigidbody.MoveRotation(_rigidbody.rotation * deltaRot);
    }

    private float ComputeYaw()
    {
        float targetYaw = _input.SteeringWheel.rotation.ReadValue<float>();

        _currentYaw = Mathf.Lerp(
            _currentYaw,
            targetYaw,
            Time.deltaTime * _rotationSmooth
        );

        return _currentYaw * _rotationSpeed * Time.deltaTime;
    }

    private float ComputePitch()
    {
        bool up =
            _input.LeftHandle.ElevatorTrimUpLeft.IsPressed() ||
            _input.LeftHandle.ElevatorTrimUpRight.IsPressed();

        bool down =
            _input.LeftHandle.ElevatorTrimDownLeft.IsPressed() ||
            _input.LeftHandle.ElevatorTrimDownRight.IsPressed();

        float targetPitch = up ? -1f : down ? 1f : 0f;

        _currentPitch = Mathf.Lerp(
            _currentPitch,
            targetPitch,
            Time.deltaTime * _pitchSmooth
        );

        return _currentPitch * _rotationSpeed * Time.deltaTime;
    }

    // // ---------------------------------------------------------
    // // Stability
    // // ---------------------------------------------------------
    //
    // private void DampAngularVelocity()
    // {
    //     // Soft angular damping without freezing rotation
    //     _rigidbody.angularVelocity *=
    //         Mathf.Clamp01(1f - (Time.fixedDeltaTime * _angularDamping));
    // }
    //
    // private void RemoveSidewaysDrift()
    // {
    //     Vector3 velocity = _rigidbody.velocity;
    //
    //     // Keep ONLY the forward velocity component
    //     Vector3 forwardComponent = Vector3.Project(velocity, transform.forward);
    //
    //     // Hard drift removal — no drifting after collisions
    //     _rigidbody.velocity = forwardComponent;
    // }

    // ---------------------------------------------------------
    // View Reset
    // ---------------------------------------------------------

    private void HandleViewReset()
    {
        if (_input.RightHandle.ResetView.triggered)
            _isResetting = true;

        if (!_isResetting)
            return;

        Quaternion target = Quaternion.identity;

        Quaternion smoothed = Quaternion.Slerp(
            _rigidbody.rotation,
            target,
            Time.deltaTime * _resetSpeed
        );

        _rigidbody.MoveRotation(smoothed);

        if (Quaternion.Angle(_rigidbody.rotation, target) < RESET_THRESHOLD)
        {
            _rigidbody.rotation = target;
            _isResetting = false;
        }
    }
}
