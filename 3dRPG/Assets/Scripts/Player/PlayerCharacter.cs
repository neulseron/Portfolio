using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(NavMeshAgent)), RequireComponent(typeof(CharacterController)), RequireComponent(typeof(Animator))]
public class PlayerCharacter : MonoBehaviour, IDamageable, IAttackable
{
#region Variables

    #region Inventory & Equipment
    [Header("Inventory & Equipment")]
    [SerializeField]
    InventoryObject equipment;
    [SerializeField]
    InventoryObject inventory;
    [SerializeField]
    InventoryObject gemSlot;
    public InventoryObject Inven => inventory;
    public List<int> hadGetList = new List<int>();
    #endregion Inventory & Equipment

    #region Components
    [Header("Components")]
    [SerializeField]
    Animator animator;
    public Animator anim => animator;
    CharacterController controller;
    NavMeshAgent agent;
    public NavMeshAgent Agent => agent;
    Camera cm;
    #endregion Components

    #region Clicker
    public ClickPointer clicker;
    Vector3 clickerInitPos = new Vector3(0, -100, 0);
    [SerializeField]
    LayerMask groundLayerMask;
    #endregion Clicker

    #region Animator Hash
    readonly int moveHash = Animator.StringToHash("Move");
    readonly int moveSpeedHash = Animator.StringToHash("MoveSpeed");
    readonly int attackHash = Animator.StringToHash("Attack");
    readonly int attackIndexHash = Animator.StringToHash("AttackIndex");
    readonly int hitHash = Animator.StringToHash("Hit");
    readonly int isAliveHash = Animator.StringToHash("IsAlive");
    #endregion Animator Hash

    #region Attack
    public bool IsInAttackState => GetComponent<AttackStateController>()?.IsInAttackState ?? false;
    public Transform target;
    [SerializeField]
    LayerMask targetMask;
    [SerializeField]
    Transform hitPoint;
    #endregion Attack

    #region UI
    [SerializeField]
    public StatsObject playerStats;
    #endregion UI
    
#endregion Variables


#region Unity Methods
    private void Awake() {
        InitAttackBehaviour();
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();

        inventory.OnUseItem += OnUseItem;
        equipment.OnUseItem += OnUseItem;
        equipment.OnRemoveItem += OnRemoveItem;

        agent = GetComponent<NavMeshAgent>();
        agent.updatePosition = false;
        agent.updateRotation = true;

        cm = Camera.main;
    }

    void Update()
    { 
        if (!IsAlive)   return;
        
        ChkAttackBehaviour();

        bool isOnUI = EventSystem.current.IsPointerOverGameObject();

        // *** 타겟 설정 ***
        // ** 1. 캐릭터 이동 (마우스 좌클릭 위치까지 이동)
        if (!isOnUI && Input.GetMouseButtonDown(0)/* && !IsInAttackState*/) {
            Ray ray = cm.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100, groundLayerMask)) {
                RemoveTarget();
                agent.SetDestination(hit.point);
                SetClicker(hit.point);
            }
        // ** 2. 상호작용 (마우스 우클릭)
        } else if (!isOnUI && Input.GetMouseButtonDown(1)){
            Ray ray = cm.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            int wallUnlayerMask = (1 << LayerMask.NameToLayer("Wall"));   wallUnlayerMask = ~wallUnlayerMask;

            if (Physics.Raycast(ray, out hit, 100, wallUnlayerMask)) {
                // 2-1. 데미지 받는 오브젝트 (몬스터)
                IDamageable damageable = hit.collider.GetComponent<IDamageable>();
                if (damageable != null && damageable.IsAlive) {
                    SetTarget(hit.collider.transform, CurrentAttackBehaviour != null ? CurrentAttackBehaviour.range : 0.02f);

                    SetClicker(hit.collider.transform.position);
                }

                // 2-2. 기타 상호작용 오브젝트 (아이템, NPC)
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                Debug.Log("상호작용 대상 : " + hit.collider.gameObject.name); 
                if (interactable != null && interactable.IsInteractable) {
                    SetTarget(hit.collider.transform, interactable.Distance);
                }
            }
        }

        // *** 타겟 상태 확인 ***
        if (target != null) {
            // ** 1. 오브젝트와 상호작용 가능한 거리까지 접근했는지
            if (target.GetComponent<IInteractable>() != null && target.GetComponent<IInteractable>().IsInteractable) {
                float calcDistance = Vector3.Distance(target.position, transform.position);
                float range = target.GetComponent<IInteractable>().Distance;
                if (calcDistance > range) { // 타겟과 상호작용 가능한 거리보다 더 남았으면
                    Debug.Log("거리 부족"); 
                    SetTarget(target, range);
                }

                FaceToTarget();
            // ** 2. 몬스터가 살아있지 않으면 타겟 제거
            } else if (!(target.GetComponent<IDamageable>()?.IsAlive ?? false)) {   
                RemoveTarget();
            // ** 3. 타겟 몬스터가 아직 살아있으면
            } else { 
                float calcDistance = Vector3.Distance(target.position, transform.position);
                float range = CurrentAttackBehaviour?.range ?? 1f;
                if (calcDistance > range) {
                    SetTarget(target, range);
                }

                FaceToTarget();
            }
        }

        // *** 경로 탐색 ***
        // ** 1. 탐색할 경로가 남은 경우
        if (agent.pathPending || agent.remainingDistance > agent.stoppingDistance) {
            Debug.Log("목적지까지 이동 중"); 
            controller.Move(agent.velocity * Time.deltaTime);
            animator.SetFloat(moveSpeedHash, agent.velocity.magnitude / agent.speed, .1f, Time.deltaTime);
            animator.SetBool(moveHash, true);
        // ** 2. 탐색을 종료한 경우 (타겟 혹은 목적지에 도착한 경우)
        } else {
            controller.Move(Vector3.zero);

            animator.SetFloat(moveSpeedHash, 0);
            animator.SetBool(moveHash, false);
            
            // ** 2-1. 타겟에 도착한 경우
            if (target !=  null) {
                // ** 2-1-1. 상호작용 가능한 오브젝트
                if (target.GetComponent<IInteractable>() != null && target.GetComponent<IInteractable>().IsInteractable)
                {
                    IInteractable interactable = target.GetComponent<IInteractable>();
                    interactable.Interact(this.gameObject);
                    Invoke("RemoveTarget", 1f);
                // ** 2-1-2. 몬스터
                } else if (target.GetComponent<IDamageable>() != null) {
                    AttackTarget();
                }
            // ** 2-2. 목적지에 도착한 경우
            } else {
                RemoveClicker();
            }
        }
    }

    void OnAnimatorMove()
    {
        Vector3 position = agent.nextPosition;
        animator.rootPosition = agent.nextPosition;
        transform.position = position;
    }
    
