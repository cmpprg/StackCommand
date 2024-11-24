using UnityEngine;

public class UnitStateController : MonoBehaviour
{
    private Animator animator;
    private Unit unit;
    
    public Unit TargetUnit { get; private set; }
    public Vector3 TargetPosition { get; private set; }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        unit = GetComponent<Unit>();
    }

    public void Attack(Unit target)
    {
        if (target == null || target == unit) return;
        
        TargetUnit = target;
        animator.SetBool("IsAttacking", true);
        animator.SetBool("IsMoving", false);
        animator.SetBool("IsFollowing", false);
    }

    public void Follow(Unit target)
    {
        if (target == null || target == unit) return;
        
        TargetUnit = target;
        animator.SetBool("IsFollowing", true);
        animator.SetBool("IsAttacking", false);
        animator.SetBool("IsMoving", false);
    }

    public void MoveTo(Vector3 position)
    {
        TargetPosition = position;
        TargetUnit = null;
        animator.SetBool("IsMoving", true);
        animator.SetBool("IsFollowing", false);
        animator.SetBool("IsAttacking", false);
    }

    public void Stop()
    {
        TargetUnit = null;
        animator.SetBool("IsMoving", false);
        animator.SetBool("IsFollowing", false);
        animator.SetBool("IsAttacking", false);
    }
}