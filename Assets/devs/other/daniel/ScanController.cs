using UnityEngine;

public class ScanController : MonoBehaviour
{
    [SerializeField] private SubmarineScanner scanner;
    [SerializeField] private TargetLockUI ui;

    private void OnEnable()
    {
        scanner.OnTargetLocked += ui.SetTarget;
        scanner.OnTargetLost += ui.ClearTarget;
        scanner.OnFishScanned += ui.ShowScan;
    }

    private void OnDisable()
    {
        scanner.OnTargetLocked -= ui.SetTarget;
        scanner.OnTargetLost -= ui.ClearTarget;
        scanner.OnFishScanned -= ui.ShowScan;
    }
}
