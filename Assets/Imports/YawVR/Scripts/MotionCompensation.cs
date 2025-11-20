using UnityEngine;

namespace YawVR
{
    /// <summary>
    /// Cancels the camera's rotation based on IMU data
    /// </summary>
    public class MotionCompensation : MonoBehaviour
    {
        [SerializeField] private Transform cameraOffsetTransform;
        [SerializeField] private YawController yawController;
        [SerializeField] [Range(0f,0.9f)] private float smoothing = 0.7f;
        
        private Vector3 _simData;
        private Vector3 _offset;

        public void UpdateOffset()
        {
            _offset.y = _simData.y;
        }

        private void Update()
        {
            if (YawController.Instance().State != ControllerState.Started &&
                YawController.Instance().State != ControllerState.Connected) return;
            
            _simData.y = -yawController.Device.ActualPosition.yaw;

            if (cameraOffsetTransform != null)
                cameraOffsetTransform.rotation = Quaternion.Slerp(cameraOffsetTransform.rotation,Quaternion.Euler(_simData - _offset),1f-smoothing);
        }
    }
}