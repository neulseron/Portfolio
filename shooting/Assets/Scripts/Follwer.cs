using System.Collections.Generic;
using UnityEngine;

public class Follwer : MonoBehaviour
{
#region Variables
    public float maxShotDelay;
    float curShotDelay;

    [SerializeField]
    Transform parent;
    Vector3 followPos;
    Queue<Vector3> parentPos;
    int followDelay;
#endregion Variables


#region Unity Methods
    void Awake() 
    {
        parentPos = new Queue<Vector3>();
        followDelay = 20;
    }

    void Update() 
    {
        Watch();
        Follow();
        Fire();
        Reload();
    }
#endregion Unity Methods


#region Methods
    void Watch()
    {
        if (!parentPos.Contains(parent.position))
            parentPos.Enqueue(parent.position);

        if (parentPos.Count > followDelay)
            followPos = parentPos.Dequeue();
        else if (parentPos.Count < followDelay)
            followPos = parent.position;
    }

    void Follow() => transform.position = followPos; 

    void Fire()
    {
        if (curShotDelay < maxShotDelay)
            return;

        GameObject bullet = ObjectManager.Instance.MakeObj("FollwerBullet");
        bullet.transform.position = transform.position;

        Rigidbody2D bulletRigid = bullet.GetComponent<Rigidbody2D>();
        bulletRigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse); 

        curShotDelay = 0;
    }

    void Reload() => curShotDelay += Time.deltaTime; 
#endregion Methods
}
