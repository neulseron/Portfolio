using UnityEngine;

public class SummonAttackBehaviour : AttackBehaviour
{
    public Transform[] summonPos;
    public GameObject summons;

    public override void ExcuteAttack(GameObject target = null, Transform startPoint = null)
    {
        if (target == null)     return;

        if (summons) {
            foreach (var pos in summonPos) {
                GameObject.Instantiate<GameObject>(summons, pos.position, Quaternion.identity);
            }
        }
    }
}