#endregion Unity Methods


#region Inventory Methods
    void OnUseItem(ItemObject itemObject)
    {
        foreach(ItemBuff buff in itemObject.data.buffs) {
            if (buff.stat == AttributeType.Health) {
                playerStats.AddHealth(buff.value);
            }
        }
    }
    
    void OnRemoveItem(ItemObject itemObject)
    {
        foreach(ItemBuff buff in itemObject.data.buffs) {
            if (buff.stat == AttributeType.MaxHealth) {
                playerStats.RemoveMaxHealth(buff.value);
            }
        }
    }

    public bool PickupItem(ItemObject itemObject, int amount = 1)
    {
        if (itemObject != null) {
            if (itemObject.type == ItemType.Gem) {
                return gemSlot.AddGem(new Item(itemObject));
            }

            if (itemObject.type == ItemType.Key || itemObject.type == ItemType.LeftWeapon || itemObject.type == ItemType.RightWeapon) {
                hadGetList.Add(itemObject.data.id);
            }
            
            return inventory.AddItem(new Item(itemObject), amount);
        } 

        return false;
    }

    public bool HavingItem(int itemId)
    {
        return inventory.Container.IsContain(itemId) || gemSlot.Container.IsContain(itemId);
    }

    public bool HadItem(int itemId)
    {
        return hadGetList.Contains(itemId);
    }
#endregion Inventory Methods


#region Helper Methods
    void InitAttackBehaviour()
    {
        foreach (AttackBehaviour behaviour in attackBehaviours)
        {
            if (CurrentAttackBehaviour == null) {
                CurrentAttackBehaviour = behaviour;
            }

            behaviour.targetMask = targetMask;
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
        target = newTarget;

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
            CurrentAttackBehaviour.ExcuteAttack(target.gameObject);
            CurrentAttackBehaviour.calcCoolTime = 0;
            CurrentAttackBehaviour = null;
        }
    }
#endregion IAttackable


#region IDamageable
    public bool IsAlive => (playerStats.Health > 0);

    public void TakeDamage(int damage, GameObject damageEffectPrefabs)
    {
        if (!IsAlive)   return;

        playerStats.AddHealth(-damage);

        // ** 이펙트 효과 
        if (damageEffectPrefabs) {
            Instantiate(damageEffectPrefabs, hitPoint);
        }

        if (playerStats.Health > 0) {
            animator.SetTrigger(hitHash);
        } else {
            animator.SetBool(isAliveHash, false);
        }
    }
#endregion IDamageable
}