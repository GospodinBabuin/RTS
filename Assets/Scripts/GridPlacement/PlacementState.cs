using UnityEngine;

public class PlacementState : IBuildingState
{
    private int _selectedObjectIndex = -1;
    private int _ID;
    private Grid _grid;
    private PreviewSystem _previewSystem;
    private ObjectsDatabaseSO _database;
    private GridData _floorData;
    private GridData _furnitureData;
    private ObjectPlacer _objectPlacer;

    public PlacementState(int iD, Grid grid,
        PreviewSystem previewSystem, ObjectsDatabaseSO objectsDatabaseSO,
        GridData floorData, GridData furnitureData, ObjectPlacer objectPlacer)
    {
        _ID = iD;
        _grid = grid;
        _previewSystem = previewSystem;
        _database = objectsDatabaseSO;
        _floorData = floorData;
        _furnitureData = furnitureData;
        _objectPlacer = objectPlacer;

        _selectedObjectIndex = _database.ObjectsData.FindIndex(data => data.ID == _ID);

        if (_selectedObjectIndex > -1)
        {
            _previewSystem.StartShowingPlacementPreview(
                _database.ObjectsData[_selectedObjectIndex].Prefab,
                _database.ObjectsData[_selectedObjectIndex].Size);
        }
        else
            throw new System.Exception($"No object with ID {iD}");
    }

    public void EndState()
    {
        _previewSystem.StopShowingPreview();
    }

    public void OnAction(Vector3Int gridPosition)
    {
        if (!CheckPlacementValidity(gridPosition, _selectedObjectIndex)) return;

        int index = _objectPlacer.PlaceObject(_database.ObjectsData[_selectedObjectIndex].Prefab, 
            _grid.CellToWorld(gridPosition));

        GridData selectedData = _database.ObjectsData[_selectedObjectIndex].ID == 0 ? _floorData : _furnitureData;
        selectedData.AddObjectAt(gridPosition, _database.ObjectsData[_selectedObjectIndex].Size,
            _database.ObjectsData[_selectedObjectIndex].ID, index);
        _previewSystem.UpdatePosition(_grid.CellToWorld(gridPosition), false);
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        _previewSystem.UpdatePosition(_grid.CellToWorld(gridPosition), 
            CheckPlacementValidity(gridPosition, _selectedObjectIndex));
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        GridData selectedData = _database.ObjectsData[selectedObjectIndex].ID == 0 ? _floorData : _furnitureData;
        return selectedData.CanPlaceObjectAt(gridPosition, _database.ObjectsData[selectedObjectIndex].Size);
    }
}