using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Camera _camera;

    [SerializeField] private LayerMask selectableLayerMask;
    
    private Unit _selectedUnit;
    private Building _selectedBuilding;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Select()
    {
        Vector3 mousePosition = Mouse.current.position.ReadValue();
        mousePosition.z = _camera.nearClipPlane;

        Ray ray = _camera.ScreenPointToRay(mousePosition);
        
        if (Physics.Raycast(ray, out RaycastHit hitData, float.MaxValue, selectableLayerMask))
        {
            if (hitData.transform.gameObject.GetComponent<Unit>())
            {
                _selectedUnit = hitData.transform.gameObject.GetComponent<Unit>();
                _selectedBuilding = null;
                return;
            }
            
            if (hitData.transform.gameObject.GetComponent<Building>())
            {
                _selectedBuilding = hitData.transform.gameObject.GetComponent<Building>();
                _selectedUnit = null;
                
                _selectedBuilding.ActivateMenu();
                return;
            }

            if (_selectedUnit != null)
            {
                _selectedUnit.SelectNewPosition(hitData.point);
            }
        }
        
        
    }

    private void DeselectObject()
    {
        _selectedUnit = null;
    }

    private void OnSelect()
    {
        Select();
    }

    private void OnCancel()
    {
        DeselectObject();
    }
}
