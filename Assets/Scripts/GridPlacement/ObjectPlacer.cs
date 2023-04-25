using System.Collections.Generic;
using UnityEngine;

namespace GridPlacement
{
    public class ObjectPlacer : MonoBehaviour
    {
        [SerializeField] private List<GameObject> placedBuildings = new List<GameObject>();

        public int PlaceObject(GameObject prefab, Vector3 position)
        {
            GameObject newBuilding = Instantiate(prefab);
            newBuilding.transform.position = position;
            placedBuildings.Add(newBuilding);
            return placedBuildings.Count - 1;
        }

        public void RemoveObjectAy(int gameObjectIndex)
        {
            if (placedBuildings.Count <= gameObjectIndex
                || placedBuildings[gameObjectIndex] == null)
                return;
            
            Destroy(placedBuildings[gameObjectIndex]);
            placedBuildings[gameObjectIndex] = null;
        }
    }
}
