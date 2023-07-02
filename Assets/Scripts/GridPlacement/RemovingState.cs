using Buildings;
using UnityEngine;

namespace GridPlacement
{
    public class RemovingState : IBuildingState
    {
        private Grid _grid;
        private PreviewSystem _previewSystem;
        private GridData _buildingData;
        private GridData _massData;
        private ObjectPlacer _objectPlacer;
        private PlacementSystem _placementSystem;

        public RemovingState(Grid grid, PreviewSystem previewSystem,
            GridData buildingData, GridData massData, ObjectPlacer objectPlacer, PlacementSystem placementSystem)
        {
            _grid = grid;
            _previewSystem = previewSystem;
            _buildingData = buildingData;
            _objectPlacer = objectPlacer;
            _massData = massData;
            _placementSystem = placementSystem;
            
            _previewSystem.StartShowingRemovePreview();
        }

        public void EndState()
        {
            _previewSystem.StopShowingPreview();
        }

        public void OnAction(Vector3Int gridPosition)
        {
            GridData selectedData = null;
            if (!_buildingData.CanPlaceObjectAt(gridPosition, Vector2Int.one))
            {
                selectedData = _buildingData;
            }
            else if (!_massData.CanPlaceObjectAt(gridPosition, Vector2Int.one))
            {
                selectedData = _massData;
            }
            if (selectedData == null)
            {
                //sound
            }
            else
            {
                int gameObjectIndex = selectedData.GetRepresentationIndex(gridPosition);

                if (gameObjectIndex <= -1)
                    return;

                selectedData.RemoveObjectAt(gridPosition);
                _objectPlacer.RemoveObjectAy(gameObjectIndex);
            }

            Vector3 cellPosition = _grid.CellToWorld(gridPosition);
            _previewSystem.UpdatePosition(cellPosition, CheckIfSelectionIsValid(gridPosition));
        }

        public void UpdateState(Vector3Int gridPosition)
        {
            _previewSystem.UpdatePosition(_grid.CellToWorld(gridPosition),
                CheckIfSelectionIsValid(gridPosition));
        }

        private bool CheckIfSelectionIsValid(Vector3Int gridPosition)
        {
            return !(_buildingData.CanPlaceObjectAt(gridPosition, Vector2Int.one) &&
                     _massData.CanPlaceObjectAt(gridPosition, Vector2Int.one));
        }
    }
}
