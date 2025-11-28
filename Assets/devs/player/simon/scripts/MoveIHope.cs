using UnityEngine;

public class WheelControl : MonoBehaviour
{
    public PlayerInput input;  // Drag your auto-generated input class here
    public float rotationSpeed = 100f;
    public float forwardSpeed = 5f;
    public float resetSpeed = 2f;

    void OnEnable()
    {
        input.SteeringWheel.Enable();
    }

    void OnDisable()
    {
        input.SteeringWheel.Disable();
    }

    void Update()
    {
        // --- ROTATION USING THE WHEEL AXIS ---
        float wheelX = input.SteeringWheel.rotation.ReadValue<float>();
        transform.Rotate(0, wheelX * rotationSpeed * Time.deltaTime, 0);

        // --- FORWARD MOTION USING "Forward" AXIS ---
        float forward = input.SteeringWheel.Forward.ReadValue<float>();
        transform.Translate(Vector3.forward * forward * forwardSpeed * Time.deltaTime);

        // --- RESET WITH F KEY ---
        // if (Keyboard.current.fKey.isPressed)
        // {
        //     transform.rotation = Quaternion.Slerp(
        //         transform.rotation,
        //         Quaternion.identity,
        //         Time.deltaTime * resetSpeed
        //     );
        // }
    }
}
