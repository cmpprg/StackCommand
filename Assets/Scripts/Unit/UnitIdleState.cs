using UnityEngine;

public class UnitIdleState : UnitBaseState
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        
        if (agent != null)
        {
            agent.isStopped = true;
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Check for new commands or state changes
        if (controller.IsMoving || controller.IsAttacking)
        {
            return;
        }
    }
}
