using UnityEngine;

public class UnitAttackState : UnitBaseState
{
    private const float ATTACK_INTERVAL = 1f;
    private float lastAttackTime;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        Debug.Log("Entering attack state");
        
        lastAttackTime = 0f;
        if (agent != null) agent.isStopped = true;
        
        // Ensure we're facing the target
        if (controller.TargetUnit != null)
        {
            FaceTarget(animator.transform, controller.TargetUnit.transform);
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (controller.TargetUnit == null)
        {
            animator.SetBool("IsAttacking", false);
            return;
        }

        // Update facing direction
        FaceTarget(animator.transform, controller.TargetUnit.transform);

        // Handle attack timing
        if (Time.time - lastAttackTime >= ATTACK_INTERVAL)
        {
            if (controller.CanAttack)
            {
                controller.PerformAttack();
                lastAttackTime = Time.time;
            }
            else
            {
                // Target out of range, switch to follow
                controller.Follow(controller.TargetUnit);
            }
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        controller.StopAttack();
    }

    private void FaceTarget(Transform attacker, Transform target)
    {
        Vector3 direction = (target.position - attacker.position).normalized;
        direction.y = 0; // Keep rotation on horizontal plane
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            attacker.rotation = Quaternion.Slerp(attacker.rotation, lookRotation, Time.deltaTime * 10f);
        }
    }
}