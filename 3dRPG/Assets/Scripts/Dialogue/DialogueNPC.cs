using UnityEngine;

public class DialogueNPC : MonoBehaviour, IInteractable
{
#region Variables
    [SerializeField]
    Dialogue dialogue;
    bool isStartDialogue = false;
    GameObject interactGO;


    [SerializeField]
    float distance = 2f;
    public float Distance => distance;

    bool isInteractable = true;
    public bool IsInteractable => isInteractable;
#endregion Variables


#region Methods
    public void Interact(GameObject other)
    {
        float calcDistance = Vector3.Distance(other.transform.position, transform.position);
        if (calcDistance > distance)    return;
        if (isStartDialogue)    return;

        interactGO = other;
        DialogueManager.Instance.OnEndDialogue += OnEndDialogue;
        isStartDialogue = true;

        DialogueManager.Instance.StartDialogue(dialogue);
    }

    public void StopInteract(GameObject other)
    {
        isStartDialogue = false;
    }

    void OnEndDialogue()
    {
        StopInteract(interactGO);
    }
#endregion Methods
}
