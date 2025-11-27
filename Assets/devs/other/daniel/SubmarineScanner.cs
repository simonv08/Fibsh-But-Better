using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handles scanning logic for detecting scannable targets.
/// </summary>
public class SubmarineScanner : MonoBehaviour
{
    // ------------------------------
    // Instance Fields
    // ------------------------------

    [SerializeField]
    private float _maxDistance = 50f;

    [SerializeField]
    private LayerMask _scanLayer;

    [SerializeField]
    private Transform _raycastOrigin;

    [SerializeField]
    private GameObject _scanner;

    private PlayerInput _input;
    private IScannable _currentTarget;

    // ------------------------------
    // Events
    // ------------------------------

    public event System.Action<IScannable> OnTargetLocked;
    public event System.Action<FishInfo> OnFishScanned;
    public event System.Action OnTargetLost;

    // ------------------------------
    // Unity Messages
    // ------------------------------

    private void Awake()
    {
        _input = new PlayerInput();
    }

    private void OnEnable()
    {
        _input.LeftHandle.Enable();
        _input.LeftHandle.ScanButton.performed += OnScanPerformed;
    }

    private void OnDisable()
    {
        _input.LeftHandle.ScanButton.performed -= OnScanPerformed;
        _input.LeftHandle.Disable();
    }

    private void Update()
    {
        DoRaycast();
    }

    // ------------------------------
    // Private Methods
    // ------------------------------

    private void OnScanPerformed(InputAction.CallbackContext context)
    {
        _scanner.SetActive(true);
        StartCoroutine(TryScan());
    }

    private void DoRaycast()
    {
        if (Physics.SphereCast(
                _raycastOrigin.position,
                1.5f,
                _raycastOrigin.forward,
                out RaycastHit hit,
                _maxDistance,
                _scanLayer))
        {
            if (hit.collider.TryGetComponent(out IScannable scannable))
            {
                if (_currentTarget != scannable)
                {
                    _currentTarget = scannable;
                    OnTargetLocked?.Invoke(scannable);
                }
                return;
            }
        }

        if (_currentTarget != null)
        {
            _currentTarget = null;
            OnTargetLost?.Invoke();
        }
    }

    private IEnumerator TryScan()
    {
        yield return new WaitForSeconds(1f);

        _scanner.SetActive(false);

        if (_currentTarget != null)
        {
            OnFishScanned?.Invoke(_currentTarget.GetScanData());
        }
    }
}
