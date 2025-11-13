using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

namespace YawVR
{
    public class SampleYawControllerDelegateImplementation : MonoBehaviour, YawControllerDelegate
    {
        // YAW device settings ui elements
        [Header("UI Elements")]
        [SerializeField] private Text setupTitleLabel;
        [SerializeField] private GameObject deviceListScrollViewContent;
        [SerializeField] private GameObject deviceListItemPrefab;
        [SerializeField] private Button connectButton;
        [SerializeField] private Button disconnectButton;
        [SerializeField] private InputField ipAddressInputField;
        [SerializeField] private InputField udpPortInputField;
        [SerializeField] private InputField tcpPortInputField;
        [SerializeField] private Text errorText;
        
        [Header("Settings Panel")]
        [SerializeField] private GameObject settingsPanel;
        [SerializeField] private GameObject buttonSample;
        
        //DeviceDiscovery deviceDiscovery = new DeviceDiscovery();
        private int? _udpPort = 50010;
        private int? _tcpPort;

        private IPAddress _ipAddress;
        private YawDevice _selectedDevice;
       // private List<YawDevice> availableDevices = new List<YawDevice>();
       // private List<GameObject> deviceButtons = new List<GameObject>();

        private List<IPAddress> foundIps = new List<IPAddress>();
        private Coroutine searchCoroutine;

        private void Start()
        {
            //Setting ui elements' initial state and methods
            setupTitleLabel.text = "Set target YAW device";

            //Yaw device settings ui elements
            connectButton.onClick.AddListener(ConnectButtonPressed);
            connectButton.interactable = false;
            disconnectButton.onClick.AddListener(DisconnectButtonPressed);
            udpPortInputField.text = _udpPort.ToString();
            udpPortInputField.onValueChanged.AddListener(delegate { UDPPortInputFieldTextDidChange(udpPortInputField); });
            tcpPortInputField.text = _tcpPort.ToString();
            tcpPortInputField.onValueChanged.AddListener(delegate { TCPPortInputFieldTextDidChange(tcpPortInputField); });
            ipAddressInputField.onValueChanged.AddListener(delegate { IPAddressInputFieldTextDidChange(ipAddressInputField); });
            
            //Set self to delegate, to recieve DidFoundDevice(:) method calls from YAWController
            YawController.Instance().ControllerDelegate = this;

            //Initially set YAWController related ui elements according to YAWController's state
            RefreshLayout(YawController.Instance().State);
           
            //Start seacrhing for devices
            //StartCoroutine(SearchForDevices());
        }
        
        public void DeviceStoppedFromApp()
        {
            Debug.Log("DEVICE STOPPED FROM CONFIGAPP");
        }
        
        public void DeviceStartedFromApp()
        {
            Debug.Log("DEVICE STARTED FROM CONFIGAPP");
        }

        private void OnDestroy()
        {
           // StopCoroutine(SearchForDevices());

            //Remove all listeners
            connectButton.onClick.RemoveAllListeners();
            disconnectButton.onClick.RemoveAllListeners();
            udpPortInputField.onValueChanged.RemoveAllListeners();
            tcpPortInputField.onValueChanged.RemoveAllListeners();
            ipAddressInputField.onValueChanged.RemoveAllListeners();
        }

        private IEnumerator SearchForDevices()
        {
            Debug.Log("started searching for devices");
            while (true)
            {
                if (_udpPort != null && _udpPort > 1024)
                {
                    YawController.Instance().DiscoverDevices(_udpPort.Value);
                }
                yield return new WaitForSeconds(0.5f);
            }
        }

        private void UDPPortInputFieldTextDidChange(InputField inputField)
        {
            // availableDevices.Clear();
            // LayoutDeviceButtons(availableDevices);
            int portNumber;
            if (int.TryParse(inputField.text, out portNumber))
            {
                //TODO: - Error - not a port number
                _udpPort = portNumber;
            }
            else
            {
                _selectedDevice = null;
                _udpPort = null;
            }
            SetDeviceFromPortAndIp();
        }

        private void TCPPortInputFieldTextDidChange(InputField inputField)
        {
            int portNumber;
            if (int.TryParse(inputField.text, out portNumber))
            {
                _tcpPort = portNumber;
            }
            else
            {
                //TODO: - Error - not a port number
                _selectedDevice = null;
                _tcpPort = null;
            }
            SetDeviceFromPortAndIp();
        }

        private void IPAddressInputFieldTextDidChange(InputField inputField)
        {
            IPAddress ipFromString;
            if (IPAddress.TryParse(inputField.text, out ipFromString))
            {
                _ipAddress = ipFromString;
            }
            else
            {
                //TODO: - Error - not an ip address
                _selectedDevice = null;
                _ipAddress = null;
            }
            SetDeviceFromPortAndIp();
        }

        private void SetDeviceFromPortAndIp()
        {
            if (_ipAddress != null && _udpPort != null && _tcpPort != null)
            {
                //string hostName = Dns.GetHostEntry(this.ipAddress).HostName;
                _selectedDevice = new YawDevice(_ipAddress,DeviceType.YAW1, _tcpPort.Value, _udpPort.Value, "Manually set device", "Manually set device", DeviceStatus.Unknown); //TODO: - status
                connectButton.interactable = true;
            }
            else
            {
                connectButton.interactable = false;
            }
        }

