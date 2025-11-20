using UnityEngine;
using YawVR;

/// <summary>
/// Sets the YawTracker's orientation based on the GameObject's speed
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class VelocityController : MonoBehaviour
{
    /*
     This script uses the gameObjects's rigidbody's velocity to control the YawTracker
    */
    private YawController yawController;

    private Rigidbody _rigidbody;
    
    [SerializeField] private Vector3 multiplier = new Vector3(3f, 1f, -2f);
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    
    private void Start()
    {
        yawController = YawController.Instance();
    }

    private void FixedUpdate()
    {
        Vector3 vel = transform.InverseTransformVector(_rigidbody.linearVelocity);

        vel.x *= multiplier.x;
        vel.y *= multiplier.y;
        vel.z *= multiplier.z;

        Vector3 v = new Vector3(vel.z, 0f, vel.x);
   
        yawController.TrackerObject.SetRotation(v);
    }
}