using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInstances : MonoBehaviour
{
    public List<Transform> itemTransforms = new List<Transform>();

    public void Destroy() {
        foreach (Transform item in itemTransforms) {
            GameObject.Destroy(item.gameObject);
        }
    }
}
