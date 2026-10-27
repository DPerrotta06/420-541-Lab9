using UnityEngine;
using UnityEngine.SceneManagement;

public class AttackState : IState
{
    private AIController aiController;
    private float attackCoolDown = 1.5f;
    private float lastAttackTime = -999f;

    public StateType Type => StateType.Attack;
    public AttackState(AIController aiController)
    {
        this.aiController = aiController;
    }
    public void Enter()
    {
        aiController.Agent.isStopped = true;
    }
    public void Execute()
    {
        if (Vector3.Distance(aiController.transform.position, aiController.Player.position) > aiController.AttackRange)
        {
            aiController.StateMachine.TransitionToState(StateType.Chase);
            return;
        }
        if (Time.time - lastAttackTime > attackCoolDown)
        {
            lastAttackTime = Time.time;
            aiController.aiAnimationController.animator.SetTrigger("doAttack");
        }
    }
    public void Exit()
    {
        aiController.Agent.isStopped = false;
    }
}
