using UnityEngine;
using UnityEngine.AI;

public class MoveState : State<EnemyController>
{
#region Variables
    Animator animator;
    CharacterController controller;
    NavMeshAgent agent;

    protected int hasMove = Animator.StringToHash("Move");
    protected int hasMoveSpeed = Animator.StringToHash("MoveSpeed");
#endregion Variables


#region Methods
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
        if (context.Target) {
            agent.SetDestination(context.Target.transform.position);

            if (agent.remainingDistance > context.AttackRange) {
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
        
        agent.ResetPath();
    }
#endregion Methods
}
