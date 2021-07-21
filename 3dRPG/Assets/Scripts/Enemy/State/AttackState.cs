// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class AttackState : State<EnemyController>
// {
//     Animator animator;
//     AttackStateController attackStateController;
//     IAttackable attackable;

//     protected int attackHash = Animator.StringToHash("Attack");

//     public override void OnInitialized()
//     {
//         animator = context.GetComponent<Animator>();
//         attackStateController = context.GetComponent<AttackStateController>();
//         attackable = context.GetComponent<IAttackable>();
//     }

//     public override void OnEnter()
//     {
//         if (context.IsAvailableAttack) {
//             animator.SetTrigger(attackHash);
//         } else {
//             stateMachine.ChangeState<IdleState>();
//         }
//     }

//     public override void Update(float deltaTime)
//     {
//         Debug.Log("Attack 상태입니당");
//     }

//     public override void OnExit()
//     {
//     }
// }

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State<EnemyController>
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

        attackStateController.enterAttackHandler += OnEnterAttackState;
        attackStateController.exitAttackHandler += OnExitAttackState;

        animator.SetInteger(attackIndexHash, attackable.CurrentAttackBehaviour.animationIndex);
        animator.SetTrigger(attackHash);
    }

    public override void Update(float deltaTime)
    {
    }

    public override void OnExit()
    {
        attackStateController.enterAttackHandler -= OnEnterAttackState;
        attackStateController.exitAttackHandler -= OnExitAttackState;
    }

    public void OnEnterAttackState()
    {
    }

    public void OnExitAttackState()
    {
        stateMachine.ChangeState<IdleState>();
    }

    
}
