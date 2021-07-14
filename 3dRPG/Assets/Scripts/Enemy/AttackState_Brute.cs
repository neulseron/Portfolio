using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState_Brute : State<EnemyController>
{
    Animator animator;
    AttackStateController attackStateController;
    IAttackable attackable;

    protected int attackHash = Animator.StringToHash("Attack");
    protected int attackIndexHash = Animator.StringToHash("AttackIndex");

    public override void OnInitialized()
    {
        animator = context.GetComponent<Animator>();
        attackStateController = context.GetComponent<AttackStateController>();
        attackable = context.GetComponent<IAttackable>();
    }

    public override void OnEnter()
    {
        if (attackable == null || attackable.CurrentAttackBehaviour == null) {
            stateMachine.ChangeState<IdleState>();
            return;
        }

        //attackStateController.enterAttackStateHandler += OnEnterAttackState();
        //attackStateController.exitAttackStateHandler += OnExitAttackState();

        animator.SetInteger(attackIndexHash, attackable.CurrentAttackBehaviour.animationIndex);
        animator.SetTrigger(attackHash);
    }

    public void OnEnterAttackState()
    {

    }

    public void OnExitAttackState()
    {
        stateMachine.ChangeState<IdleState>();
    }

    public override void Update(float deltaTime)
    {
        
    }

    public override void OnExit()
    {
    }
}
