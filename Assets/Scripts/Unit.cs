using UnityEngine;

public class Unit : MonoBehaviour
{
    [Header("Unit Properties")]
    [SerializeField] private int maxStackHeight = 10;
    [SerializeField] private float baseRange = 5f;
    [SerializeField] private float baseFirepower = 10f;
    [SerializeField] private float baseDefense = 5f;
    
    [Header("Stack Properties")]
    [SerializeField] private float heightPerStack = 0.5f; // Height added per stacked unit
    private int currentStackHeight = 1;

    // Calculated stats based on stack height
    private float currentRange;
    private float currentFirepower;
    private float currentDefense;

    private void Awake()
    {
        UpdateStats();
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

    // Properties to access current stats
    public int CurrentStackHeight => currentStackHeight;
    public float CurrentRange => currentRange;
    public float CurrentFirepower => currentFirepower;
    public float CurrentDefense => currentDefense;
}