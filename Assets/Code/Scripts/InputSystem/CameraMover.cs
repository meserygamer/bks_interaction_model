using UnityEngine;

namespace Code.Scripts.InputSystem
{
    public class CameraMover : MonoBehaviour
    {
        [SerializeField] private Camera _cameraForMove;
        [SerializeField] private int _cameraMoveSpeed = 40;

        [SerializeField] private int _beginMoveMouseButton = 2;
        [SerializeField] private string _inputXAxisName = "Mouse X";
        [SerializeField] private string _inputYAxisName = "Mouse Y";

        private void LateUpdate()
        {
            if (Input.GetMouseButton(_beginMoveMouseButton))
            {
                Vector3 cameraMove = new Vector3(Input.GetAxis(_inputXAxisName) * _cameraMoveSpeed * Time.deltaTime,
                        Input.GetAxis(_inputYAxisName) * _cameraMoveSpeed * Time.deltaTime);
                _cameraForMove.gameObject.transform.Translate(cameraMove, Space.Self);
            }
        }
    }
}