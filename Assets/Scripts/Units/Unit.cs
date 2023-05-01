using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class Unit : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    [SerializeField] private Vector3 _selectedPosition;

    [SerializeField] private int damage;
    [SerializeField] private int maxHealth;
    private int _currentHealth;

    [SerializeField] private int cost;
    public int Cost => cost;

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
