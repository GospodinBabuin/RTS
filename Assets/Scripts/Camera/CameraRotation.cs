using UnityEngine;
using UnityEngine.InputSystem;

public class CameraRotation : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 15f;
    [SerializeField] private float _smoothing = 20f;

    private float _targetAngle;
    private float _currentAngle;

    [SerializeField] private bool _cameraOnPosition = false;

    private bool _select;
    private Vector2 _cursorDirection;

    private void Awake()
    {
        _targetAngle = transform.eulerAngles.y;
        _currentAngle = _targetAngle;
    }

    private void Update()
    {
        HandleInput();
        Rotate();
    }

    private void HandleInput()
    {
        if (!_select) return;
        _targetAngle += _cursorDirection.x * _rotationSpeed;
    }

    private void Rotate()
    {
        _cameraOnPosition = _currentAngle == _targetAngle;

        if (!_cameraOnPosition)
        {
            _currentAngle = Mathf.Lerp(_currentAngle, _targetAngle, Time.deltaTime * _smoothing);
            transform.rotation = Quaternion.AngleAxis(_currentAngle, Vector3.up);
        }
    }

    private void OnSelect(InputValue value)
    {
        SelectInput(value.isPressed);
    }

    private void SelectInput(bool newSelectState)
    {
        _select = newSelectState;
    }

    private void OnCursorDirection(InputValue value)
    {
        CursorDirectionInput(value.Get<Vector2>());
    }

    private void CursorDirectionInput(Vector2 newCursorDirection)
    {
        _cursorDirection = newCursorDirection;
    }
}
