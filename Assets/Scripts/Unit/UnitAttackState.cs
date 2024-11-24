using UnityEngine;

public class UnitAttackState : UnitBaseState
{
    private float attackCooldown = 1f;
    private float currentCooldown = 0f;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        
        // Check range immediately on entering the state
        if (controller.TargetUnit != null)
        {
            float distanceToTarget = Vector3.Distance(animator.transform.position, controller.TargetUnit.transform.position);
            if (distanceToTarget > unit.AttackRange)
            {
                // If out of range, transition to follow state
                controller.Follow(controller.TargetUnit);
                return;
            }
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (controller.TargetUnit == null)
        {
            controller.Stop();
            return;
        }

        // Update facing direction
        Vector3 directionToTarget = (controller.TargetUnit.transform.position - animator.transform.position).normalized;
        animator.transform.rotation = Quaternion.LookRotation(directionToTarget);

        // Check if still in range
        float distanceToTarget = Vector3.Distance(animator.transform.position, controller.TargetUnit.transform.position);
        if (distanceToTarget > unit.AttackRange)
        {
            // If we're out of range, switch to follow state
            controller.Follow(controller.TargetUnit);
            return;
        }

        // Handle attack timing
        if (currentCooldown <= 0)
        {
            PerformAttack();
            currentCooldown = attackCooldown;
        }
        
        currentCooldown -= Time.deltaTime;
    }

    private void PerformAttack()
    {
        if (controller.TargetUnit != null)
        {
            controller.TargetUnit.TakeDamage(unit.AttackDamage);
            ShowLaserBeam(controller.TargetUnit);
        }
    }

    private void ShowLaserBeam(Unit target)
    {
        if (unit.LaserBeamPrefab != null)
        {
            Vector3 targetPoint = target.transform.position;
            LaserBeamEffect laser = Instantiate(unit.LaserBeamPrefab);
            laser.ShowBeam(agent.transform, targetPoint);
        }
    }
}