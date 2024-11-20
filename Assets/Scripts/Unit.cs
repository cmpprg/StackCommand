using UnityEngine;

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

    // Movement properties
    private Vector3 targetPosition;
    private bool isMoving = false;
    
    // Calculated stats based on stack height
    private float currentRange;
    private float currentFirepower;
    private float currentDefense;

    private void Awake()
    {
        UpdateStats();
    }

    private void Update()
    {
        if (isMoving)
        {
            MoveToTarget();
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
    }
    
    // Movement methods
    public void SetDestination(Vector3 destination)
    {
        targetPosition = destination;
        isMoving = true;
    }
    
    private void MoveToTarget()
    {
        // Calculate direction to target
        Vector3 direction = (targetPosition - transform.position).normalized;
        direction.y = 0; // Keep unit level with ground
        
        // Move towards target
        transform.position += direction * moveSpeed * Time.deltaTime;
        
        // Check if we've reached the target
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);
        if (distanceToTarget < 0.1f)
        {
            isMoving = false;
            transform.position = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);
            
            // Update selection indicator position after movement
            var selection = GetComponent<UnitSelection>();
            if (selection != null && selection.IsSelected)
            {
                selection.UpdateIndicatorPosition();
            }
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
}