using UnityEngine;

public class AttackState : UnitBaseState
{
    private float attackInterval = 1f;
    private float lastAttackTime;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        lastAttackTime = 0f;
        
        if (agent != null)
        {
            agent.isStopped = true;
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
{
    if (controller.TargetUnit == null)
    {
        animator.SetBool("IsAttacking", false);
        return;
    }

    // Simple look at target
    animator.transform.LookAt(controller.TargetUnit.transform);

    // Attack logic
    if (Time.time - lastAttackTime >= attackInterval && unit.CanAttack(controller.TargetUnit))
    {
        unit.StartAttack();
        CombatManager.Instance.InitiateAttack(unit, controller.TargetUnit);
        lastAttackTime = Time.time;
    }
    else if (!unit.CanAttack(controller.TargetUnit))
    {
        // Target out of range, switch to follow
        animator.SetBool("IsAttacking", false);
        animator.SetBool("IsFollowing", true);
    }
}

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        unit.EndAttack();
    }
}