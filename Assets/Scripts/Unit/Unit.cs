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

    private void Awake()
    {
        currentHealth = maxHealth;

        if (laserBeamPrefab == null)
        {
            Debug.LogError("Unit is missing a LaserBeamEffect prefab reference!");
        }
    }

    public float AttackRange => attackRange;
    
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
}