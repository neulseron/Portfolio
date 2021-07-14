using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State<EnemyController>
{
    Animator animator;
    CharacterController controller;

    public bool isPatrol = false;
    float minIdleTime = 0f;
    float maxIdleTime = 3f;
    float idleTime = 0f;

    protected int hasMove = Animator.StringToHash("Move");
    protected int hasMoveSpeed = Animator.StringToHash("MoveSpeed");

    public override void OnInitialized()
    {
        animator = context.GetComponent<Animator>();
        controller = context.GetComponent<CharacterController>();
    }

    public override void OnEnter()
    {
        animator.SetBool(hasMove, false);
        animator.SetFloat(hasMoveSpeed, 0f);
        controller.Move(Vector3.zero);

        if (isPatrol) {
            idleTime = Random.Range(minIdleTime, maxIdleTime);
        }
    }

    public override void Update(float deltaTime)
    {
        Debug.Log("Idle 상태입니당");
        Transform enemy = context.SearchEnemy();
        
        if (enemy) {
            if (context.IsAvailableAttack) {
                stateMachine.ChangeState<AttackState>();
            } else {
                stateMachine.ChangeState<MoveState>();
            }
        } else if (isPatrol && stateMachine.ElapsedTimeInState > idleTime) {
            stateMachine.ChangeState<PatrolState>();
        }
    }

    public override void OnExit()
    {
    }
}
