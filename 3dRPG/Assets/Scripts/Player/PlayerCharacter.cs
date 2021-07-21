using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(NavMeshAgent)), RequireComponent(typeof(CharacterController)), RequireComponent(typeof(Animator))]
public class PlayerCharacter : MonoBehaviour, IDamageable, IAttackable
{
    #region Variables
    [SerializeField]
    InventoryObject equipment;
    [SerializeField]
    InventoryObject inventory;

    public ClickPointer clicker;
    Vector3 clickerInitPos = new Vector3(0, -100, 0);
    CharacterController controller;
    [SerializeField]
    LayerMask groundLayerMask;
    NavMeshAgent agent;
    Camera cm;
    [SerializeField]
    Animator animator;

    readonly int moveHash = Animator.StringToHash("Move");
    readonly int moveSpeedHash = Animator.StringToHash("MoveSpeed");
    readonly int attackHash = Animator.StringToHash("Attack");
    readonly int attackIndexHash = Animator.StringToHash("AttackIndex");
    readonly int hitHash = Animator.StringToHash("Hit");
    readonly int isAliveHash = Animator.StringToHash("IsAlive");

    public bool IsInAttackState => GetComponent<AttackStateController>()?.IsInAttackState ?? false;
    public Transform target;
    [SerializeField]
    LayerMask targetMask;

    public float maxHealth = 100f;
    protected float health;

    [SerializeField]
    Transform hitPoint;

    [SerializeField]
    public StatsObject playerStats;
    #endregion


    #region Main Methods
    void Start()
    {
        controller = GetComponent<CharacterController>();
        inventory.OnUseItem += OnUseItem;

        agent = GetComponent<NavMeshAgent>();
        agent.updatePosition = false;
        agent.updateRotation = true;

        cm = Camera.main;

        //health = maxHealth;

        InitAttackBehaviour();
    }

