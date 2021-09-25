using UnityEngine;

public class AttackState : State<EnemyController>
{
#region Variables
    Animator animator;
    AttackStateController attackStateController;
    IAttackable attackable;

    protected int attackHash = Animator.StringToHash("Attack");
    protected int attackIndexHash = Animator.StringToHash("AttackIndex");
#endregion Variables


#region Methods
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
#endregion Methods
}
