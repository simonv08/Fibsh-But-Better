namespace YawVR
{
    /// <summary>
    /// SDK Connection type
    /// </summary>
    public enum ConnectType
    {
        ConnectToFirstFoundDevice,
        ConnectViaIP,
        NoAutoConnect
    }

    /// <summary>
    /// YawDevice's status
    /// </summary>
    public enum DeviceStatus
    {
        Available,
        Reserved,
        Unknown
    }
    
    /// <summary>
    /// The controller's inner state
    /// </summary>
    public enum ControllerState
    {
        Initial,
        Connecting,
        Connected,
        Starting,
        Started,
        Stopping,
        Disconnecting
    } 
}