using UnityEngine;

public class Manager : MonoBehaviour
{
    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 250; // or 999 for unlimited
    }

}
