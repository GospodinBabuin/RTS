using System;
using UnityEditor.Rendering;
using UnityEngine;

public class RemovingState : IBuildingState
{
    private int _gameObjectIndex = -1;
    private Grid _grid;
    private PreviewSystem _previewSystem;
    private GridData _floorData;
    private GridData _furnitureData;
    private ObjectPlacer _objectPlacer;

    public RemovingState(Grid grid, PreviewSystem previewSystem,
        GridData floorData, GridData furnitureData, ObjectPlacer objectPlacer)
    {
        _grid = grid;
        _previewSystem = previewSystem;
        _floorData = floorData;
        _furnitureData = furnitureData;
        _objectPlacer = objectPlacer;

        _previewSystem.StartShowingRemovePreview();
    }

    public void EndState()
    {
        _previewSystem.StopShowingPreview();
    }

    public void OnAction(Vector3Int gridPosition)
    {
        GridData selectedData = null;
        if (!_furnitureData.CanPlaceObjectAt(gridPosition, Vector2Int.one))
        {
            selectedData = _furnitureData;
        }
        else
        if (!_floorData.CanPlaceObjectAt(gridPosition, Vector2Int.one))
        {
            selectedData = _floorData;
        }

        if (selectedData == null)
        {
            //sound
        }
        else
        {
            _gameObjectIndex = selectedData.GetRepresentationIndex(gridPosition);
            if (_gameObjectIndex == -1) return;

            selectedData.RemoveObjectAt(gridPosition);
            _objectPlacer.RemoveObjectAt(_gameObjectIndex);
        }

        Vector3 cellPosition = _grid.CellToWorld(gridPosition);
        _previewSystem.UpdatePosition(cellPosition, CheckIfSelectionIsValid(gridPosition));
    }

    private bool CheckIfSelectionIsValid(Vector3Int gridPosition)
    {
        return !(_furnitureData.CanPlaceObjectAt(gridPosition, Vector2Int.one) &&
            _floorData.CanPlaceObjectAt(gridPosition, Vector2Int.one));
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        bool validity = CheckIfSelectionIsValid(gridPosition);
        _previewSystem.UpdatePosition(_grid.CellToWorld(gridPosition), validity);
    }
}
