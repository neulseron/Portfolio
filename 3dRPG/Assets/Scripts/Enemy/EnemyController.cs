using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    #region Variables
    protected StateMachine<EnemyController> stateMachine;
    public StateMachine<EnemyController> StateMachine => stateMachine;

    protected FieldOfView fov;
    protected Animator animator;
    protected NavMeshAgent agent;

    public virtual float AttackRange => 3.0f;
    public Transform Target => fov.NearestTarget;
    public LayerMask TargetMask => fov.targetMask;

    public Transform[] waypoints;
    
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
    }
    #endregion Unity Methods


    #region Other Methods
    public virtual bool IsAvailableAttack => false;

    public Transform SearchEnemy()
    {
        return Target;
    }

    /*
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, viewRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
    */

    
    #endregion Other Methods
}
