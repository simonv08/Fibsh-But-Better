using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class SubmarineScanner : MonoBehaviour
{
    [SerializeField] private float maxDistance = 50f;
    [SerializeField] private LayerMask scanLayer;
    [SerializeField] private Transform raycastOrigin;
    [SerializeField] private GameObject scanner;

    public event System.Action<IScannable> OnTargetLocked;
    public event System.Action<FishInfo> OnFishScanned;
    public event System.Action OnTargetLost;

    private IScannable currentTarget;

    // reference naar jouw input actions
    private Controls controls;

    private void Awake()
    {
        controls = new Controls();
    }

    private void OnEnable()
    {
        controls.Submarine.Enable();
        controls.Submarine.Scan.performed += OnScanPerformed;
    }

    private void OnDisable()
    {
        controls.Submarine.Scan.performed -= OnScanPerformed;
        controls.Submarine.Disable();
    }

    private void OnScanPerformed(InputAction.CallbackContext ctx)
    {
        scanner.SetActive(true);
        StartCoroutine(TryScan());
    }

    private void Update()
    {
        DoRaycast();
    }

    private void DoRaycast()
    {
        if (Physics.SphereCast(raycastOrigin.position, 1.5f, raycastOrigin.forward, out RaycastHit hit, maxDistance, scanLayer))
        {
            if (hit.collider.TryGetComponent(out IScannable scannable))
            {
                if (currentTarget != scannable)
                {
                    currentTarget = scannable;
                    OnTargetLocked?.Invoke(scannable);
                }
                return;
            }
        }

        if (currentTarget != null)
        {
            currentTarget = null;
            OnTargetLost?.Invoke();
        }
    }

    private IEnumerator TryScan()
    {
        yield return new WaitForSeconds(1);
        scanner.SetActive(false);
        if (currentTarget != null)
            OnFishScanned?.Invoke(currentTarget.GetScanData());
    }
}
