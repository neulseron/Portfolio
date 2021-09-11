 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonAttackBehaviour : AttackBehaviour
{
    public Transform[] summonPos;
    public GameObject summons;

    public override void ExcuteAttack(GameObject target = null, Transform startPoint = null)
    {
        Debug.Log("들어옵니까4");
        if (target == null)     return;

        Debug.Log("들어옵니까3");
        if (summons) {
            Debug.Log("들어옵니까1");
            foreach (var pos in summonPos) {
                Debug.Log("들어옵니까2");
                GameObject.Instantiate<GameObject>(summons, pos.position, Quaternion.identity);
            }
        }
    }
}
