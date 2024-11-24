using UnityEngine;

public class UnitAttackState : UnitBaseState
{
    private float attackCooldown = 1f;
    private float currentCooldown = 0f;
    private LaserBeamEffect laserBeamPrefab;

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (controller.TargetUnit == null)
        {
            animator.SetBool("IsAttacking", false);
            return;
        }

        // Update facing direction
        Vector3 directionToTarget = (controller.TargetUnit.transform.position - animator.transform.position).normalized;
        animator.transform.rotation = Quaternion.LookRotation(directionToTarget);

        // Check if in range
        float distanceToTarget = Vector3.Distance(animator.transform.position, controller.TargetUnit.transform.position);
        if (distanceToTarget > unit.AttackRange)
        {
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
            // Deal damage
            controller.TargetUnit.TakeDamage(unit.AttackDamage);
            // Show laser beam effect
            ShowLaserBeam(controller.TargetUnit);
            // Play attack sound
            // TODO: Add sound effect
            // Play hit animation
            // TODO: Add hit animation
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