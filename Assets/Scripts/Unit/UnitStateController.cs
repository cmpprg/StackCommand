using UnityEngine;
using UnityEngine.AI;

public class UnitStateController : MonoBehaviour
{
    private Unit unit;
    private Animator animator;
    private NavMeshAgent agent;

    public bool IsAttacking { get; private set; }
    public Vector3 TargetPosition { get; private set; }
    public Unit TargetUnit { get; private set; }

    // Attack-related properties
    public float AttackRange => unit.CurrentRange;
    public bool CanAttack => unit.CanAttack(TargetUnit);
    
    private void Awake()
    {
        unit = GetComponent<Unit>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    public void Attack(Unit target)
    {
        Debug.Log("UnitStateController#Attack");
        Debug.Log("UnitStateController#Attack - target: " + target);
        if (target == null || target == unit) return;
        
        TargetUnit = target;
        animator.SetBool("IsAttacking", true);
        animator.SetBool("IsMoving", false);
        animator.SetBool("IsFollowing", false);
    }

    public void StopAttack()
    {
        if (IsAttacking)
        {
            CombatManager.Instance.StopAttack(unit);
            animator.SetBool("IsAttacking", false);
        }
        TargetUnit = null;
    }

    public void PerformAttack()
    {
        if (TargetUnit != null && unit.CanAttack(TargetUnit))
        {
            CombatManager.Instance.InitiateAttack(unit, TargetUnit);
        }
        else
        {
            // If target is out of range, transition to follow state
            Follow(TargetUnit);
        }
    }

    public void Follow(Unit target)
    {
        if (target == null || target == unit) return;
        
        TargetUnit = target;
        animator.SetBool("IsAttacking", false);
        animator.SetBool("IsFollowing", true);
        animator.SetBool("IsMoving", false);
    }

    public void MoveTo(Vector3 position)
    {
        TargetPosition = position;
        TargetUnit = null;
        animator.SetBool("IsAttacking", false);
        animator.SetBool("IsMoving", true);
    }

    public void Stop()
    {
        TargetUnit = null;
        animator.SetBool("IsMoving", false);
        animator.SetBool("IsFollowing", false);
        animator.SetBool("IsAttacking", false);
    }
}