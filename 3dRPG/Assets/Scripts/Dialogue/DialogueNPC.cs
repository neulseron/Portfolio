using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueNPC : MonoBehaviour, IInteractable
{
    [SerializeField]
    Dialogue dialogue;

    bool isStartDialogue = false;

    GameObject interactGO;


    [SerializeField]
    float distance = 2f;
    public float Distance => distance;


    public void Interact(GameObject other)
    {
        float calcDistance = Vector3.Distance(other.transform.position, transform.position) - 0.2f;
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
}
