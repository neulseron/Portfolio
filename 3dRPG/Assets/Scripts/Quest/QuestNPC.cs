using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestNPC : MonoBehaviour, IInteractable
{
    public QuestObject questObject;

    public Dialogue readyDialogue;
    public Dialogue acceptedDialogue;
    public Dialogue completedDialogue;

    bool isStartQuestDialogue = false;
    GameObject interactGO = null;


    void Start()
    {
        QuestManager.Instance.OnCompletedQuest += OnCompletedQuest;
    }

    public float distance = 2f;
    public float Distance => distance;

    public void Interact(GameObject other)
    {
        float calcDistance = Vector3.Distance(other.transform.position, transform.position) - 0.2f;
        if (calcDistance > distance)    return;
        if (isStartQuestDialogue)    return;

        interactGO = other;
        DialogueManager.Instance.OnEndDialogue += OnEndDialogue;
        isStartQuestDialogue = true;

        if (questObject.status == QuestStatus.None) {
            DialogueManager.Instance.StartDialogue(readyDialogue);
            questObject.status = QuestStatus.Accepted;
        } else if (questObject.status == QuestStatus.Accepted) {
            DialogueManager.Instance.StartDialogue(acceptedDialogue);
        } else if (questObject.status == QuestStatus.Completed) {
            DialogueManager.Instance.StartDialogue(completedDialogue);
            questObject.status = QuestStatus.Completed;

            // ** reward
            questObject.status = QuestStatus.Rewarded;
        }
    }

    public void StopInteract(GameObject other)
    {
        isStartQuestDialogue = false;
    }

    void OnEndDialogue()
    {
        StopInteract(interactGO);
    }


    void OnCompletedQuest(QuestObject questObject)
    {
        if (questObject.data.id == this.questObject.data.id && questObject.status == QuestStatus.Completed) {
            // ** npc effect
        }
    }
}
