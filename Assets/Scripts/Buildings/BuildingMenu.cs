using System;
using UnityEngine;

public class BuildingMenu : MonoBehaviour
{
    [SerializeField] private GameObject factoryMenu;
    [SerializeField] private GameObject heavyFactoryMenu;

    public GameObject FactoryMenu => factoryMenu;
    public GameObject HeavyFactoryMenu => heavyFactoryMenu;

    private Building _currentBuilding;
    private GameObject _currentMenu;
    
    public void OpenMenu(GameObject menu, Building building)
    {
        if (_currentMenu != null)
            _currentMenu.SetActive(false);

        _currentBuilding = building;
        _currentMenu = menu;
        
        _currentMenu.SetActive(true);
    }

    public void CloseMenu()
    {
        _currentMenu.SetActive(false);
        
        _currentBuilding = null;
        _currentMenu = null;
    }
    
    public void SpawnUnit(GameObject unit)
    {
        if (_currentBuilding == null) return;
        
        Instantiate(unit, _currentBuilding.UnitSpawnPosition.position, Quaternion.identity);
    }
}

[Serializable]
public class MenuData : ScriptableObject
{
    public string Name { get; private set; }
    public int ID { get; private set; }
    public GameObject Prefab { get; private set; }
}