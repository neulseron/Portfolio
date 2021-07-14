using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleRandomStateMachineBehaviour : StateMachineBehaviour
{
    #region Variables
    public int numberOfStates = 3;
    public float minNormalTime = 0f;
    public float maxNormalTime = 5f;

    public float randomNormalTime;

    readonly int hashRandomIdle = Animator.StringToHash("RandomIdle");
    #endregion Variables
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 애니메이션 처음 시작될 때
        randomNormalTime = Random.Range(minNormalTime, maxNormalTime);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Base 레이어 상태일 때는 동작하지 않도록
        if (animator.IsInTransition(0) && animator.GetCurrentAnimatorStateInfo(0).fullPathHash == stateInfo.fullPathHash) {
            animator.SetInteger(hashRandomIdle, -1);
        }

        // 재생 시간 초과했을 때
        if (stateInfo.normalizedTime > randomNormalTime && !animator.IsInTransition(0)) {
            animator.SetInteger(hashRandomIdle, Random.Range(0, numberOfStates));
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
