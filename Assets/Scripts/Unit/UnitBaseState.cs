using UnityEngine;
using UnityEngine.AI;

public abstract class UnitBaseState : StateMachineBehaviour
{
    protected UnitStateController controller;
    protected Unit unit;
    protected NavMeshAgent agent;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (controller == null)
        {
            controller = animator.GetComponent<UnitStateController>();
            unit = animator.GetComponent<Unit>();
            agent = animator.GetComponent<NavMeshAgent>();
        }
    }
}
