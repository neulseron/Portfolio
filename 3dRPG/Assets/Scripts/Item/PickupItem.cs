using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour, IInteractable
{
    public float distance = 5f;
    public float Distance => distance;

    public ItemObject itemObject;

    public void Interact(GameObject other)
    {
        float calcDistance = Vector3.Distance(transform.position, other.transform.position);
        //Debug.Log("떨어진 거리 : " + calcDistance + ", 가능 거리 : " + distance);
        if (calcDistance > distance)    return;

        PlayerCharacter playerCharacter = other.GetComponent<PlayerCharacter>();
        if (playerCharacter?.PickupItem(itemObject) ?? false) {
            Destroy(gameObject);
        }
    }

    public void StopInteract(GameObject other)
    {
    }

    void OnValidate() {
    #if UNITY_EDITOR
        GetComponent<SpriteRenderer>().sprite = itemObject.icon;
    #endif
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distance);
    }
}
