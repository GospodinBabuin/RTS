using System;
using UnityEngine;

namespace GridPlacement
{
    public class PlacementSystem : MonoBehaviour
    {
        private InputManager _inputManager;
        private Grid _grid;

        [SerializeField] private ObjectsDatabaseSO database;

        [SerializeField] private GameObject gridVisualization;

        private GridData _buildingData;
        private GridData _massData;
        
        private PreviewSystem _preview;

        private Vector3Int _lastDetectedPosition = Vector3Int.zero;

        private bool _isRemoving;

        private ObjectPlacer _objectPlacer;

        private IBuildingState _buildingState;

        [SerializeField] private GameObject _massIndicator;

        private void Awake()
        {
            GameObject parent = transform.parent.gameObject;
            
            _inputManager = parent.GetComponentInChildren<InputManager>();
            _grid = parent.GetComponentInChildren<Grid>();
            _preview = parent.GetComponentInChildren<PreviewSystem>();
            _objectPlacer = parent.GetComponentInChildren<ObjectPlacer>();

            _buildingData = new GridData();
            _massData = new GridData();
            
            StopPlacement();
        }

        private void Start()
        {
            gridVisualization.SetActive(false);
            
            _buildingState.EndState();

            _lastDetectedPosition = Vector3Int.zero;
            _buildingState = null;
        }

        private void Update()
        {
            if (_buildingState == null) return;

            Vector3 mousePosition = _inputManager.GetSelectedMapPosition();
            Vector3Int gridPosition = _grid.WorldToCell(mousePosition);
            
            if (_lastDetectedPosition == gridPosition) return;
            
            _buildingState.UpdateState(gridPosition);
            _lastDetectedPosition = gridPosition;
        }

        public void StartPlacement(int id)
        {
            StopPlacement();
            gridVisualization.SetActive(true);
            
            _buildingState = new PlacementState(id, _grid, _preview,
                database, _buildingData, _massData, _objectPlacer);
            
            _inputManager.OnClicked += PlaceStructure;
            _inputManager.OnExit += StopPlacement;
        }

        public void StartRemoving()
        {
            StopPlacement();
            gridVisualization.SetActive(true);
            
            _buildingState = new RemovingState(_grid, _preview,
                _buildingData, _massData, _objectPlacer, this);
            
            _inputManager.OnClicked += PlaceStructure;
            _inputManager.OnExit += StopPlacement;
        }
        
        private void StopPlacement()
        {
            if (_buildingState == null) return;
            
            gridVisualization.SetActive(false);
            _buildingState.EndState();
            _inputManager.OnClicked -= PlaceStructure;
            _inputManager.OnExit -= StopPlacement;
            _lastDetectedPosition = Vector3Int.zero;
            _buildingState = null;
        }

        private void PlaceStructure()
        {
            if (_inputManager.IsPointerOverUI()) return;
            
            Vector3 mousePosition = _inputManager.GetSelectedMapPosition();
            Vector3Int gridPosition = _grid.WorldToCell(mousePosition);

            _buildingState.OnAction(gridPosition);
        }
    }
}
