using UnityEngine;

public class FollowState : UnitBaseState
{
    private float updatePathInterval = 0.2f;
    private float lastPathUpdate;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        lastPathUpdate = 0f;
        
        if (agent != null)
        {
            agent.isStopped = false;
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (agent == null || controller.TargetUnit == null) 
        {
            animator.SetBool("IsFollowing", false);
            return;
        }

        // Update path to target periodically
        if (Time.time - lastPathUpdate > updatePathInterval)
        {
            agent.SetDestination(controller.TargetUnit.transform.position);
            lastPathUpdate = Time.time;

            // Check if within attack range
            if (unit.CanAttack(controller.TargetUnit))
            {
                animator.SetBool("IsAttacking", true);
            }
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (agent != null)
        {
            agent.isStopped = true;
        }
    }
}