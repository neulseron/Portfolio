using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController_Mouse : EnemyController, IAttackable, IDamageable
{
    #region Variables
    public override float AttackRange => CurrentAttackBehaviour?.range ?? 8.0f;

    public Transform projectileTransform;
    public Transform hitTransform;

    public int maxHelth = 100;
    public int health;
    [SerializeField]
    NPCBattleUI battleUI;

    int hitTriggerHash = Animator.StringToHash("Hit");

    [SerializeField]
    List<AttackBehaviour> attackBehaviours = new List<AttackBehaviour>();
    #endregion Variables


    #region Properties
    public override bool IsAvailableAttack  
    {
        get {
            if (!Target)    return false;
            float distance = Vector3.Distance(transform.position, Target.position);
            return (distance <= AttackRange);
        }
    }   // 공격 거리가 되는지 확인
    #endregion Properties


    #region Unity Methods
    protected override void Start()
    {
        base.Start();

        stateMachine.AddState(new MoveState());
        stateMachine.AddState(new AttackState());
        stateMachine.AddState(new DeadState());

        InitAttackBehaviour();

        health = maxHelth;
        if (battleUI) {
            battleUI.MinValue = 0f;
            battleUI.MaxValue = maxHelth;
            battleUI.Value = health;
        }
    }

    protected override void Update()
    {
        ChkAttackBehaviour();

        base.Update();
    }
    #endregion Unity Methods


    #region Helper Methods
    void InitAttackBehaviour()
    {
        foreach (AttackBehaviour behaviour in attackBehaviours) {
            if (CurrentAttackBehaviour == null) {
                CurrentAttackBehaviour = behaviour;
            }

            behaviour.targetMask = TargetMask;
        }
    }

    void ChkAttackBehaviour()
    {
        if (CurrentAttackBehaviour == null || !CurrentAttackBehaviour.IsAvailable) {
            CurrentAttackBehaviour = null;

            foreach (AttackBehaviour behaviour in attackBehaviours) {
                if (behaviour.IsAvailable) {
                    if (CurrentAttackBehaviour == null || (CurrentAttackBehaviour.priority < behaviour.priority)) {
                        CurrentAttackBehaviour = behaviour;
                    }
                }
            }
        }
    }
    #endregion Helper Methods


    #region Other Methods
    #endregion Other Methods


    #region IAttackable
    public AttackBehaviour CurrentAttackBehaviour
    {
        get;
        private set;
    }

    public void OnExcuteAttack(int attackIndex)
    {
        if (CurrentAttackBehaviour != null && Target != null) {
            CurrentAttackBehaviour.ExcuteAttack(Target.gameObject, projectileTransform);
            CurrentAttackBehaviour = null;
        }
    }
    #endregion IAttackable


    #region IDamageable
    public bool IsAlive => (health > 0);

    public void TakeDamage(int damage, GameObject hitEffectPrefabs)
    {
        if (!IsAlive)   return;

        health -= damage;
        if (battleUI) {
            battleUI.Value = health;
            battleUI.CreateDmgTxt(damage);
        }

        if (hitEffectPrefabs) {
            Instantiate(hitEffectPrefabs, hitTransform);
        }

        if (IsAlive) {
            animator?.SetTrigger(hitTriggerHash);
        } else {
            if (battleUI != null) {
                battleUI.enabled = false;
            }

            stateMachine.ChangeState<DeadState>();

            QuestManager.Instance.ProcessQuest(QuestType.DestroyEnemy, 0);    
        }
    }
    #endregion IDamageable
}
