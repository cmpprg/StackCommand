using UnityEngine;
using UnityEngine.AI;

public class MoveState : UnitBaseState
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        
        if (agent != null)
        {
            agent.isStopped = false;
            agent.SetDestination(controller.TargetPosition);
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (agent == null) return;

        if (!agent.pathStatus.Equals(NavMeshPathStatus.PathInvalid))
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                animator.SetBool("IsMoving", false);
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