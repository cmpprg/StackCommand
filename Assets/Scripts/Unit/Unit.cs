using UnityEngine;

public class Unit : MonoBehaviour
{
    [Header("Combat Properties")]
    [SerializeField] private float attackRange = 5f;
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float maxHealth = 100f;
    
    private float currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
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
}