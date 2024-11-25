using UnityEngine;
using UnityEngine.AI;

public class UnitMoveState : UnitBaseState
{
    private bool IsPathValid => agent?.pathStatus != NavMeshPathStatus.PathInvalid;
    private Vector3 currentDestination;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        
        if (agent != null)
        {
            agent.isStopped = false;
            agent.ResetPath(); // Clear any existing path
            
            UpdateDestination(controller.TargetPosition);
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (agent == null) return;

        // Check if we have a new destination
        if (currentDestination != controller.TargetPosition)
        {
            UpdateDestination(controller.TargetPosition);
        }

        // Check if we've reached our destination
        if (IsPathValid && HasReachedDestination())
        {
            controller.Stop();
            return;
        }

        // Handle path failure
        if (!IsPathValid)
        {
            Debug.LogWarning("Invalid path detected in MoveState");
            controller.Stop();
            return;
        }
    }

    private void UpdateDestination(Vector3 newDestination)
    {
        currentDestination = newDestination;
        agent.SetDestination(newDestination);
    }

    private bool HasReachedDestination()
    {
        // Consider both distance and velocity for a more accurate "arrived" check
        return agent.remainingDistance <= agent.stoppingDistance &&
               agent.velocity.sqrMagnitude < 0.1f;
    }
}