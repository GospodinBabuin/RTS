using UnityEngine;

namespace GridPlacement
{
    public class RemovingState : IBuildingState
    {
        private int _gameObjectIndex = -1;
        private Grid _grid;
        private PreviewSystem _previewSystem;
        private GridData _buildingData;
        private ObjectPlacer _objectPlacer;

        public RemovingState(Grid grid, PreviewSystem previewSystem,
            GridData buildingData, ObjectPlacer objectPlacer)
        {
            _grid = grid;
            _previewSystem = previewSystem;
            _buildingData = buildingData;
            _objectPlacer = objectPlacer;
            
            _previewSystem.StartShowingRemovePreview();
        }

        public void EndState()
        {
            _previewSystem.StopShowingPreview();
        }

        public void OnAction(Vector3Int gridPosition)
        {
            if (_buildingData == null)
            {
                //sound
            }
            else
            {
                _gameObjectIndex = _buildingData.GetRepresentationIndex(gridPosition);

                if (_gameObjectIndex == -1)
                    return;

                _buildingData.RemoveObjectAt(gridPosition);
                _objectPlacer.RemoveObjectAy(_gameObjectIndex);
            }

            Vector3 cellPosition = _grid.CellToWorld(gridPosition);
            _previewSystem.UpdatePosition(cellPosition,
                !_buildingData.CanPlaceObjectAt(gridPosition, Vector2Int.one));
        }

        public void UpdateState(Vector3Int gridPosition)
        {
            _previewSystem.UpdatePosition(_grid.CellToWorld(gridPosition),
                !_buildingData.CanPlaceObjectAt(gridPosition, Vector2Int.one));
        }
    }
}
