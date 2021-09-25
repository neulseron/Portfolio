using UnityEngine;

public class QuestNPC : MonoBehaviour, IInteractable
{
#region Variables
    [SerializeField]
    float distance = 2f;
    public float Distance => distance;
    bool isInteractable = true;
    public bool IsInteractable => isInteractable;

    public QuestObject questObject;

    public GameObject[] questMarkers;

    public Dialogue readyDialogue;
    public Dialogue acceptedDialogue;
    public Dialogue completedDialogue;

    bool isStartQuestDialogue = false;
    GameObject interactGO = null;
#endregion Variables


#region Unity Methods
    void Start()
    {
        QuestManager.Instance.OnCompletedQuest += OnCompletedQuest;
    }

    void Update() {
        SetMarkers(questObject, questObject.status);
    }
#endregion Unity Methods


#region Methods
    public void Interact(GameObject other)
    {
        float calcDistance = Vector3.Distance(other.transform.position, transform.position) - 0.2f;
        if (calcDistance > distance)    return;
        if (isStartQuestDialogue)    return;

        interactGO = other;
        DialogueManager.Instance.OnEndDialogue += OnEndDialogue;
        isStartQuestDialogue = true;

        if (questObject.status == QuestStatus.None) {   // ** 1. 아직 수락하지 않은 상태
            DialogueManager.Instance.StartDialogue(readyDialogue);
            questObject.status = QuestStatus.Accepted;
        } else if (questObject.status == QuestStatus.Accepted) {    // ** 2. 수락한 상태
            DialogueManager.Instance.StartDialogue(acceptedDialogue);
        } else if (questObject.status == QuestStatus.Completed) {   // ** 3. 완료한 상태
            DialogueManager.Instance.StartDialogue(completedDialogue);

            // ** 보상
            questObject.status = QuestStatus.Rewarded;
            if (questObject.data.rewardItemId != -1) {
                Item item = new Item();
                item.id = questObject.data.rewardItemId;
                GameManager.Instance.Inven.AddItem(item, 1);
            }
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
    }

    public void SetMarkers(QuestObject quest, QuestStatus status)
    {
        switch (status) {
            case QuestStatus.Accepted:
                questMarkers[0].SetActive(false);
                questMarkers[1].SetActive(true);
                questMarkers[2].SetActive(false);
                break;
            case QuestStatus.Completed:
                questMarkers[0].SetActive(false);
                questMarkers[1].SetActive(false);
                questMarkers[2].SetActive(true);
                break;
            case QuestStatus.Rewarded:
                questMarkers[0].SetActive(false);
                questMarkers[1].SetActive(false);
                questMarkers[2].SetActive(false);
                break;
        }
    }
#endregion Methods
}
