using UnityEngine;

namespace Buildings
{
    public abstract class Building : MonoBehaviour
    {
        private Player _owner;
        
        protected BuildingMenu MenuManager;
        [SerializeField] private bool canOpenMenu;

        [SerializeField] private int maxHealth;
        protected int currentHealth;

        [SerializeField] private Transform unintSpawnPosition;
        public Transform UnitSpawnPosition => unintSpawnPosition;
        public bool CanOpenMenu => canOpenMenu;
        public Player Owner => _owner;
        
        private void Start()
        {
            MenuManager = GameObject.FindWithTag("MenuManager").GetComponent<BuildingMenu>();
            currentHealth = maxHealth;
        }
    
        public abstract void ActivateMenu();
    }
}
