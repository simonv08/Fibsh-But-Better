using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handles smooth underwater-style player movement, rotation, pitch trimming, 
/// and view resetting. Uses smoothed acceleration and deceleration for all axes.
/// </summary>
public class Movement : MonoBehaviour
{
    // ------------------------------
    // Instance Fields
    // ------------------------------

    [Header("Movement Settings")]
    [SerializeField]
    private float _moveSpeed = 5f;

    [SerializeField]
    private float _acceleration = 4f;

    [SerializeField]
    private float _deceleration = 2f;

    [Header("Rotation Settings")]
    [SerializeField]
    private float _rotationSpeed = 100f;

    [SerializeField]
    private float _rotationSmooth = 4f;

    [Header("Pitch Settings")]
    [SerializeField]
    private float _pitchSmooth = 4f;

    [Header("View Reset")]
    [SerializeField]
    private float _resetSpeed = 5f;

    private PlayerInput _input;
    private bool _isResetting;

    private float _currentMove;
    private float _currentRotation;
    private float _currentPitch;

    // ------------------------------
    // Unity Messages
    // ------------------------------

    private void Awake()
    {
        _input = new PlayerInput();
    }

    private void OnEnable()
    {
        _input.Enable();
    }

    private void OnDisable()
    {
        _input.Disable();
    }

    private void Update()
    {
        HandleMovement();
        HandleRotation();
        HandlePitch();
        HandleViewReset();
    }

    // ------------------------------
    // Private Methods
    // ------------------------------

    private void HandleMovement()
    {
        float forwardInput = _input.SteeringWheel.Forward.ReadValue<float>();
        float backwardInput = _input.SteeringWheel.Backward.ReadValue<float>();
        float targetMove = forwardInput - backwardInput;

        // Smooth acceleration/deceleration.
        float smoothing = Mathf.Abs(targetMove) > 0.01f ? _acceleration : _deceleration;
        _currentMove = Mathf.Lerp(_currentMove, targetMove, Time.deltaTime * smoothing);

        transform.Translate(
            0f,
            0f,
            _currentMove * _moveSpeed * Time.deltaTime,
            Space.Self
        );
    }

    private void HandleRotation()
    {
        float targetRotation = _input.SteeringWheel.rotation.ReadValue<float>();

        _currentRotation = Mathf.Lerp(
            _currentRotation,
            targetRotation,
            Time.deltaTime * _rotationSmooth
        );

        transform.Rotate(
            0f,
            _currentRotation * _rotationSpeed * Time.deltaTime,
            0f,
            Space.Self
        );
    }

    private void HandlePitch()
    {
        bool trimUp =
            _input.LeftHandle.ElevatorTrimUpLeft.IsPressed() ||
            _input.LeftHandle.ElevatorTrimUpRight.IsPressed();

        bool trimDown =
            _input.LeftHandle.ElevatorTrimDownLeft.IsPressed() ||
            _input.LeftHandle.ElevatorTrimDownRight.IsPressed();

        float targetPitch = 0f;
        if (trimUp)
        {
            targetPitch = -1f;
        }
        else if (trimDown)
        {
            targetPitch = 1f;
        }

        _currentPitch = Mathf.Lerp(
            _currentPitch,
            targetPitch,
            Time.deltaTime * _pitchSmooth
        );

        transform.Rotate(
            _currentPitch * _rotationSpeed * Time.deltaTime,
            0f,
            0f,
            Space.Self
        );
    }

    private void HandleViewReset()
    {
        if (_input.RightHandle.ResetView.triggered)
        {
            _isResetting = true;
        }

        if (!_isResetting)
        {
            return;
        }

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.identity,
            Time.deltaTime * _resetSpeed
        );

        // Stop when close to forward orientation.
        if (Quaternion.Angle(transform.rotation, Quaternion.identity) < 0.5f)
        {
            transform.rotation = Quaternion.identity;
            _isResetting = false;
        }
    }
}
