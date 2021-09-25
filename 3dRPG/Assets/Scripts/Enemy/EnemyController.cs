using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
#region Variables
    protected StateMachine<EnemyController> stateMachine;

    protected FieldOfView fov;
    protected Animator animator;
    protected NavMeshAgent agent;
    public virtual float AttackRange => 3.0f;

    public Transform Target => fov.NearestTarget;
    public LayerMask TargetMask => fov.targetMask;

    public Transform[] waypoints;
    
    public GameObject dropItem = null;
#endregion Variables


#region Unity Methods
    protected virtual void Start()
    {
        stateMachine = new StateMachine<EnemyController>(this, new IdleState());

        fov = GetComponent<FieldOfView>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    protected virtual void Update() 
    {
        stateMachine.Update(Time.deltaTime);
        if (!(stateMachine.CurrState is MoveState) && !(stateMachine.CurrState is DeadState))
        {
            FaceToTarget();
        }
    }
#endregion Unity Methods


#region Methods
    public virtual bool IsAvailableAttack => false;

    void FaceToTarget()
    {
        if (Target) {
            Vector3 direction = (Target.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10.0f);
        }
    }
#endregion Other Methods
}
