using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlacementSystemInputHolder : MonoBehaviour
{
    private Camera _camera;
    private Vector3 _position;
    [SerializeField] private LayerMask _placementLayerMask;

    public event Action OnClicked, OnEscape;
    public bool IsPointerOverUI() => EventSystem.current.IsPointerOverGameObject();

    private void Awake()
    {
        _camera = Camera.main;
    }

    public Vector3 GetSelectedMapPosition()
    {
        Vector3 mousePosition = Mouse.current.position.ReadValue();
        mousePosition.z = _camera.nearClipPlane;

        Ray ray = _camera.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, _placementLayerMask))
            _position = hit.point;

        return _position;
    }

    private void OnSelect(InputValue value)
    {
        OnClicked?.Invoke();
    }
    
    private void OnExit(InputValue value)
    {
        OnEscape?.Invoke();
    }
}
