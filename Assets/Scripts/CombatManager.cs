using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class CombatManager : MonoBehaviour
{
    [Header("Combat Settings")]
    [SerializeField] private float damageMultiplier = 1f;
    [SerializeField] private float minimumDamage = 1f;
    [SerializeField] private LayerMask lineOfSightMask;
    
    [Header("Visual Feedback")]
    [SerializeField] private GameObject hitEffectPrefab;
    [SerializeField] private LaserBeamEffect laserBeamPrefab;
    [SerializeField] private float effectDuration = 0.5f;

    // Events for UI and feedback
    public UnityEvent<Unit, Unit> onCombatStarted = new UnityEvent<Unit, Unit>();
    public UnityEvent<Unit, float> onDamageDealt = new UnityEvent<Unit, float>();
    public UnityEvent<Unit> onUnitDestroyed = new UnityEvent<Unit>();

    // Combat tracking
    private Dictionary<Unit, Unit> currentEngagements = new Dictionary<Unit, Unit>();
    private List<GameObject> activeEffects = new List<GameObject>();

    private static CombatManager instance;
    public static CombatManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<CombatManager>();
                if (instance == null)
                {
                    GameObject go = new GameObject("Combat Manager");
                    instance = go.AddComponent<CombatManager>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        // Update ongoing combat engagements
        List<Unit> unitsToRemove = new List<Unit>();
        
        foreach (var engagement in currentEngagements)
        {
            Unit attacker = engagement.Key;
            Unit target = engagement.Value;

            if (!IsValidEngagement(attacker, target))
            {
                unitsToRemove.Add(attacker);
                continue;
            }
            
            // Check if current attacker can attack
            if (attacker.CanAttack(target))
            {
                // set isAttacking to true and start cooldown
                attacker.StartAttack();
                // Process combat scenario
                ProcessCombat(attacker, target);
            }
        }

        // Clean up invalid engagements
        foreach (var unit in unitsToRemove)
        {
            Debug.Log($"Removing engagement for {unit.name}");
            currentEngagements.Remove(unit);
            unit.EndAttack();
        }
    }

    public void InitiateAttack(Unit attacker, Unit target)
    {
        if (!ValidateAttack(attacker, target))
            return;

        Debug.Log($"Adding engagement for {attacker.name}");
        // Start combat engagement
        currentEngagements[attacker] = target;
        
        // Trigger combat started event
        onCombatStarted.Invoke(attacker, target);
    }

    public void StopAttack(Unit attacker)
    {
        if (currentEngagements.ContainsKey(attacker))
        {
            attacker.EndAttack();
            currentEngagements.Remove(attacker);
        }
    }

    private bool ValidateAttack(Unit attacker, Unit target)
    {
        if (attacker == null || target == null)
            return false;

        if (currentEngagements.ContainsKey(attacker))
            return false;

        return attacker.CanAttack(target);
    }

    private bool IsValidEngagement(Unit attacker, Unit target)
    {
        if (attacker == null || target == null)
            return false;

        // Only check fundamental engagement validity:
        // - Units exist
        // - Within maximum possible engagement range
        // - Line of sight exists (if that's a requirement)
        // - Any other strategic conditions
        
        float maxRange = attacker.CurrentRange;
        float distance = Vector3.Distance(attacker.transform.position, target.transform.position);
        
        return distance <= maxRange * 1.2f; // Maybe add a small buffer for movement
    }

    private void ProcessCombat(Unit attacker, Unit target)
    {
        

        CombatStats attackerStats = attacker.GetCombatStats();
        CombatStats targetStats = target.GetCombatStats();

        float damage = CalculateDamage(attackerStats.firepower, targetStats.defense);

        // Show laser, apply damage, and show hit effect
        ShowLaserBeam(attacker, target);
        ApplyDamage(target, damage);
        ShowHitEffect(target.transform.position);

        // Trigger damage event
        onDamageDealt.Invoke(target, damage);

        // Complete the attack (reset isAttacking, leaving cooldown in place)
        attacker.CompleteAttack();
    }

    private float CalculateDamage(float attackPower, float defense)
    {
        float damage = (attackPower - defense) * damageMultiplier;
        return Mathf.Max(minimumDamage, damage);
    }

    private void ApplyDamage(Unit target, float damage)
    {
        target.TakeDamage(damage);
        
        if (target.GetHealthPercent() <= 0)
        {
            onUnitDestroyed.Invoke(target);
        }
    }

    private void ShowHitEffect(Vector3 position)
    {
        if (hitEffectPrefab != null)
        {
            GameObject effect = Instantiate(hitEffectPrefab, position, Quaternion.identity);
            activeEffects.Add(effect);
            Destroy(effect, effectDuration);
        }
    }

    private void ShowLaserBeam(Unit attacker, Unit target)
    {
        if (laserBeamPrefab != null)
        {
            LaserBeamEffect beam = Instantiate(laserBeamPrefab);
            beam.ShowBeam(attacker.GetWeaponMount(), target.transform.position);
        }
    }

    private void CleanupEffects()
    {
        activeEffects.RemoveAll(effect => effect == null);
    }

    // Helper method to check if a unit is currently engaged in combat
    public bool IsUnitEngaged(Unit unit)
    {
        return currentEngagements.ContainsKey(unit) || currentEngagements.ContainsValue(unit);
    }

    // Helper method to get the current target of a unit
    public Unit GetUnitTarget(Unit unit)
    {
        return currentEngagements.TryGetValue(unit, out Unit target) ? target : null;
    }

    private void OnDestroy()
    {
        foreach (var effect in activeEffects)
        {
            if (effect != null)
                Destroy(effect);
        }
    }
}