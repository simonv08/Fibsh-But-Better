using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
        currentTarget = null;
        lockPanel.SetActive(false);
        // scanPanel.SetActive(false);
    }

    private void Update()
    {
        if (currentTarget != null)
            FollowTarget();
    }

    private void FollowTarget()
    {
        // Make UI follow the fish position in the world
        lockPanel.transform.position = currentTarget.position + Vector3.up * 0.2f;

        // Make UI face the camera
        lockPanel.transform.LookAt(mainCamera.transform);
        lockPanel.transform.Rotate(0, 180, 0);  // because LookAt flips it
    }


    public void ShowScan(FishInfo info)
    {
        fishName.text = info.fishName;
        fishDescription.text = info.fishDescription;
        fishImage.sprite = info.image;

        scanPanel.SetActive(true);  //stays visible until next scan
    }
}
