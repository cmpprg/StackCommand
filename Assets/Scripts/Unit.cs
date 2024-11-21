using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Unit : MonoBehaviour
{
    [Header("Unit Properties")]
    [SerializeField] private int maxStackHeight = 10;
    [SerializeField] private float moveSpeed = 5f;
    
    [Header("Combat Properties")]
    [SerializeField] private float baseRange = 5f;
    [SerializeField] private float baseFirepower = 10f;
    [SerializeField] private float baseDefense = 5f;
    
    [Header("Stack Properties")]
    [SerializeField] private float heightPerStack = 0.5f;
    private int currentStackHeight = 1;

    // Component references
    private NavMeshAgent agent;
    private CombatStats combatStats;

    // Properties
    public bool IsMoving => agent && !agent.isStopped && agent.remainingDistance > agent.stoppingDistance;
    public int CurrentStackHeight => currentStackHeight;
    public float CurrentRange => combatStats.range;
    public float CurrentFirepower => combatStats.firepower;
    public float CurrentDefense => combatStats.defense;

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

    private void Update()
    {
        combatStats.UpdateCooldown(Time.deltaTime);
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
        float heightMultiplier = 1f + (currentStackHeight - 1) * 0.2f; // 20% increase per stack
        
        // Update combat stats with stack multipliers
        combatStats = new CombatStats(
            baseRange * heightMultiplier,
            baseFirepower * heightMultiplier,
            baseDefense * heightMultiplier,
            currentStackHeight
        );

        // Update agent speed based on stack height
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

    // Combat-related methods
    public bool CanAttack(Unit target)
    {
        return combatStats.CanAttack() &&
            combatStats.CanTargetUnit(transform.position, target);
    }


    public void StartAttack()
    {
        combatStats.isAttacking = true;
        combatStats.currentCooldown = combatStats.attackCooldown;
    }

    public void CompleteAttack()
    {
        // Cooldown remains, but isAttacking is reset
        combatStats.isAttacking = false;
    }

    public void EndAttack()
    {
        combatStats.isAttacking = false;
    }

    public bool IsAttacking()
    {
        return combatStats.isAttacking;
    }

    public CombatStats GetCombatStats()
    {
        return combatStats;
    }

    public Transform GetWeaponMount()
    {
        // Find and return the nose transform
        return transform.Find("Nose");
    }
}