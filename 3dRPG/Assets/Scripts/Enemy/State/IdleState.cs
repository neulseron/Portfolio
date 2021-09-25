using UnityEngine;

public class IdleState : State<EnemyController>
{
#region Variables
    Animator animator;
    CharacterController controller;

    public bool isPatrol = false;
    float minIdleTime = 0f;
    float maxIdleTime = 3f;
    float idleTime = 0f;

    protected int hasMove = Animator.StringToHash("Move");
    protected int hasMoveSpeed = Animator.StringToHash("MoveSpeed");
#endregion Variables


#region Methods
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

        if (context is EnemyController_Patrol) {
            isPatrol = true;
            idleTime = Random.Range(minIdleTime, maxIdleTime);
        }
    }

    public override void Update(float deltaTime)
    {
        if (context.Target) {
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
#endregion Methods
}
