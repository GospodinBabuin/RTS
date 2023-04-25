using UnityEngine;
using UnityEngine.InputSystem;

namespace CameraController
{
    public class CameraMotion : MonoBehaviour
    {
        [SerializeField] private float _cameraSpeed = 1f;
        [SerializeField] private float _smoothing = 10f;
        [SerializeField] private Vector2 _range = new Vector2(100, 100);

        private Vector3 _targetPosition;
        private Vector3 _input;
        private Vector2 _cameraMoveAxis;

        [SerializeField] private bool _cameraOnPosition = false;

        private void Awake()
        {
            _targetPosition = transform.position;
        }

        private void Update()
        {
            HandleInput();
            MoveCamera();
        }

        private void HandleInput()
        {
            if (_cameraMoveAxis == Vector2.zero)
            {
                _input = Vector3.zero;
                return;
            }

            Vector3 right = transform.right * _cameraMoveAxis.x;
            Vector3 forward = transform.forward * _cameraMoveAxis.y;

            _input = (forward + right).normalized;
        }

        private void MoveCamera()
        {
            if (_input != Vector3.zero)
            {
                Vector3 nextTargetPosition = _targetPosition + _input * _cameraSpeed;
                if (IsInBounds(nextTargetPosition)) _targetPosition = nextTargetPosition;
            }

            _cameraOnPosition = transform.position == _targetPosition;

            if (!_cameraOnPosition)
                transform.position = Vector3.Lerp(transform.position, _targetPosition, Time.deltaTime * _smoothing);
        }

        private bool IsInBounds(Vector3 position)
        {
            return position.x > -_range.x &&
                   position.x < _range.x &&
                   position.z > -_range.y &&
                   position.z < _range.y;
        }

        private void OnMoveCamera(InputValue value)
        {
            MoveCameraInput(value.Get<Vector2>());
        }

        private void MoveCameraInput(Vector2 newMoveDirection)
        {
            _cameraMoveAxis = newMoveDirection;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 5f);
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(_range.x * 2f, 5f, _range.y * 2f));
        }
    }
}