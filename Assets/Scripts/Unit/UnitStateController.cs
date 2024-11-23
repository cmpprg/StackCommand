using UnityEngine;
using UnityEngine.AI;

public class UnitStateController : MonoBehaviour
{
    // Components
    private Unit unit;
    private Animator animator;
    private NavMeshAgent agent;

    // State Parameters
    public bool IsMoving { get; private set; }
    public bool IsAttacking { get; private set; }
    public Vector3 TargetPosition { get; private set; }
    public Unit TargetUnit { get; private set; }

    private void Awake()
    {
        unit = GetComponent<Unit>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    public void MoveTo(Vector3 position)
    {
        TargetPosition = position;
        TargetUnit = null;
        animator.SetBool("IsMoving", true);
        animator.SetBool("IsAttacking", false);
    }

    public void Follow(Unit target)
    {
        TargetUnit = target;
        animator.SetBool("IsMoving", true);
        animator.SetBool("IsFollowing", true);
        animator.SetBool("IsAttacking", false);
    }

    public void Attack(Unit target)
    {
        TargetUnit = target;
        animator.SetBool("IsAttacking", true);
    }

    public void Stop()
    {
        TargetUnit = null;
        animator.SetBool("IsMoving", false);
        animator.SetBool("IsFollowing", false);
        animator.SetBool("IsAttacking", false);
    }
}