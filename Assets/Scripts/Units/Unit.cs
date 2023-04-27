using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    [SerializeField] private Vector3 _selectedPosition;

    [SerializeField] private int maxHealth;
    private int _currentHealth;
    private int _damage;

    public int CurrentHealth
    {
        get => _currentHealth;
        set => _currentHealth = value;
    }

    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        
        _currentHealth = maxHealth;
        _selectedPosition = transform.position;
    }

    public void SelectNewPosition(Vector3 newPosition)
    {
        _selectedPosition = newPosition;
        GoToPosition();
    }

    private void GoToPosition()
    {
        _navMeshAgent.destination = _selectedPosition;
    }
}
