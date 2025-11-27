using UnityEngine;

/// <summary>
/// Keeps a world-space minimap indicator aligned upright while following a target.
/// Designed to remain a child of a rotating 3D object so position stays correct,
/// while rotation is forcibly reset each frame.
/// </summary>
public class MinimapIndicator : MonoBehaviour
{
    // --------------------------------------------------------------------
    // Instance Fields
    // --------------------------------------------------------------------

    [SerializeField]
    private Transform _target;

    [SerializeField]
    private Transform _minimapCamera;

    // --------------------------------------------------------------------
    // Unity Messages
    // --------------------------------------------------------------------

    private void LateUpdate()
    {
        if (_target == null)
        {
            return;
        }

        // Follows target position, unaffected by parent rotation
        transform.position = _target.position;

        // Forces upright rotation in world space
        if (_minimapCamera != null)
        {
            // Makes indicator align with minimap camera’s orientation
            transform.rotation = Quaternion.Euler(90.0f, _minimapCamera.eulerAngles.y, 0.0f);
        }
        else
        {
            transform.rotation = Quaternion.Euler(90.0f, 0.0f, 0.0f);
        }
    }

    // --------------------------------------------------------------------
    // Public Methods
    // --------------------------------------------------------------------

    /// <summary>
    /// Sets the target that the minimap indicator follows.
    /// </summary>
    /// <param name="newTarget">The transform to follow.</param>
    public void SetTarget(Transform newTarget)
    {
        _target = newTarget;
    }

    /// <summary>
    /// Sets the minimap camera used for orientation alignment.
    /// </summary>
    /// <param name="cameraTransform">The minimap camera transform.</param>
    public void SetMinimapCamera(Transform cameraTransform)
    {
        _minimapCamera = cameraTransform;
    }
}
