using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController_Chest : EnemyController, IAttackable, IDamageable, IInteractable
{
#region Variables
    [SerializeField]
    float distance = 5f;
    public float Distance => distance;
    bool isInteractable;
    public bool IsInteractable => isInteractable;

    [SerializeField]
    int monsterID = -1;

    public override float AttackRange => CurrentAttackBehaviour?.range ?? 3f;
    [SerializeField]
    List<AttackBehaviour> attackBehaviours = new List<AttackBehaviour>();

    public Transform projectileTransform;
    public Transform hitTransform;

    public int maxHelth = 100;
    public int health;
    [SerializeField]
    NPCBattleUI battleUI;

    bool isAwake = false;

    int hitTriggerHash = Animator.StringToHash("Hit");
    int AwakeHash = Animator.StringToHash("Awake");

    public override bool IsAvailableAttack  
    {
        get {
            if (!Target)    return false;
            float distance = Vector3.Distance(transform.position, Target.position);
            return (distance <= AttackRange);
        }
    }   // 공격 거리가 되는지 확인

#endregion Variables


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
        battleUI.gameObject.SetActive(false);

        isInteractable = true;
    }

    protected override void Update()
    {
        if (!isAwake)   return;

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
        if (CurrentAttackBehaviour == null || !CurrentAttackBehaviour.IsAvailable) {    // 공격하고 있지 않은 상황이면
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


#region IInteractable
    public void Interact(GameObject other)
    {
        if (isAwake) return;

        float calcDistance = Vector3.Distance(transform.position, other.transform.position);
        if (calcDistance > distance)    return;

        isAwake = true;
        animator?.SetBool(AwakeHash, true);

        battleUI.gameObject.SetActive(true);
        isInteractable = false;
    }

    public void StopInteract(GameObject other)
    {

    }
#endregion IInteractable


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
            CurrentAttackBehaviour.calcCoolTime = 0;
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

        //if (IsAlive) {
        if (health > 0) {
            animator?.SetTrigger(hitTriggerHash);
        } else {
            if (battleUI != null) {
                battleUI.enabled = false;
            }

            stateMachine.ChangeState<DeadState>();
            //Instantiate(dropItem, transform);

            QuestManager.Instance.ProcessQuest(QuestType.DestroyEnemy, monsterID);    
        }
    }
#endregion IDamageable
}
