using UnityEngine;

public class ScanController : MonoBehaviour
{
    [SerializeField] private SubmarineScanner scanner;
    [SerializeField] private TargetLockUI ui;
    [SerializeField] private RecentScansManager recentScansManager;

    private void OnEnable()
    {
        scanner.OnTargetLocked += ui.SetTarget;
        scanner.OnTargetLost += ui.ClearTarget;
        scanner.OnFishScanned += HandleFishScanned;
    }

    private void OnDisable()
    {
        scanner.OnTargetLocked -= ui.SetTarget;
        scanner.OnTargetLost -= ui.ClearTarget;
        scanner.OnFishScanned -= HandleFishScanned;
    }

    private void HandleFishScanned(FishInfo fishInfo)
    {
        ui.ShowScan(fishInfo);

        recentScansManager.AddScan(fishInfo);
    }
}
