using UnityEngine;
using YawVR;

/// <summary>
/// Sets the YawTracker's orientation based on the GameObject's orientation
/// </summary>
public class SimpleOrientationCopy : MonoBehaviour
{
    private YawController yawController; // reference to YawController

    private void Start()
    {
        yawController = YawController.Instance();
    }
    
    private void FixedUpdate()
    {
        yawController.TrackerObject.SetRotation(transform.localEulerAngles);
    }
}
