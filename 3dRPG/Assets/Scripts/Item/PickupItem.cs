using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour, IInteractable
{
    public float distance = 5f;
    public float Distance => distance;
    bool isInteractable = true;
    public bool IsInteractable => isInteractable;

    public ItemObject itemObject;

    [Header("Dialogue")]
    public Dialogue getItemDialogue;
    GameObject interactGO = null;


    void Update() {
        if (GameManager.Instance.Data != null && GameManager.Instance.Data.chkGetItem.Contains(itemObject.data.id))
            Destroy(gameObject);
    }

    public void Interact(GameObject other)
    {
        Debug.Log("인터랙트 입니다");
        float calcDistance = Vector3.Distance(transform.position, other.transform.position);
        Debug.Log("현재 거리 :" + calcDistance + ", 가능 거리 : " + distance);
        if (calcDistance - 0.6f > distance)    return;

        interactGO = other;
        DialogueManager.Instance.OnEndDialogue += OnEndDialogue;

        PlayerCharacter playerCharacter = other.GetComponent<PlayerCharacter>();
        //if (playerCharacter?.PickupItem(itemObject) ?? false) {
        if (playerCharacter?.PickupItem(itemObject, itemObject.amount) ?? false) {
            DialogueManager.Instance.StartDialogue(getItemDialogue);
            Destroy(gameObject);
        }
    }

    public void StopInteract(GameObject other)
    {
    }

    void OnEndDialogue()
    {
        StopInteract(interactGO);
    }

    void OnValidate() {
    #if UNITY_EDITOR
        //GetComponent<SpriteRenderer>().sprite = itemObject.icon;
    #endif
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distance);
    }
}
