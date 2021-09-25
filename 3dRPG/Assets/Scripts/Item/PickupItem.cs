using UnityEngine;

public class PickupItem : MonoBehaviour, IInteractable
{
#region Variables
    public float distance = 5f;
    public float Distance => distance;
    bool isInteractable = true;
    public bool IsInteractable => isInteractable;

    public ItemObject itemObject;

    [Header("Dialogue")]
    public Dialogue getItemDialogue;
    GameObject interactGO = null;
#endregion Variables


#region Methods
    void Update() {
        if (GameManager.Instance.Data != null && GameManager.Instance.Data.chkGetItem.Contains(itemObject.data.id))
            Destroy(gameObject);
    }

    public void Interact(GameObject other)
    {
        float calcDistance = Vector3.Distance(transform.position, other.transform.position);
        if (calcDistance - 0.6f > distance)    return;

        interactGO = other;
        DialogueManager.Instance.OnEndDialogue += OnEndDialogue;

        PlayerCharacter playerCharacter = other.GetComponent<PlayerCharacter>();
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
#endregion Methods
}
