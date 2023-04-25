using System;
using UnityEngine;

namespace GridPlacement
{
    public class PlacementState : IBuildingState
    {
        private int _selectedObjectIndex = -1;
        private int _id;
        private Grid _grid;
        private PreviewSystem _previewSystem;
        private ObjectsDatabaseSO _database;
        private GridData _buildingData;
        private ObjectPlacer _objectPlacer;

        public PlacementState(int id, Grid grid, PreviewSystem previewSystem,
            ObjectsDatabaseSO database, GridData buildingData, ObjectPlacer objectPlacer)
        {
            _id = id;
            _grid = grid;
            _previewSystem = previewSystem;
            _database = database;
            _buildingData = buildingData;
            _objectPlacer = objectPlacer;
            
            _selectedObjectIndex = database.objectsData.FindIndex(data => data.ID == id);
            if (_selectedObjectIndex > -1)
            {
                _previewSystem.StartShowingPreview(database.objectsData[_selectedObjectIndex].Prefab,
                    database.objectsData[_selectedObjectIndex].Size);
            }
            else
            {
                throw new Exception($"No Object with ID {_id}");
            }
        }

        public void EndState()
        {
            _previewSystem.StopShowingPreview();
        }

        public void OnAction(Vector3Int gridPosition)
        {
            bool placementValidity = CheckPlacementValidity(gridPosition, _selectedObjectIndex);
            if (!placementValidity) return;

            int index = _objectPlacer.PlaceObject(_database.objectsData[_selectedObjectIndex].Prefab,
                _grid.CellToWorld(gridPosition));

            _buildingData.AddObjectAt(gridPosition, 
                _database.objectsData[_selectedObjectIndex].Size,
                _database.objectsData[_selectedObjectIndex].ID, 
                index);
            
            _previewSystem.UpdatePosition(_grid.CellToWorld(gridPosition), false);
        }
        
        private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
        {
            return _buildingData.CanPlaceObjectAt(gridPosition, _database.objectsData[selectedObjectIndex].Size);
        }

        public void UpdateState(Vector3Int gridPosition)
        {
            bool placementValidity = CheckPlacementValidity(gridPosition, _selectedObjectIndex);

            _previewSystem.UpdatePosition(_grid.CellToWorld(gridPosition), placementValidity);
        }
    }
}
