using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent)), RequireComponent(typeof(CharacterController)), RequireComponent(typeof(Animator))]
public class PlayerCharacter : MonoBehaviour, IDamageable
{
    #region Variables

    CharacterController controller;
    [SerializeField]
    LayerMask groundLayerMask;

    NavMeshAgent agent;
    Camera camera;

    [SerializeField]
    private Animator animator;

    readonly int moveHash = Animator.StringToHash("Move");
    readonly int fallingHash = Animator.StringToHash("Falling");
    readonly int hitHash = Animator.StringToHash("Hit");
    readonly int isAliveHash = Animator.StringToHash("IsAlive");

    public float maxHealth = 100f;
    protected float health;

    [SerializeField]
    Transform hitPoint;
    #endregion


    #region Main Methods
    void Start()
    {
        controller = GetComponent<CharacterController>();

        agent = GetComponent<NavMeshAgent>();
        agent.updatePosition = false;
        agent.updateRotation = true;

        camera = Camera.main;

        health = maxHealth;
    }

    void Update()
    {  
        // 클릭 앤 무브
        // ** mouse left button **
        if (Input.GetMouseButtonDown(0)) {
            // screen to world 좌표
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100, groundLayerMask)) {
                agent.SetDestination(hit.point);
            }
        }

        if (agent.remainingDistance > agent.stoppingDistance) {
            controller.Move(agent.velocity * Time.deltaTime);
            animator.SetBool(moveHash, true);
        } else {
            controller.Move(Vector3.zero);
            animator.SetBool(moveHash, false);
        }
    }

    void OnAnimatorMove()
    {
        Vector3 position = agent.nextPosition;
        animator.rootPosition = agent.nextPosition;
        transform.position = position;
    }
    #endregion Main Methods


    #region IDamageable
    public bool IsAlive => (health > 0);

    public void TakeDamage(int damage, GameObject damageEffectPrefabs)
    {
        if (!IsAlive)   return;

        health -= damage;

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