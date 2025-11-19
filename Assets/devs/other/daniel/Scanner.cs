//using UnityEngine;
//using UnityEngine.EventSystems;

//public class Scanner : MonoBehaviour
//{
//    [SerializeField] private float scanrange = 10f;
//    [SerializeField] private LayerMask scanLayer;

//    public System.Action<FishInfo> OnFishScanned;

//    private void Update()
//    {
//        if (Input.GetButtonDown("Fire1"))
//        {
//            TryScan();
//        }
//    }

//    private void TryScan()
//    {
//        if(Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, scanrange, scanLayer))
//        {
//            if(hit.collider.TryGetComponent(out IScannable scannable))
//            {
//                FishInfo info = scannable.GetScanData();
//                OnFishScanned?.Invoke(info);
//            }
//        }
//    }
//}
