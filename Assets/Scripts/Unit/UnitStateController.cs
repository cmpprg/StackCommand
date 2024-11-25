using UnityEngine;

public class UnitStateController : MonoBehaviour
{
    private static readonly int MoveTrigger = Animator.StringToHash("Move");
    private static readonly int AttackTrigger = Animator.StringToHash("Attack");
    private static readonly int FollowTrigger = Animator.StringToHash("Follow");
    private static readonly int StopTrigger = Animator.StringToHash("Stop");
    
    private Animator animator;
    private Unit unit;
    
    public Unit TargetUnit { get; private set; }
    public Vector3 TargetPosition { get; private set; }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        unit = GetComponent<Unit>();
    }

    public void MoveTo(Vector3 position)
    {
        TargetPosition = position;
        TargetUnit = null;
        ResetAllTriggers();
        animator.SetTrigger(MoveTrigger);
    }

    public void Attack(Unit target)
    {
        if (target == null || target == unit) return;
        // Add team targeting check
        if (!unit.CanTarget(target)) return;
        
        TargetUnit = target;
        ResetAllTriggers();
        animator.SetTrigger(AttackTrigger);
    }

    public void Follow(Unit target)
    {
        if (target == null || target == unit) return;
        
        TargetUnit = target;
        ResetAllTriggers();
        animator.SetTrigger(FollowTrigger);
    }

    public void Stop()
    {
        TargetUnit = null;
        ResetAllTriggers();
        animator.SetTrigger(StopTrigger);
    }
    
    private void ResetAllTriggers()
    {
        animator.ResetTrigger(MoveTrigger);
        animator.ResetTrigger(AttackTrigger);
        animator.ResetTrigger(FollowTrigger);
        animator.ResetTrigger(StopTrigger);
    }
}