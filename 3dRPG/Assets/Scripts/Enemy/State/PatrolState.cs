using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : State<EnemyController>
{
    #region Variables
    Animator animator;
    CharacterController controller;
    NavMeshAgent agent;

    protected int hasMove = Animator.StringToHash("Move");
    protected int hasMoveSpeed = Animator.StringToHash("MoveSpeed");

    Transform[] Waypoints => context.waypoints;
    [HideInInspector]
    public Transform targetWaypoint = null;
    int waypointIndex = 0;
    #endregion Variables


    public override void OnInitialized()
    {
        animator = context.GetComponent<Animator>();
        controller = context.GetComponent<CharacterController>();
        agent = context.GetComponent<NavMeshAgent>();
    }

    public override void OnEnter()
    {
        if (targetWaypoint == null) {
            FindNextWaypoint();
        }

        if (targetWaypoint) {
            agent.SetDestination(targetWaypoint.position);
            animator.SetBool(hasMove, true);
        }
    }

    public override void Update(float deltaTime)
    {
        Debug.Log("Patrol 상태입니당");
        Transform enemy = context.SearchEnemy();
        
        if (enemy) {
            if (context.IsAvailableAttack) {
                stateMachine.ChangeState<AttackState>();
            } else {
                stateMachine.ChangeState<MoveState>();
            }
        } else {
            if (!agent.pathPending && (agent.remainingDistance <= agent.stoppingDistance)) {    // 이동할 거리가 안남았으면
                Transform nextDest =  FindNextWaypoint();
                if (nextDest) {
                    agent.SetDestination(nextDest.position);
                }

                stateMachine.ChangeState<IdleState>();
            } else {
                controller.Move(agent.velocity * deltaTime);
                animator.SetFloat(hasMoveSpeed, agent.velocity.magnitude / agent.speed, 1f, deltaTime);
            }
        }
    }

    public override void OnExit()
    {
        animator.SetBool(hasMove, false);
        agent.ResetPath();
    }

    public Transform FindNextWaypoint()
    {
        targetWaypoint = null;

        if (Waypoints.Length > 0) {
            targetWaypoint = Waypoints[waypointIndex];
        }
        waypointIndex = (waypointIndex + 1) % Waypoints.Length;

        return targetWaypoint;
    }
}
