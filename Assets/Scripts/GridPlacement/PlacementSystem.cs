using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private Grid _grid;
    [SerializeField] private PlacementSystemInputHolder _inputHolder;

    [SerializeField] private ObjectsDatabaseSO _database;
    [SerializeField] private GameObject _gridVisualisation;

    private GridData _floorData;
    private GridData _furnitureData;

    [SerializeField] private PreviewSystem _preview;

    [SerializeField] private ObjectPlacer _objectPlacer;

    private Vector3Int _lastDetectedPosition = Vector3Int.zero;

    IBuildingState _buildingState;

    private void Start()
    {
        StopPlacement();
        _floorData = new();
        _furnitureData = new();
    }

    private void Update()
    {
        if (_buildingState == null) return;

        Vector3 mousePosition = _inputHolder.GetSelectedMapPosition();
        Vector3Int gridPosition = _grid.WorldToCell(mousePosition);

        if (_lastDetectedPosition != gridPosition)
        {
            _buildingState.UpdateState(gridPosition);
            _lastDetectedPosition = gridPosition;
        }

    }

    public void StartPlacement(int ID)
    {
        StopPlacement();
        _gridVisualisation.SetActive(true);

        _buildingState = new PlacementState(ID, _grid, _preview,
            _database, _floorData, _furnitureData, _objectPlacer);

        _inputHolder.OnClicked += PlaceStructure;
        _inputHolder.OnEscape += StopPlacement;
    }

    public void StartRemoving()
    {
        StopPlacement();
        _gridVisualisation.SetActive(true);
        _buildingState = new RemovingState(_grid, _preview, _floorData, _furnitureData, _objectPlacer);
        _inputHolder.OnClicked += PlaceStructure;
        _inputHolder.OnEscape += StopPlacement;
    }

    private void PlaceStructure()
    {
        if (_inputHolder.IsPointerOverUI()) return;

        Vector3 mousePosition = _inputHolder.GetSelectedMapPosition();
        Vector3Int gridPosition = _grid.WorldToCell(mousePosition);

        _buildingState.OnAction(gridPosition);
    }

    //private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    //{
    //    GridData selectedData = _database.ObjectsData[selectedObjectIndex].ID == 0 ? _floorData : _furnitureData;
    //    return selectedData.CanPlaceObjectAt(gridPosition, _database.ObjectsData[selectedObjectIndex].Size);
    //}

    private void StopPlacement()
    {
        if (_buildingState == null) return;

        _gridVisualisation.SetActive(false);
        _buildingState.EndState();
        _inputHolder.OnClicked -= PlaceStructure;
        _inputHolder.OnEscape -= StopPlacement;
        _lastDetectedPosition = Vector3Int.zero;
        _buildingState = null;
    }
}
