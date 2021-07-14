using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DeadState : State<EnemyController>
{
    Animator animator;
    CharacterController controller;
    NavMeshAgent agent;

    protected int hasMove = Animator.StringToHash("Move");
    protected int hasMoveSpeed = Animator.StringToHash("MoveSpeed");

    public override void OnInitialized()
    {
        animator = context.GetComponent<Animator>();
        controller = context.GetComponent<CharacterController>();
        agent = context.GetComponent<NavMeshAgent>();
    }

    public override void OnEnter()
    {
        agent.SetDestination(context.Target.position);
        
        animator.SetBool(hasMove, true);
    }

    public override void Update(float deltaTime)
    {
        Transform enemy = context.SearchEnemy();
        if (enemy) {
            agent.SetDestination(context.Target.transform.position);

            if (agent.remainingDistance > agent.stoppingDistance) {
                controller.Move(agent.velocity * deltaTime);
                animator.SetFloat(hasMoveSpeed, agent.velocity.magnitude / agent.speed, 1f, deltaTime);     // damp 보간
                return;
            }
        }
            
        stateMachine.ChangeState<IdleState>();
    }

    public override void OnExit()
    {
        animator.SetBool(hasMove, false);
        //animator.SetFloat(hasMoveSpeed, 0f);
        
        agent.ResetPath();
    }
}
