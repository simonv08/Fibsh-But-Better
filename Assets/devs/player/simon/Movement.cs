using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    private PlayerInput input;
    public float moveSpeed = 5f;
    public float rotationSpeed = 100f;

    private void Awake()
    {
        input = new PlayerInput();   // initializes your SteeringWheel action map
    }

    private void OnEnable()
    {
        input.SteeringWheel.Enable();
    }

    private void OnDisable()
    {
        input.SteeringWheel.Disable();
    }

    void Update()
    {
        float forwardValue = input.SteeringWheel.Forward.ReadValue<float>();
        float backwardValue = input.SteeringWheel.Backward.ReadValue<float>();
        float moveValue = forwardValue - backwardValue;

        transform.Translate(0f, 0f, moveValue * moveSpeed * Time.deltaTime, Space.Self);

        float rotationValue = input.SteeringWheel.rotation.ReadValue<float>();
        transform.Rotate(0f, rotationValue * rotationSpeed * Time.deltaTime, 0f);
    }
}
