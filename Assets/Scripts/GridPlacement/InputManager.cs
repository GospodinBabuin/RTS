using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace GridPlacement
{
    public class InputManager : MonoBehaviour
    {
        private Camera _camera;
        private Vector3 _lastPosition;
        [SerializeField] private LayerMask placementLayerMask;

        private PlacementSystem _placementSystem;

        public event Action OnClicked; 
        public event Action OnExit; 

        private void Awake()
        {
            _camera = Camera.main;

            _placementSystem = transform.parent.GetComponentInChildren<PlacementSystem>();
        }

        public Vector3 GetSelectedMapPosition()
        {
            Vector3 mousePosition = Mouse.current.position.ReadValue();
            mousePosition.z = _camera.nearClipPlane;
            Ray ray = _camera.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, placementLayerMask))
            {
                _lastPosition = hit.point;
            }

            return _lastPosition;
        }

        public bool IsPointerOverUI() => EventSystem.current.IsPointerOverGameObject();
        
        private void OnConfirmAction()
        {
            OnClicked?.Invoke();
        }
        
        private void OnCancelAction()
        {
            OnExit?.Invoke();
        }
        
        private void OnRemoveBuilding()
        {
            _placementSystem.StartRemoving();
        }
    }
}
