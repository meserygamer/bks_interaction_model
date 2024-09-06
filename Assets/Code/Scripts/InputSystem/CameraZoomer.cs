using UnityEngine;

namespace Code.Scripts.InputSystem
{
    public class CameraZoomer : MonoBehaviour
    {
        [SerializeField] private Camera _cameraForZoom;
        [SerializeField] private string _inputAxisName = "Mouse ScrollWheel";
        [SerializeField] private int _cameraZoomSpeed = 20;
        
        private void LateUpdate()
        {
            _cameraForZoom.fieldOfView -= Input.GetAxis(_inputAxisName) * _cameraZoomSpeed;
        }
    }
}