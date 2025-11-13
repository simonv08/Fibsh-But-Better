using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using YawVR;

/// <summary>
/// Shows the IMU's battery level
/// </summary>
public class BatteryLevelUI : MonoBehaviour
{
    private Slider _slider;

    private YawController _yawController;
    
    private void Awake()
    {
        _slider = GetComponent<Slider>();
    }
    
    private void Start()
    {
        _yawController = YawController.Instance();

        StartCoroutine(UpdateUI());
    }
    
    private IEnumerator UpdateUI()
    {
        WaitForSeconds wait = new WaitForSeconds(2f);
        while(true)
        {
            _slider.value = _yawController.Device.batteryVoltage;
            yield return wait;
        }
    }
}
