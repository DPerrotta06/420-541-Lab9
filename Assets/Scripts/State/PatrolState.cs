using UnityEngine;
using System.Collections;

public class PatrolState : IState
{
    private AIController aiController;
    private int currentWayPointIndex = 0;
    private bool isWaiting = false;

    public StateType Type => StateType.Patrol;

    public PatrolState(AIController aiController)
    {
        this.aiController = aiController;
    }
    public void Enter()
    {
        aiController.Agent.isStopped = false;
        MoveToNextWayPoint();
    }

    public void Execute()
    {
        if (aiController.CanSeePlayer())
        {
            aiController.StateMachine.TransitionToState(StateType.Chase);
            return;
        }
        if (!isWaiting && !aiController.Agent.pathPending && aiController.Agent.remainingDistance <= aiController.Agent.stoppingDistance)
        {
            aiController.StartCoroutine(WaitAndAnimate());
        }
    }

    public void Exit()
    {
        aiController.Agent.isStopped = false;
    }

    private IEnumerator WaitAndAnimate()
    {
        isWaiting = true;
        aiController.Agent.isStopped = true;
        aiController.aiAnimationController.animator.SetTrigger("doScream");
        yield return new WaitForSeconds(5);
        aiController.Agent.isStopped = false;
        MoveToNextWayPoint();
        isWaiting = false;
    }

    private void MoveToNextWayPoint()
    {
        if (aiController.Waypoints.Length == 0)
        {
            return;
        }
        aiController.Agent.destination = aiController.Waypoints[currentWayPointIndex].position;
        currentWayPointIndex = (currentWayPointIndex + 1) % aiController.Waypoints.Length;
    }
}
