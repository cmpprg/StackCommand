using UnityEngine;

public class Unit : MonoBehaviour
{
    [Header("Combat Properties")]
    [SerializeField] private float attackRange = 5f;
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float maxHealth = 100f;

    [Header("Effects")]
    [SerializeField] private LaserBeamEffect laserBeamPrefab;
    
    private float currentHealth;
    private TeamComponent teamComponent;

    private void Awake()
    {
        currentHealth = maxHealth;
        teamComponent = GetComponent<TeamComponent>();

        if (laserBeamPrefab == null)
        {
            Debug.LogError("Unit is missing a LaserBeamEffect prefab reference!");
        }
        if (teamComponent == null)
        {
            Debug.LogError($"Unit {gameObject.name} requires a TeamComponent!");
        }
    }

    public float AttackRange => attackRange;
    public float AttackDamage => attackDamage;
    
    public void TakeDamage(float damage)
    {
        currentHealth = Mathf.Max(0, currentHealth - damage);
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    public float GetHealthPercent() => currentHealth / maxHealth;

    public LaserBeamEffect LaserBeamPrefab => laserBeamPrefab;

    public bool CanTarget(Unit otherUnit)
    {
        if (otherUnit == null || otherUnit == this)
            return false;

        TeamComponent otherTeam = otherUnit.GetComponent<TeamComponent>();
        return teamComponent.IsHostileTo(otherTeam);
    }
}