using UnityEngine;

public abstract class AttackBehaviour : MonoBehaviour
{
#region Variables
    public int animationIndex;
    public int priority;    // 높은 것 부터
    public int damage = 10;
    public int addDamage = 0;
    public float range = 3f;

    [SerializeField]
    protected float coolTime;
    public float calcCoolTime = 0f;

    [SerializeField]
    public bool IsAvailable => (calcCoolTime >= coolTime);

    public GameObject effectPrefab;

    [HideInInspector]
    public LayerMask targetMask;
#endregion Variables


#region Methods
    void Start()
    {
        calcCoolTime = 0;
    }

    void Update()
    {   
        if (calcCoolTime < coolTime) {
            calcCoolTime += Time.deltaTime;
        }
    }

    public abstract void ExcuteAttack(GameObject target = null, Transform startPoint = null);
#endregion Methods
}
