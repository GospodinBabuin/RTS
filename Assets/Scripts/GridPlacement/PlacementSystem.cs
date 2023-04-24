using System;
using System.Collections.Generic;
using UnityEngine;

namespace GridPlacement
{
    public class PlacementSystem : MonoBehaviour
    {
        [SerializeField] private GameObject mouseIndicator;

        private InputManager _inputManager;
        private Grid _grid;

        [SerializeField] private ObjectsDatabaseSO database;
        private int _selectedObjectIndex = -1;

        [SerializeField] private GameObject gridVisualization;

        private GridData _buildingData;
        
        private PreviewSystem _preview;

        private Vector3Int _lastDetectedPosition = Vector3Int.zero;

        private bool _isRemoving;

        private ObjectPlacer _objectPlacer;

        private void Awake()
        {
            GameObject parent = transform.parent.gameObject;
            
            _inputManager = parent.GetComponentInChildren<InputManager>();
            _grid = parent.GetComponentInChildren<Grid>();
            _preview = parent.GetComponentInChildren<PreviewSystem>();
            _objectPlacer = parent.GetComponentInChildren<ObjectPlacer>();

            _buildingData = new GridData();
            
            StopPlacement();
        }

        private void Update()
        {
            if (_selectedObjectIndex < 0) return;

            Vector3 mousePosition = _inputManager.GetSelectedMapPosition();
            Vector3Int gridPosition = _grid.WorldToCell(mousePosition);
            
            bool placementValidity = CheckPlacementValidity(gridPosition, _selectedObjectIndex);

            if (_lastDetectedPosition == gridPosition) return;

            mouseIndicator.transform.position = mousePosition;
            _preview.UpdatePosition(_grid.CellToWorld(gridPosition), placementValidity);
            _lastDetectedPosition = gridPosition;
        }

        public void StartPalcement(int id)
        {
            StopPlacement();
            _selectedObjectIndex = database.objectsData.FindIndex(data => data.ID == id);
            if (_selectedObjectIndex < 0)
            {
                Debug.LogError($"No ID found {id}");
                return;
            }

            gridVisualization.SetActive(true);
            _preview.StartShowingPlacementPreview(database.objectsData[_selectedObjectIndex].Prefab,
                database.objectsData[_selectedObjectIndex].Size);
            _inputManager.OnClicked += PlaceStructure;
            _inputManager.OnExit += StopPlacement;
        }
        
        private void StopPlacement()
        {
            _selectedObjectIndex = -1;
            
            gridVisualization.SetActive(false);
            _preview.StopShowingPlacementPreview();
            _inputManager.OnClicked -= PlaceStructure;
            _inputManager.OnExit -= StopPlacement;
            _lastDetectedPosition = Vector3Int.zero;
        }

        private void PlaceStructure()
        {
            if (_inputManager.IsPointerOverUI()) return;
            
            Vector3 mousePosition = _inputManager.GetSelectedMapPosition();
            Vector3Int gridPosition = _grid.WorldToCell(mousePosition);

            bool placementValidity = CheckPlacementValidity(gridPosition, _selectedObjectIndex);
            if (!placementValidity) return;

            int index = _objectPlacer.PlaceObject(database.objectsData[_selectedObjectIndex].Prefab,
                _grid.CellToWorld(gridPosition));

            _buildingData.AddObjectAt(gridPosition, 
                database.objectsData[_selectedObjectIndex].Size,
                database.objectsData[_selectedObjectIndex].ID, 
                index);
            
            _preview.UpdatePosition(_grid.CellToWorld(gridPosition), false);
        }

        private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
        {
            return _buildingData.CanPlaceObjectAt(gridPosition, database.objectsData[selectedObjectIndex].Size);
        }
    }
}
