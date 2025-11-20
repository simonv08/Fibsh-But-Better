using System;
using System.Net;

namespace YawVR
{
    [Serializable]
    public enum DeviceType
    {
        YAW1,
        YAW2,
        YAW3
    }

    [Serializable]
    public enum DeviceState
    {
        STOPPED,
        STARTED,
        NOTRACKER,
        PARKING
    }

    /// <summary>
    /// Describes a YawDevice
    /// </summary>
    [Serializable]
    public class YawDevice
    {
        private IPAddress ipAddress;
        private int tcpPort;
        private int udpPort;
        private string id;
        private string name;
        private DeviceStatus status;

        public IPAddress IPAddress => ipAddress;
        public int TCPPort => tcpPort;
        public int UDPPort => udpPort;
        public string Id => id;
        public string Name => name;
        public DeviceStatus Status => status;

        public float batteryVoltage;
        public float batteryPercent;
        public OVector ActualPosition;
        public Parameters deviceParams;
        public DeviceType type;
        public byte[] temps = new byte[4];
        public DeviceState State;

        public YawDevice(IPAddress ipAddress, DeviceType type, int tcpPort, int udpPort, string id, string name,
            DeviceStatus status)
        {
            this.ipAddress = ipAddress;
            this.type = type;
            this.tcpPort = tcpPort;
            this.udpPort = udpPort;
            this.id = id;
            this.name = name;
            this.status = status;
        }

        public void SetStatus(DeviceStatus status)
        {
            this.status = status;
        }
    }
}