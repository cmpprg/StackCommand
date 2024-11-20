using UnityEngine;

[System.Serializable]
public struct CombatStats
{
    // Base stats
    public float range;
    public float firepower;
    public float defense;
    
    // Combat state
    public bool isAttacking;
    public float attackCooldown;
    public float currentCooldown;
    
    // Line of sight
    public int stackHeight;
    public float lineOfSightHeight;

    public CombatStats(float range, float firepower, float defense, int stackHeight)
    {
        this.range = range;
        this.firepower = firepower;
        this.defense = defense;
        this.stackHeight = stackHeight;
        
        // Initialize combat state
        isAttacking = false;
        attackCooldown = 1f; // 1 second default cooldown
        currentCooldown = 0f;
        
        // Calculate line of sight height based on stack
        lineOfSightHeight = stackHeight * 0.5f; // 0.5f is heightPerStack from Unit.cs
    }

    public void UpdateCooldown(float deltaTime)
    {
        if (currentCooldown > 0)
        {
            currentCooldown = Mathf.Max(0, currentCooldown - deltaTime);
        }
    }

    public bool CanAttack()
    {
        return !isAttacking && currentCooldown <= 0;
    }

    public bool CanTargetUnit(Vector3 attackerPosition, Unit targetUnit)
    {
        // Check range
        float distance = Vector3.Distance(attackerPosition, targetUnit.transform.position);
        if (distance > range) return false;

        // Check line of sight
        float targetHeight = targetUnit.GetComponent<Unit>().GetBaseHeight();
        
        // Ray starts from attacker's height
        Vector3 rayStart = attackerPosition + Vector3.up * lineOfSightHeight;
        Vector3 rayEnd = targetUnit.transform.position + Vector3.up * targetHeight;
        
        // Check for obstacles between units
        Ray ray = new Ray(rayStart, (rayEnd - rayStart).normalized);
        if (Physics.Raycast(ray, out RaycastHit hit, distance))
        {
            Unit hitUnit = hit.collider.GetComponent<Unit>();
            // If we hit something that's not our target and blocks line of sight
            if (hitUnit != null && hitUnit != targetUnit && 
                hitUnit.GetBaseHeight() > lineOfSightHeight)
            {
                return false;
            }
        }

        return true;
    }
}