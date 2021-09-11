using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DeadState : State<EnemyController>
{
    Animator animator;

    protected int isAliveHash = Animator.StringToHash("IsAlive");

    public override void OnInitialized()
    {
        animator = context.GetComponent<Animator>();
    }

    public override void OnEnter()
    {
        animator.SetBool(isAliveHash, false);
    }

    public override void Update(float deltaTime)
    {
        if (stateMachine.ElapsedTimeInState > 3.0f) {
            if (context.dropItem != null) {
                GameObject.Instantiate(context.dropItem, context.gameObject.transform.position + Vector3.up * 0.5f, Quaternion.identity);
            }
            GameObject.Destroy(context.gameObject);
        }
    }

    public override void OnExit()
    {
        
    }
}
