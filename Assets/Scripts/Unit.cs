using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Unit : MonoBehaviour
{
    [Header("Unit Properties")]
    [SerializeField] private int maxStackHeight = 10;
    [SerializeField] private float baseRange = 5f;
    [SerializeField] private float baseFirepower = 10f;
    [SerializeField] private float baseDefense = 5f;
    [SerializeField] private float moveSpeed = 5f;
    
    [Header("Stack Properties")]
    [SerializeField] private float heightPerStack = 0.5f;
    private int currentStackHeight = 1;

    // NavMesh properties
    private NavMeshAgent agent;
    public bool IsMoving => agent && !agent.isStopped && agent.remainingDistance > agent.stoppingDistance;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.speed = moveSpeed;
            agent.angularSpeed = 360f;
            agent.acceleration = 8f;
            agent.stoppingDistance = 0.1f;
        }
        
        UpdateStats();
    }

    public void SetDestination(Vector3 destination)
    {
        if (agent != null && agent.isOnNavMesh)
        {
            agent.SetDestination(destination);
            agent.isStopped = false;
        }
    }

    public void StopMoving()
    {
        if (agent != null)
        {
            agent.isStopped = true;
        }
    }

    public bool CanAddToStack()
    {
        return currentStackHeight < maxStackHeight;
    }

    public void AddToStack()
    {
        if (CanAddToStack())
        {
            currentStackHeight++;
            
            // Adjust the visual height
            Vector3 position = transform.position;
            position.y += heightPerStack;
            transform.position = position;
            
            UpdateStats();
            
            // Notify UnitSelection to update indicator position
            var selection = GetComponent<UnitSelection>();
            if (selection != null)
            {
                selection.UpdateIndicatorPosition();
            }
        }
    }

    private void UpdateStats()
    {
        // Scale stats based on stack height
        float heightMultiplier = 1f + (currentStackHeight - 1) * 0.2f; // 20% increase per stack
        
        currentRange = baseRange * heightMultiplier;
        currentFirepower = baseFirepower * heightMultiplier;
        currentDefense = baseDefense * heightMultiplier;

        // Update agent speed based on stack height (optional)
        if (agent != null)
        {
            agent.speed = moveSpeed * (1f / Mathf.Sqrt(currentStackHeight)); // Bigger stacks move slower
        }
    }

    // Helper method to get current height for selection indicator
    public float GetBaseHeight()
    {
        return (currentStackHeight - 1) * heightPerStack;
    }

    // Properties to access current stats
    public int CurrentStackHeight => currentStackHeight;
    public float CurrentRange => currentRange;
    public float CurrentFirepower => currentFirepower;
    public float CurrentDefense => currentDefense;

    private float currentRange;
    private float currentFirepower;
    private float currentDefense;
}