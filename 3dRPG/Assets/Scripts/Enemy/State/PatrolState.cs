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

    EnemyController_Patrol patrolController;

    Transform[] Waypoints => context.waypoints;
    [HideInInspector]
    public Transform targetWaypoint = null;
    int waypointIndex = 0;
#endregion Variables


#region Methods
    public override void OnInitialized()
    {
        animator = context.GetComponent<Animator>();
        controller = context.GetComponent<CharacterController>();
        agent = context.GetComponent<NavMeshAgent>();

        patrolController = context as EnemyController_Patrol;
    }

    public override void OnEnter()
    {
        agent.stoppingDistance = 0.02f;

        if (targetWaypoint == null) {
            FindNextWaypoint();
        }

        if (targetWaypoint) {
            agent.SetDestination(targetWaypoint.position);
            animator.SetBool(hasMove, true);
        } else {
            stateMachine.ChangeState<IdleState>();
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
        } else {
            if (!agent.pathPending && (agent.remainingDistance <= agent.stoppingDistance)) {    // 이동할 거리가 안남았으면
                FindNextWaypoint();
                stateMachine.ChangeState<IdleState>();
            } else {
                controller.Move(agent.velocity * deltaTime);
                animator.SetFloat(hasMoveSpeed, agent.velocity.magnitude / agent.speed, 1f, deltaTime);
            }
        }
    }

    public override void OnExit()
    {
        agent.stoppingDistance = context.AttackRange;
        animator.SetBool(hasMove, false);
        agent.ResetPath();
    }

    public void FindNextWaypoint()
    {
        targetWaypoint = null;

        if (Waypoints != null && Waypoints.Length > 0) {
            targetWaypoint = Waypoints[waypointIndex];
        }
        waypointIndex = (waypointIndex + 1) % Waypoints.Length;
    }
#endregion Methods
}
