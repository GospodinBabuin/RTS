using UnityEngine;
using UnityEngine.InputSystem;

namespace CameraController
{
    public class CameraZoom : MonoBehaviour
    {
        [SerializeField] private float _zoomSpeed = 5f;
        [SerializeField] private float _smoothing = 10f;
        [SerializeField] private float _maxZoomRange = 5f;
        [SerializeField] private float _minZoomRange = 40f;
        [SerializeField] private Transform _cameraHolder;

        [SerializeField] private bool _cameraOnPosition = false;

        private float _zoom;

        private Vector3 _cameraDirection => transform.InverseTransformDirection(_cameraHolder.forward);
        private Vector3 _targetPosition;

        private void Awake()
        {
            _targetPosition = _cameraHolder.localPosition;
        }

        private void Update()
        {
            Zoom();
        }

        private void Zoom()
        {
            if (_zoom != 0)
            {
                Vector3 nextTargetPosition = _targetPosition + _cameraDirection * (_zoom * _zoomSpeed);
                if (IsInBounds(nextTargetPosition)) _targetPosition = nextTargetPosition;
            }

            _cameraOnPosition = _cameraHolder.localPosition == _targetPosition;

            if (!_cameraOnPosition)
                _cameraHolder.localPosition = Vector3.Lerp(_cameraHolder.localPosition, _targetPosition, Time.deltaTime * _smoothing);
        }

        private bool IsInBounds(Vector3 position)
        {
            return position.y < _minZoomRange && position.y > _maxZoomRange;
        }

        private void OnZoom(InputValue value)
        {
            ZoomInput(value.Get<float>());
        }

        private void ZoomInput(float newZoom)
        {
            _zoom = newZoom;
        }
    }
}
