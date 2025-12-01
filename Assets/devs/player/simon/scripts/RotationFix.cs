using UnityEngine;
/// <summary>
/// Provides physical stabilization for a submarine-like Rigidbody.
/// Dampens angular velocity and removes unwanted sideways drift.
/// Attach this to the same GameObject as the Movement script.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class RotationFix : MonoBehaviour
{
    // ------------------------------
    // Instance Fields
    // ------------------------------

    [Header("Stability Settings")]
    [SerializeField]
    private float _angularDamping = 4f;

    [SerializeField]
    private float _driftHardness = 10f;

    private Rigidbody _rigidbody;

    // ------------------------------
    // Unity Messages
    // ------------------------------

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        DampAngularVelocity();
        RemoveSidewaysDrift();
    }

    // ------------------------------
    // Private Methods
    // ------------------------------

    private void DampAngularVelocity()
    {
        // Gradually reduces rotation without freezing rotation axes completely.
        float dampingFactor = Mathf.Clamp01(1f - (Time.fixedDeltaTime * _angularDamping));
        _rigidbody.angularVelocity *= dampingFactor;
    }

    private void RemoveSidewaysDrift()
    {
        // Keeps movement aligned with forward direction.
        Vector3 velocity = _rigidbody.linearVelocity;
        Vector3 forward = transform.forward;

        // Extract only the forward movement.
        Vector3 forwardComponent = Vector3.Project(velocity, forward);
        Vector3 sidewaysComponent = velocity - forwardComponent;

        // Remove drifting gradually for smooth underwater feeling.
        _rigidbody.linearVelocity -= sidewaysComponent * (Time.fixedDeltaTime * _driftHardness);
    }
}
