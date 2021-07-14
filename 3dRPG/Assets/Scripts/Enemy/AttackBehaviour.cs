using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackBehaviour : MonoBehaviour
{
#if UNITY_EDITOR
[Multiline]
public string developDescription = "";
#endif
    #region Variables
    public int animationIndex;
    public int priority;
    public int damage = 10;
    public float range = 3f;

    [SerializeField]
    protected float coolTime;
    protected float calcCoolTime = 0f;

    [SerializeField]
    public bool IsAvailable => (calcCoolTime >= coolTime);

    public GameObject effectPrefab;

    [HideInInspector]
    public LayerMask targetMask;
    #endregion Variables


    void Start()
    {
        calcCoolTime = coolTime;
    }

    void Update()
    {
        if (calcCoolTime < coolTime) {
            calcCoolTime += Time.deltaTime;
        }
    }

    public abstract void ExcuteAttack(GameObject target = null, Transform startPoint = null);
}
