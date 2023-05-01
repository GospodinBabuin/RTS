using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Buildings
{
    public class BuildingMenu : MonoBehaviour
    {
        [SerializeField] private GameObject factoryMenu;
        [SerializeField] private GameObject heavyFactoryMenu;
        private Player _player;
        
        public GameObject FactoryMenu => factoryMenu;
        public GameObject HeavyFactoryMenu => heavyFactoryMenu;

        private Building _currentBuilding;
        private GameObject _currentMenu;

        private void Start()
        {
            _player = GameObject.FindWithTag("Player").GetComponent<Player>();
        }

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
            int cost = unit.GetComponent<Unit>().Cost;
            if (!_player.Bank.IsEnoughMass(cost))
                return;
            
            if (_currentBuilding == null) return;
        
            _player.Bank.SpendMass(this, cost);
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
}