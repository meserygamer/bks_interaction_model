using System;
using Code.Scripts.ControlObjectsSystem;
using JetBrains.Annotations;
using UnityEngine;

namespace Code.Scripts.InputSystem
{
    public class CameraRotatorAroundObject : MonoBehaviour
    {
        [SerializeField] private Camera _cameraForRotate;

        private ControllableObject _selectedObject;
        private Vector3 _previousPositionCursor;

        private void LateUpdate()
        {
            if (Input.GetMouseButton(0)) 
                UpdateSelectedObject();
            
            if(_selectedObject is null)
                return;

            if (Input.GetMouseButtonDown(1)) 
                _previousPositionCursor = _cameraForRotate.ScreenToViewportPoint(Input.mousePosition);
            
            if (Input.GetMouseButton(1))
            {
                RotateCameraAroundObject();
                _cameraForRotate.transform.LookAt(_selectedObject.transform.position);
                _previousPositionCursor = _cameraForRotate.ScreenToViewportPoint(Input.mousePosition);
            }
        }

        private void UpdateSelectedObject()
        {
            ControllableObject objectUnderMouseCursor = GetControllableObjectUnderMouseCursor();
            if (objectUnderMouseCursor is null)
                return;
                
            _cameraForRotate.transform.LookAt(objectUnderMouseCursor.transform);
            _selectedObject = objectUnderMouseCursor;
        }
        
        [CanBeNull]
        private ControllableObject GetControllableObjectUnderMouseCursor()
        {
            Ray rayToObject = _cameraForRotate.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(rayToObject, out RaycastHit hitInfo))
                return null;

            GameObject targetOfRaycast = hitInfo.collider.gameObject; 
            targetOfRaycast.TryGetComponent<ControllableObject>(out ControllableObject controllableObject);
            return controllableObject;
        }

        private void RotateCameraAroundObject()
        {
            Vector3 cursorMovingDirection = _previousPositionCursor 
                                            - _cameraForRotate.ScreenToViewportPoint(Input.mousePosition);
                
            Vector3 vectorBetweenTwoCameraAndObject = (_cameraForRotate.transform.position - _selectedObject.transform.position).normalized;
            
            if (Math.Abs(vectorBetweenTwoCameraAndObject.x) < Math.Abs(vectorBetweenTwoCameraAndObject.z))
            {
                _cameraForRotate.transform.RotateAround(_selectedObject.transform.position, 
                    vectorBetweenTwoCameraAndObject.z > 0f ? Vector3.left : Vector3.right,
                    cursorMovingDirection.y * 180);
            }
            else
            {
                _cameraForRotate.transform.RotateAround(_selectedObject.transform.position,
                    vectorBetweenTwoCameraAndObject.x > 0f ? Vector3.forward : Vector3.back,
                    cursorMovingDirection.y * 180);
            }
                
            _cameraForRotate.transform.RotateAround(_selectedObject.transform.position,
                Vector3.up,
                -cursorMovingDirection.x * 180);
        }
    }
}