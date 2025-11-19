//using Unity.Android.Gradle.Manifest;
//using UnityEngine;

//public class FishScanUI : MonoBehaviour
//{
//    [SerializeField] private Scanner scanner;
//    [SerializeField] private TMPro.TextMeshProUGUI nameText;
//    [SerializeField] private TMPro.TextMeshProUGUI descText;
//    [SerializeField] private UnityEngine.UI.Image fishImage;
//    [SerializeField] private GameObject panel;

//    private void OnEnable()
//    {
//        scanner.OnFishScanned += ShowFishInfo;
//    }

//    private void OnDisable()
//    {
//        scanner.OnFishScanned -= ShowFishInfo;
//    }

//    private void ShowFishInfo(FishInfo info)
//    {
//        nameText.text = info.fishName;
//        descText.text = info.fishDescription;
//        fishImage.sprite = info.image;

//        panel.SetActive(true);
//    }
//}
