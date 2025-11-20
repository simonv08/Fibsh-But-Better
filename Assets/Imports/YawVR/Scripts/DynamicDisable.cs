using UnityEngine;

namespace YawVR
{
    public class DynamicDisable : MonoBehaviour
    {
        [SerializeField] private bool yaw1;
        [SerializeField] private bool yaw2;
        [SerializeField] private bool yaw3;
        
        private void Start()
        {
            YawController.OnConnectReceivers.Add(OnConnected);
        }
        
        private void OnConnected()
        {
            if (YawController.Instance() == null) return;
            
            switch (YawController.Instance().Device.type)
            {
                case DeviceType.YAW1:
                    gameObject.SetActive(yaw1);
                    break;
                case DeviceType.YAW2:
                    gameObject.SetActive(yaw2);
                    break;
                case DeviceType.YAW3:
                    gameObject.SetActive(yaw3);
                    break;
            }
        }
    }
}