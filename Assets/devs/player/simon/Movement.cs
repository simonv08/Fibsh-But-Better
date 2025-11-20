using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handles player movement, steering wheel rotation input, and smooth view reset.
/// </summary>
public class Movement : MonoBehaviour
{
    // ------------------------------
    // Instance Fields
    // ------------------------------

    private PlayerInput _input;
    private bool _isResetting = false;

    [SerializeField]
    private float _moveSpeed = 5f;

    [SerializeField]
    private float _rotationSpeed = 100f;

    [SerializeField]
    private float _resetSpeed = 5f;

    // ------------------------------
    // Unity Messages
    // ------------------------------

    private void Awake()
    {
        _input = new PlayerInput();
    }

    private void OnEnable()
    {
        _input.SteeringWheel.Enable();
        _input.LeftHandle.Enable();
        _input.RightHandle.Enable();
        _input.SwitchPanel.Enable();
        _input.StarterSwitch.Enable();
    }

    private void OnDisable()
    {
        _input.SteeringWheel.Disable();
        _input.LeftHandle.Disable();
        _input.RightHandle.Disable();
        _input.SwitchPanel.Disable();
        _input.StarterSwitch.Disable();
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
        float forwardValue = _input.SteeringWheel.Forward.ReadValue<float>();
        float backwardValue = _input.SteeringWheel.Backward.ReadValue<float>();
        float moveValue = forwardValue - backwardValue;

        transform.Translate(
            0f,
            0f,
            moveValue * _moveSpeed * Time.deltaTime,
            Space.Self
        );
    }

    private void HandleRotation()
    {
        float rotationValue = _input.SteeringWheel.rotation.ReadValue<float>();

        transform.Rotate(
            0f,
            rotationValue * _rotationSpeed * Time.deltaTime,
            0f,
            Space.Self
        );
    }

    private void HandlePitch()
    {
        // Read trim inputs
        bool trimUp =
            _input.LeftHandle.ElevatorTrimUpLeft.IsPressed() ||
            _input.LeftHandle.ElevatorTrimUpRight.IsPressed();

        bool trimDown =
            _input.LeftHandle.ElevatorTrimDownLeft.IsPressed() ||
            _input.LeftHandle.ElevatorTrimDownRight.IsPressed();

        float pitchDirection = 0f;

        if (trimUp)
        {
            pitchDirection = -1f; // negative X = pitch nose up
        }
        else if (trimDown)
        {
            pitchDirection = 1f; // positive X = pitch nose down
        }

        transform.Rotate(
            pitchDirection * _rotationSpeed * Time.deltaTime,
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

        // stop when close enough
        if (Quaternion.Angle(transform.rotation, Quaternion.identity) < 0.5f)
        {
            transform.rotation = Quaternion.identity;
            _isResetting = false;
        }
    }
}
