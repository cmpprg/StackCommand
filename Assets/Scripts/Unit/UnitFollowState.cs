using UnityEngine;

public class UnitFollowState : UnitBaseState
{
    private float updatePathInterval = 0.2f;
    private float lastPathUpdate;
    private float checkRangeInterval = 0.1f;
    private float lastRangeCheck;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        
        lastPathUpdate = 0f;
        lastRangeCheck = 0f;

        if (agent != null)
        {
            agent.isStopped = false;
            UpdatePath();
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (agent == null || controller.TargetUnit == null)
        {
            controller.Stop();
            return;
        }

        // Update path to target periodically
        if (Time.time - lastPathUpdate > updatePathInterval)
        {
            UpdatePath();
            lastPathUpdate = Time.time;
        }

        // Check range periodically
        if (Time.time - lastRangeCheck > checkRangeInterval)
        {
            float distanceToTarget = Vector3.Distance(animator.transform.position, controller.TargetUnit.transform.position);
            
            // Attack the target if in range
            if (distanceToTarget <= unit.AttackRange)
            {
                controller.Attack(controller.TargetUnit);
            }
            lastRangeCheck = Time.time;
        }
    }

    private void UpdatePath()
    {
        if (controller.TargetUnit != null)
        {
            agent.SetDestination(controller.TargetUnit.transform.position);
        }
    }
}