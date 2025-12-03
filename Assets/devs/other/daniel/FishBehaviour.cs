using UnityEngine;

public class FishBehaviour : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 2f;
    public float turnSpeed = 2f;
    public float wanderRadius = 6f;

    [Header("Obstacle Avoidance")]
    public float rayDistance = 2f;
    public float avoidanceStrength = 3f;

    private Vector3 originPos;
    private Vector3 targetPos;

    void Start()
    {
        originPos = transform.position;
        PickNewTarget();
    }

    void Update()
    {
        Vector3 moveDir = (targetPos - transform.position).normalized;

        // check forward ray for collision
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, rayDistance))
        {
            // steer away from obstacle
            Vector3 avoidDir = Vector3.Reflect(moveDir, hit.normal);
            moveDir = Vector3.Lerp(moveDir, avoidDir, avoidanceStrength * Time.deltaTime);
            targetPos = transform.position + avoidDir * 3f; // shift target dynamically
        }

        // rotation
        if (moveDir != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDir), Time.deltaTime * turnSpeed);

        // movement
        transform.position += transform.forward * speed * Time.deltaTime;

        // if near target  pick new one
        if (Vector3.Distance(transform.position, targetPos) < 0.5f)
            PickNewTarget();
    }

    void PickNewTarget()
    {
        Vector3 randomOffset = Random.insideUnitSphere * wanderRadius;
        randomOffset.y *= 0.35f; // flatten vertical movement slightly
        targetPos = originPos + randomOffset;
    }
}