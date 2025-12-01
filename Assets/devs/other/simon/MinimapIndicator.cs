using UnityEngine;

/// <summary>
/// Keeps a world-space minimap indicator aligned upright while following a target.
/// Designed to remain a child of a rotating 3D object so position stays correct,
/// while rotation is forcibly reset each frame. Includes height offset above the target.
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

    [Header("Indicator Settings")]
    [SerializeField]
    private float _heightOffset = 10f; // Units above the target to prevent environment collisions

    // --------------------------------------------------------------------
    // Unity Messages
    // --------------------------------------------------------------------

    private void LateUpdate()
    {
        if (_target == null)
        {
            return;
        }

        // Follow target position with height offset
        Vector3 targetPosition = _target.position;
        targetPosition.y += _heightOffset;
        transform.position = targetPosition;

        // Force upright rotation in world space
        if (_minimapCamera != null)
        {
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

    /// <summary>
    /// Sets the height offset of the indicator above the target.
    /// </summary>
    /// <param name="height">Height in world units.</param>
    public void SetHeightOffset(float height)
    {
        _heightOffset = height;
    }
}
