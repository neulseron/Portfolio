using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickPointer : MonoBehaviour
{
    public float surfaceOffset = 1f;
    public Transform target = null;

    private void Update() {
        if (target) {
            transform.position = target.position + (Vector3.up * surfaceOffset);
        }
    }

    public void SetPos(RaycastHit hit)
    {
        target = null;
        transform.position = hit.point + (hit.normal * surfaceOffset);
    }
}
