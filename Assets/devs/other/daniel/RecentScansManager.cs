using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecentScansManager : MonoBehaviour
{
    [Header("UI Elements for Recent Scans")]
    [SerializeField] private GameObject recentScanPanel;      // The panel where the recent scans are displayed
    [SerializeField] private Transform scanItemContainer;     // Container for scan items
    [SerializeField] private GameObject scanItemPrefab;       // Prefab to represent each fish in the UI

    private List<FishInfo> recentScans = new List<FishInfo>();

    private void Start()
    {
        // Initialize the UI by clearing all items initially
        ClearScanItems();
    }

    public void AddScan(FishInfo fishInfo)
    {
        // Add the new fish scan to the front of the list
        recentScans.Insert(0, fishInfo);

        // If the list exceeds 4 scans, remove the oldest one
        if (recentScans.Count > 4)
        {
            recentScans.RemoveAt(recentScans.Count - 1);
        }

        // Update the UI with the new list of recent scans
        UpdateRecentScansUI();
    }

    private void UpdateRecentScansUI()
    {
        // Clear existing UI items before updating
        ClearScanItems();

        // Create new UI elements for each fish in the recent scans list
        foreach (var fishInfo in recentScans)
        {
            GameObject scanItem = Instantiate(scanItemPrefab, scanItemContainer);
            TMP_Text nameText = scanItem.GetComponentInChildren<TMP_Text>();
            Image image = scanItem.GetComponentInChildren<Image>();

            nameText.text = fishInfo.fishName;
            image.sprite = fishInfo.image;
        }
    }

    private void ClearScanItems()
    {
        // Destroy all existing scan items (except the prefab itself)
        foreach (Transform child in scanItemContainer)
        {
            Destroy(child.gameObject);
        }
    }
}
