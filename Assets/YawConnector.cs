using UnityEngine;
using System.Net;

namespace YawVR
{
    public class NewYawConnector : MonoBehaviour, YawControllerDelegate
    {
        [SerializeField] private string yawIpFallback = "10.0.0.1";
        [SerializeField] private string MacPrefix = "2c:cf:67:a2:d7:29"; // optional filter

        private static NewYawConnector Instance;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            YawController.Instance().ControllerDelegate = this;
            Debug.Log("Assigned ControllerDelegate");

            // Discover devices with timeout (e.g., 5000 ms)
            YawController.Instance().DiscoverDevices(5000);
        }

        private void ConnectViaIp(string ip)
        {
            var device = new YawDevice(IPAddress.Parse(ip), DeviceType.YAW3, 50020, 50010, "001", "DEBUG",
                DeviceStatus.Available);

            YawController.Instance().ConnectToDevice(device,
                () => Debug.Log($"Connected to Yaw at {ip}"),
                err => Debug.LogError("Manual IP connect failed: " + err));
        }

        #region YawControllerDelegate

        public void ControllerStateChanged(ControllerState state)
        {
            Debug.Log("Controller state changed: " + state);
        }

        public void DidFoundDevice(YawDevice device)
        {
            Debug.Log($"Found device: {device.Name} | Status: {device.Status} | IP: {device.IPAddress}");

            // Optional: filter by MAC prefix if your SDK supports it
            // if (!device.MacAddress.StartsWith(MacPrefix, StringComparison.OrdinalIgnoreCase))
            //     return;

            if (YawController.Instance().State == ControllerState.Initial &&
                (device.Status == DeviceStatus.Available || device.Status == DeviceStatus.Unknown))
            {
                string ip = device.IPAddress.ToString();
                ConnectViaIp(ip);
            }
        }

        public void DidDisconnectFrom(YawDevice device)
        {
            Debug.Log("Disconnected from: " + device.Name);
            // Retry discovery after disconnect
            YawController.Instance().DiscoverDevices(5000);
        }

        public void DeviceStoppedFromApp()
        {
            Debug.Log("Device stopped from app");
        }

        public void DeviceStartedFromApp()
        {
            Debug.Log("Device started from app");
        }

        #endregion
    }
}
