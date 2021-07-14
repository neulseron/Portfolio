﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackBehaviour : AttackBehaviour
{
    public ManualCollision attackCollision;

    public override void ExcuteAttack(GameObject target = null, Transform startPoint = null)
    {
        Debug.Log("Melee");
        Collider[] colliders = attackCollision?.ChkOverlapBox(targetMask);

        foreach (Collider col in colliders) {    // 광역 공격
            col.gameObject.GetComponent<IDamageable>()?.TakeDamage(damage, effectPrefab);
        }
    }
}
