using UnityEngine;
using YawVR;

/// <summary>
/// Change color based on device state
/// </summary>
public class ChangeColor : MonoBehaviour
{
    [Header("Colors")]
    [SerializeField] private Color stoppedColor;
    [SerializeField] private Color startedColor;
    
    public void StateChanged(DeviceState state)
    {
        switch (state)
        {
            case DeviceState.STOPPED:
                gameObject.GetComponent<Renderer>().material.color = stoppedColor;
                break;
            case DeviceState.STARTED:
                gameObject.GetComponent<Renderer>().material.color = startedColor;
                break;
        }
    }
}