    void Update()
    { 
        if (!IsAlive)   return;
        
        ChkAttackBehaviour();

        bool isOnUI = EventSystem.current.IsPointerOverGameObject();

        if (Input.GetKeyDown(KeyCode.I)) {
        }

        // 클릭 앤 무브
        // ** mouse left button **
        if (!isOnUI && Input.GetMouseButtonDown(0) && !IsInAttackState) {  // 지면의 한 지점 클릭
            Ray ray = cm.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100, groundLayerMask)) {
                RemoveTarget();

                agent.SetDestination(hit.point);

                SetClicker(hit.point);
            }
        } else if (!isOnUI && Input.GetMouseButtonDown(1)){    // 우클릭
            Ray ray = cm.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100)) {

                IDamageable damageable = hit.collider.GetComponent<IDamageable>();  // 상대가 데미지 받는지
                if (damageable != null && damageable.IsAlive) {
                    SetTarget(hit.collider.transform, CurrentAttackBehaviour != null ? CurrentAttackBehaviour.range : 0.02f);

                    SetClicker(hit.collider.transform.position);
                }

                IInteractable interactable = hit.collider.GetComponent<IInteractable>(); 
                if (interactable != null) {
                    SetTarget(hit.collider.transform, interactable.Distance);
                }
            }
        }

        if (target != null) {
            if (target.GetComponent<IInteractable>() != null) {
                float calcDistance = Vector3.Distance(target.position, transform.position);
                float range = target.GetComponent<IInteractable>().Distance;
                if (calcDistance > range) {
                    SetTarget(target, range);
                }

                FaceToTarget();

                //IInteractable interactable = target.GetComponent<IInteractable>();
                //interactable.Interact(this.gameObject);
            } else if (!(target.GetComponent<IDamageable>()?.IsAlive ?? false)) {   
                RemoveTarget();
            } else {    // 아직 타겟(적)으로 이동중일때?
                float calcDistance = Vector3.Distance(target.position, transform.position);
                float range = CurrentAttackBehaviour?.range ?? 1f;
                if (calcDistance > range) {
                    SetTarget(target, range);
                }

                FaceToTarget();

                //agent.SetDestination(target.position);
                //FaceToTarget();
            }
        }

        if (agent.pathPending || agent.remainingDistance > agent.stoppingDistance) {
            controller.Move(agent.velocity * Time.deltaTime);
            animator.SetFloat(moveSpeedHash, agent.velocity.magnitude / agent.speed, .1f, Time.deltaTime);
            animator.SetBool(moveHash, true);
        } else {
            controller.Move(Vector3.zero);

            animator.SetFloat(moveSpeedHash, 0);
            animator.SetBool(moveHash, false);

            if (target !=  null) {
                if (target.GetComponent<IInteractable>() != null)
                {
                    IInteractable interactable = target.GetComponent<IInteractable>();
                    interactable.Interact(this.gameObject);
                    Invoke("RemoveTarget", 1f);
                }
                else if (target.GetComponent<IDamageable>() != null)
                {
                    AttackTarget();
                }
            } else {
                RemoveClicker();
            }
        }

        //AttackTarget();
    }

    void OnAnimatorMove()
    {
        Vector3 position = agent.nextPosition;
        animator.rootPosition = agent.nextPosition;
        transform.position = position;
    }
    #endregion Main Methods


    #region Inventory
    void OnUseItem(ItemObject itemObject)
    {
        foreach(ItemBuff buff in itemObject.data.buffs) {
            if (buff.stat == AttributeType.Health) {
                //this.health += buff.value;
                playerStats.AddHealth(buff.value);
            }
        }
    }

    void OnTriggerEnter(Collider other) {
        var item = other.GetComponent<GroundItem>();
        if (item) {
            if (inventory.AddItem(new Item(item.itemObject), 1)) {
                Destroy(other.gameObject);
            }
        }
    }
    #endregion Inventory


    #region Helper Methods
    public bool PickupItem(ItemObject itemObject, int amount = 1)
    {
        if (itemObject != null) {
            return inventory.AddItem(new Item(itemObject), amount);
        }

        return false;
    }

    void InitAttackBehaviour()
    {
        foreach (AttackBehaviour behaviour in attackBehaviours)
        {
            behaviour.targetMask = targetMask;
        }

        //GetComponent<AttackStateController>().enterAttackHandler += OnEnterAttackState;
        //GetComponent<AttackStateController>().exitAttackHandler += OnExitAttackState;
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

    void SetClicker(Vector3 position)
    {
        if (!clicker)   return;

        Vector3 calcPostion = position;
        calcPostion.y += clicker.surfaceOffset;
        clicker.transform.position = calcPostion;
    }

    void RemoveClicker()
    {
        if (!clicker)   return;

        Vector3 calcPostion = clickerInitPos;
        clicker.transform.position = calcPostion;
    }

    void SetTarget(Transform newTarget, float stoppingDistance)
    {
        //Debug.Log("타겟 이름 : " + newTarget.gameObject.name);
        target = newTarget;

        //ChkAttackBehaviour();
        agent.stoppingDistance = stoppingDistance;
        agent.SetDestination(newTarget.transform.position);
    }

    void RemoveTarget()
    {
        target = null;

        agent.stoppingDistance = 0.02f;
        agent.ResetPath();
    }

    void AttackTarget()
    {
        if (CurrentAttackBehaviour == null)     return;

        //Debug.Log("target : " + target + ", IsInAttackState : " + IsInAttackState + ", IsAvailable : " + CurrentAttackBehaviour.IsAvailable);
        if (target != null && !IsInAttackState && CurrentAttackBehaviour.IsAvailable) {
            float distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance <= CurrentAttackBehaviour?.range) {
                animator.SetInteger(attackIndexHash, CurrentAttackBehaviour.animationIndex);
                animator.SetTrigger(attackHash);
            }
        }
    }

    void FaceToTarget()
    {
        if (target) {
            Vector3 direction = (target.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10.0f);
        }
    }

    public void OnEnterAttackState()
    {
        Debug.Log("enter attack state");
        //playerStats.AddMana(-3);
    }

    public void OnExitAttackState()
    {
        
    }
    #endregion Helper Methods


    #region IAttackable
    [SerializeField]
    List<AttackBehaviour> attackBehaviours = new List<AttackBehaviour>();

    public AttackBehaviour CurrentAttackBehaviour
    {
        get;
        private set;
    }

    public void OnExcuteAttack(int attackIndex)
    {
        if (CurrentAttackBehaviour != null && target != null)
        {
            playerStats.AddMana(-3);
            CurrentAttackBehaviour.ExcuteAttack(target.gameObject);
        }
    }
    #endregion IAttackable


    #region IDamageable
    public bool IsAlive => (playerStats.Health > 0);

    public void TakeDamage(int damage, GameObject damageEffectPrefabs)
    {
        if (!IsAlive)   return;

        //health -= damage;
        playerStats.AddHealth(-damage);

        if (damageEffectPrefabs) {
            Instantiate(damageEffectPrefabs, hitPoint);
        }

        if (IsAlive) {
            animator.SetTrigger(hitHash);
        } else {
            animator.SetBool(isAliveHash, false);
        }
    }
    #endregion IDamageable
}