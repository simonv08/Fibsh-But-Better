using UnityEngine;

public class Fish : MonoBehaviour, IScannable
{
    [SerializeField] private FishInfo info;

    public FishInfo GetScanData()
    {
        return info;
    }
}
