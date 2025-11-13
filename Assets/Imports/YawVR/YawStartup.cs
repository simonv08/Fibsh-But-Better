using System.Collections;
using UnityEngine;
using YawVR;

public class YawStartButton : MonoBehaviour
{
    private bool disableCanRun = false;

    public void OnYawOn()
    {
        StartCoroutine(YawOn());
    }

    public void OnYawOff()
    {
        StartCoroutine(YawOff());
    }

    private IEnumerator YawOn()
    {
        // Wait 1 frame
        yield return null;

        if (YawController.Instance().State == ControllerState.Connected)
        {
            YawController.Instance().StartDevice(
                () => Debug.Log("Yaw started"),
                error => Debug.LogError("Failed to start device: " + error)
            );
        }
        else
        {
            Debug.LogWarning("Yaw not in a ready-to-start state: " + YawController.Instance().State);
        }

        disableCanRun = true;
    }

    private IEnumerator YawOff()
    {
        // Wait 1 frame
        yield return null;

        if (!disableCanRun) yield break;

        if (YawController.Instance().State == ControllerState.Started)
        {
            YawController.Instance().StopDevice(
                true,
                () => Debug.Log("Yaw stopped"),
                error => Debug.LogError("Failed to stop device: " + error)
            );
        }
        else
        {
            Debug.LogWarning("Yaw not in a running state: " + YawController.Instance().State);
        }
    }
}
