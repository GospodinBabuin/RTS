using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    private List<GameObject> _placedBuildings = new List<GameObject>();

    public int PlaceObject(GameObject prefab, Vector3 position)
    {
        GameObject newBuilding = Instantiate(prefab);
        newBuilding.transform.position = position;
        _placedBuildings.Add(newBuilding);
        return _placedBuildings.Count - 1;
    }
}
