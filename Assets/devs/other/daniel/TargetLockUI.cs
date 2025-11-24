using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class TargetLockUI : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject lockPanel;
    [SerializeField] private GameObject scanPanel;

    [Header("UI")]
    [SerializeField] private TMP_Text fishName;
    [SerializeField] private TMP_Text fishDescription;
    [SerializeField] private Image fishImage;

    private Transform currentTarget;

    private void Awake()
    {
        lockPanel.SetActive(false);
        scanPanel.SetActive(false);
    }

    public void SetTarget(IScannable scannable)
    {
        currentTarget = ((MonoBehaviour)scannable).transform;
        lockPanel.SetActive(true);
    }

    public void ClearTarget()
    {
        currentTarget= null;
        lockPanel.SetActive(false);
        scanPanel.SetActive(false);
    }

    private void Update()
    {
        if (currentTarget != null)
        {
            FollowTarget();
        }
    }

    private void FollowTarget()
    {
        Vector3 screenPos = mainCamera.WorldToScreenPoint(currentTarget.position);
        lockPanel.transform.position = screenPos;
    }

    public void ShowScan(FishInfo info)
    {
        fishName.text = info.fishName;
        fishDescription.text = info.fishDescription;
        fishImage.sprite = info.image;

        scanPanel.SetActive(true);
    }
}
