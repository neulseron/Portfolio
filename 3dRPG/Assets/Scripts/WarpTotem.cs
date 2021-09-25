using UnityEngine;

public class WarpTotem : MonoBehaviour, IInteractable
{   
#region Variables
    [SerializeField]
    float distance = 2f;
    public float Distance => distance;
    bool isInteractable = true;
    public bool IsInteractable => isInteractable;

    [SerializeField]
    int[] needItemId;    

    public Dialogue notEnoughDialogue;
    public Dialogue enoughDialogue;

    bool isStartDialogue = false;
    GameObject interactGO = null;

    bool isReady = false;
    public Transform warpPos;

    [SerializeField]
    GameObject BossHP_UI;
#endregion Variables


#region Methods
    public void Interact(GameObject other)
    {
        float calcDistance = Vector3.Distance(other.transform.position, transform.position) - 0.2f;
        if (calcDistance > distance)    return;
        if (isStartDialogue)    return;

        interactGO = other;
        DialogueManager.Instance.OnEndDialogue += OnEndDialogue;
        isStartDialogue = true;

        // ** 아이템 조건
        int nowCnt = 0;
        PlayerCharacter playerCharacter = other.GetComponent<PlayerCharacter>();
        foreach (int id in needItemId) {
            if (!playerCharacter.HavingItem(id)) {
                DialogueManager.Instance.StartDialogue(notEnoughDialogue);
                isReady = false;
                return;
            }

            nowCnt++;

            if (nowCnt >= needItemId.Length-1) {
                isReady = true;
                DialogueManager.Instance.StartDialogue(enoughDialogue);
                return;
            } else {
                isReady = false;
            }
        }
    }

    public void StopInteract(GameObject other)
    {
        isStartDialogue = false;

        if (isReady) {
            isReady = false;
            MapManager.Instance.OnMesh();
            GameManager.Instance.SyncPosition(warpPos.position);
        }

        if (BossHP_UI) {
            BossHP_UI.SetActive(true);
        }
    }

    void OnEndDialogue()
    {
        StopInteract(interactGO);
    }
#endregion Methods
}
