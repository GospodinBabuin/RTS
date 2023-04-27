using UnityEngine;

public abstract class Building : MonoBehaviour
{
    protected BuildingMenu MenuManager;

    [SerializeField] private int maxHealth;
    protected int currentHealth;

    [SerializeField] private Transform unintSpawnPosition;
    public Transform UnitSpawnPosition => unintSpawnPosition;

    private void Start()
    {
        MenuManager = GameObject.FindWithTag("MenuManager").GetComponent<BuildingMenu>();
        currentHealth = maxHealth;
    }
    
    public abstract void ActivateMenu();
}
