using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAttackBehaviour : AttackBehaviour
{
    public override void ExcuteAttack(GameObject target = null, Transform startPoint = null)
    {
        Debug.Log("Projectile");
        if (target == null)     return;

        Vector3 projectilePosition = startPoint != null ? startPoint.position : transform.position;
        if (effectPrefab) {
            GameObject projectileGO = GameObject.Instantiate<GameObject>(effectPrefab, projectilePosition, Quaternion.identity);
            projectileGO.transform.forward = transform.forward;

            Projectile projectile = projectileGO.GetComponent<Projectile>();
            if (projectile) {
                //projectile.owner = this.gameObject;
                projectile.target = target;
                projectile.attackBehaviour = this;
            }
        }

        calcCoolTime = 0f;
    }
}
