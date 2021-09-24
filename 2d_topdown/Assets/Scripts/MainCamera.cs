using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public Transform target;
    Vector3 offset = new Vector3(0, -0.8f, -10);
    public float speed;
    //-----------------
    public bool isMoving = false;
    Transform target2;
    //-----------------
    public bool isAutoMoving = false;
    public Vector3[] wayPoint;
    int waypointsIndex = 0;


    private void FixedUpdate() {
        if (isAutoMoving) {
            AutoMovingEx();
        } else if (isMoving) {
            CameraMovingEx();
        } else {
            Vector3 targetPos = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetPos, speed);
        }
    }

    public void CameraMoving(Transform _target2)
    {
        isMoving = true;
        target2 = _target2;
    }

    void CameraMovingEx()
    {
        if (Vector3.Distance(transform.position, target2.position) <= 10.01f)
            isMoving = false;
            
        Vector3 targetPos = new Vector3(target2.position.x, target2.position.y, -10);
        transform.position = Vector3.Lerp(transform.position, targetPos, 0.2f);
    }

    public void AutoMoving(int _size, params Vector3[] _vec)
    {
        isAutoMoving = true;
        wayPoint = new Vector3[_size];

        for (int i = 0; i < _size; i++)
            wayPoint[i] = _vec[i];
    }

    void AutoMovingEx()
    {
        if (waypointsIndex < wayPoint.Length) {
            Vector3 nextPos = new Vector3(wayPoint[waypointsIndex].x, wayPoint[waypointsIndex].y, -10);
            transform.position = Vector3.Lerp(transform.position, nextPos, 0.005f);
            //float step = 2f * Time.smoothDeltaTime;
            //transform.position = Vector3.MoveTowards(transform.position, nextPos, step);

            Debug.Log(Vector3.Distance(transform.position, nextPos));
            Debug.Log(waypointsIndex);
            if (Vector3.Distance(transform.position, nextPos) <= 1.1f)
                waypointsIndex++;
        } else {
            isAutoMoving = false;
            waypointsIndex = 0;
        }
    }
}
