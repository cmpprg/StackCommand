using UnityEngine;

public class UnitIdleState : UnitBaseState
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        
        if (agent != null)
        {
            agent.isStopped = true;
            agent.ResetPath(); // Clear any remaining path
        }
        
        // Reset any ongoing actions in Unit/Controller
        // unit.EndAttack(); // Ensure no lingering attack state
        controller.Stop(); // Reset controller states
    }
}
