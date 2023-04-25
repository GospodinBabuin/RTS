using System;
using System.Collections.Generic;
using UnityEngine;

namespace GridPlacement
{
    public class GridData
    {
        private Dictionary<Vector3Int, PlacementData> _placedObjects = new();

        public void AddObjectAt(Vector3Int gridPosition,
            Vector2Int objectSize, int id, int placedObjectIndex)
        {
            List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);

            PlacementData data = new PlacementData(positionToOccupy, id, placedObjectIndex);

            foreach (Vector3Int pos in positionToOccupy)
            {
                if (_placedObjects.ContainsKey(pos))
                    throw new Exception($"Dictionary already contains cell position {pos}");
                _placedObjects[pos] = data;
            }
        }

        private List<Vector3Int> CalculatePositions(Vector3Int gridPosition, Vector2Int objectSize)
        {
            List<Vector3Int> returnValues = new();
        
            for (int x = 0; x < objectSize.x; x++)
            {
                for (int y = 0; y < objectSize.y; y++)
                {
                    returnValues.Add(gridPosition + new Vector3Int(x, 0, y));
                }
            }

            return returnValues;
        }

        public bool CanPlaceObjectAt(Vector3Int gridPosition, Vector2Int objectSize)
        {
            List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);

            foreach (Vector3Int pos in positionToOccupy)
            {
                if (_placedObjects.ContainsKey(pos)) return false;
            }

            return true;
        }

        public int GetRepresentationIndex(Vector3Int gridPosition)
        {
            if (!_placedObjects.ContainsKey(gridPosition))
                return -1;

            return _placedObjects[gridPosition].PlacedObjectIndex;
        }

        public void RemoveObjectAt(Vector3Int gridPosition)
        {
            foreach (Vector3Int pos in _placedObjects[gridPosition].OccupiedPositions)
            {
                _placedObjects.Remove(pos);
            }
        }
    }

    public class PlacementData
    {
        public PlacementData(List<Vector3Int> occupiedPositions, int id, int placedObjectIndex)
        {
            OccupiedPositions = occupiedPositions;
            ID = id;
            PlacedObjectIndex = placedObjectIndex;
        }
    
        public List<Vector3Int> OccupiedPositions;
        public int ID { get; private set; }
        public int PlacedObjectIndex { get; private set; }
    }
}