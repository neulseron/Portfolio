﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackable
{
    AttackBehaviour CurrentAttackBehaviour
    {
        get;
    }

    void OnExcuteAttack(int attackIndex);
}
