using UnityEngine;
using TMPro;
using System.Net;
using System.Reflection;
using System.Collections;
using YawVR;

public class DebugIPSetter : MonoBehaviour
{
    public TMP_InputField inputField;
    public YawController yawController;

    FieldInfo ipField;
    FieldInfo stateField;
    Coroutine connectRoutine;

    void Awake()
    {
        if (!inputField)
            inputField = GetComponent<TMP_InputField>();

        // Access private fields in YawController
        ipField = typeof(YawController).GetField("debug_ipAddress", BindingFlags.NonPublic | BindingFlags.Instance);
        stateField = typeof(YawController).GetField("State", BindingFlags.Public | BindingFlags.Instance);

        if (ipField == null)
            Debug.LogError("Could not find 'debug_ipAddress' in YawController");

        if (stateField == null)
            Debug.LogError("Could not find 'State' field in YawController");

        inputField.onEndEdit.AddListener(OnIPConfirmed);
    }

    void OnDestroy()
    {
        inputField.onEndEdit.RemoveListener(OnIPConfirmed);
    }

   public  void OnIPConfirmed(string newValue)
    {
        newValue = newValue.Trim();

        if (!IPAddress.TryParse(newValue, out _))
        {
            Debug.LogWarning("Invalid IP format: " + newValue);
            return;
        }

        ipField.SetValue(yawController, newValue);
        Debug.Log("IP updated to: " + newValue);

        if (connectRoutine != null) StopCoroutine(connectRoutine);
        connectRoutine = StartCoroutine(WaitAndConnect());
    }

    IEnumerator WaitAndConnect()
    {
        Debug.Log("Waiting for YawController to be ready...");

        // Wait until not "Initial"
        while (stateField.GetValue(yawController).ToString() == "Initial")
            yield return null;

        Debug.Log("YawController ready — connecting with new IP...");

        yawController.StartDevice(
            () => Debug.Log("🎉 Connected successfully"),
            (err) => Debug.LogError("Connection failed: " + err)
        );
    }
}