        private void ConnectButtonPressed()
        {
            if (_selectedDevice == null) return;
            if (YawController.Instance().Device != null && SameDevice(YawController.Instance().Device, _selectedDevice)) return;

            YawController.Instance().ConnectToDevice(
                _selectedDevice,
                null,
                error =>
                {
                    ShowError(error);
                }
            );
        }

        private void DisconnectButtonPressed()
        {
            if (YawController.Instance().State != ControllerState.Initial)
            {
                YawController.Instance().DisconnectFromDevice(
                    null,
                    (error) =>
                    {
                        ShowError(error);
                    }
                );
            }
        }

        private void DeviceListItemPressed(YawDevice device)
        {
            if (device.Status != DeviceStatus.Available || YawController.Instance().State != ControllerState.Initial) return;
            ipAddressInputField.text = device.IPAddress.ToString();
            udpPortInputField.text = device.UDPPort.ToString();
            tcpPortInputField.text = device.TCPPort.ToString();
            _selectedDevice = device;
            connectButton.interactable = true;
        }   
        
        public void DidFoundDevice(YawDevice device)
        {
            if (foundIps.Contains(device.IPAddress)) return;
            
            Debug.Log("Found device: " + device.Name);
            foundIps.Add(device.IPAddress);

            GameObject go = Instantiate(buttonSample, buttonSample.transform.parent);
            go.SetActive(true);

            string buttonText = device.Name;
            if (device.Status != DeviceStatus.Available) buttonText += " - Reserved";

            go.GetComponentInChildren<Text>().text = buttonText;
            go.GetComponent<Button>().onClick.AddListener(delegate { DeviceListItemPressed(device); });
        }

        private bool SameDevice(YawDevice device, YawDevice toDevice)
        {
            return device.Id == toDevice.Id && device.TCPPort == toDevice.TCPPort && device.UDPPort == toDevice.UDPPort;
        }

        /* public void YawLimitDidChange(int currentLimit)
        {
            yawLimitInputField.text = currentLimit.ToString();
        }

        public void TiltLimitsDidChange(int pitchFrontLimit, int pitchBackLimit, int rollLimit)
        {
            pitchForwardLimitInputField.text = pitchFrontLimit.ToString();
            pitchBackwardLimitInputField.text = pitchBackLimit.ToString();
            rollLimitInputField.text = rollLimit.ToString();
        }
        */
       
        public void DidDisconnectFrom(YawDevice device)
        {
            ShowError("Device disconnected");
        }

        public void ControllerStateChanged(ControllerState state)
        {
            RefreshLayout(state);
        }

        private void RefreshLayout(ControllerState state)
        {
            switch (state)
            {
                case ControllerState.Initial:
                    connectButton.interactable = false;
                    connectButton.GetComponentInChildren<Text>().text = "Connect";
                    setupTitleLabel.text = "Set target YAW device";
                    disconnectButton.gameObject.SetActive(false);
                    _ipAddress = null;
                    ipAddressInputField.text = "";
                    _tcpPort = null;
                    tcpPortInputField.text = "";
                    break;
                case ControllerState.Connecting:
                    connectButton.interactable = false;
                    connectButton.GetComponentInChildren<Text>().text = "Connecting...";
                    break;
                case ControllerState.Connected:
                    connectButton.interactable = false;
                    disconnectButton.GetComponentInChildren<Text>().text = "Disconnect";
                    setupTitleLabel.text = "Active device: " + YawController.Instance().Device.Name;
                    disconnectButton.gameObject.SetActive(true);
                    disconnectButton.interactable = true;
                    connectButton.GetComponentInChildren<Text>().text = "Connect";
                    break;
                case ControllerState.Starting:
                    break;
                case ControllerState.Started:
                    break;
                case ControllerState.Stopping:
                    break;
                case ControllerState.Disconnecting:
                    connectButton.interactable = false;
                    disconnectButton.interactable = false;
                    disconnectButton.GetComponentInChildren<Text>().text = "Disconnecting...";
                    break;
            }
        }

        private void ShowError(string error, int duration = 10)
        {
            if (errorText.text != "")
            {
                StopCoroutine(ClearError(duration));
            }
            errorText.text = error;
            StartCoroutine(ClearError(duration));
        }

        private IEnumerator ClearError(int duration)
        {
            yield return new WaitForSeconds(duration);
            errorText.text = "";
        }

        public void ParkDevice()
        {
            if (YawController.Instance().State == ControllerState.Started)
            {
                YawController.Instance().StopDevice(true);
            }
        }
        
        public void StartDevice()
        {
            if (YawController.Instance().State == ControllerState.Connected)
            {
                YawController.Instance().StartDevice();
            }
        }
        
        public void CalibrateDevice()
        {
            if (YawController.Instance().State != ControllerState.Initial)
            {
                YawController.Instance().CalibrateDevice(true);
            }
        }

        public void HideShowPanel()
        {
            ClearList();
            if (!settingsPanel.activeInHierarchy)
            {
                settingsPanel.SetActive(true);
                searchCoroutine = StartCoroutine(SearchForDevices());
                if (YawController.Instance().State == ControllerState.Started)
                {
                   
                }
            }
            else
            {
                settingsPanel.SetActive(false);
                if (YawController.Instance().State == ControllerState.Connected)
                {
                   
                }
                StopCoroutine(searchCoroutine);
            }
        }
        
        private void ClearList()
        {
            foreach(Transform t in buttonSample.transform.parent)
            {
                if (t.gameObject.activeSelf)
                    Destroy(t.gameObject);
            }
            foundIps.Clear();
        }
    }
}