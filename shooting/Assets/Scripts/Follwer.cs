using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follwer : MonoBehaviour
{
    public ObjectManager objectManager;
    public Transform parent;
    //---------------------
    public float maxShotDelay;
    float curShotDelay;
    int followDelay;

    Vector3 followPos;
    Queue<Vector3> parentPos;
    //=====================
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
    //---------------------
    void Watch()
    {
        if (!parentPos.Contains(parent.position))
            parentPos.Enqueue(parent.position);

        if (parentPos.Count > followDelay)
            followPos = parentPos.Dequeue();
        else if (parentPos.Count < followDelay)
            followPos = parent.position;
    }

    void Follow()
    {
        transform.position = followPos;
    }

    void Fire()
    {
        //if (!Input.GetButton("Fire1"))
            //return;

        if (curShotDelay < maxShotDelay)
            return;

        GameObject bullet = objectManager.MakeObj("FollwerBullet");
        bullet.transform.position = transform.position;

        Rigidbody2D bulletRigid = bullet.GetComponent<Rigidbody2D>();
        bulletRigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse); 

        curShotDelay = 0;
    }

    void Reload()
    {
        curShotDelay += Time.deltaTime;
    }
}
