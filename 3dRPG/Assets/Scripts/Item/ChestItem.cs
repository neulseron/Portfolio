using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestItem : MonoBehaviour, IInteractable
{
#region Variables
    [SerializeField]
    float distance = 5f;
    public float Distance => distance;
    bool isInteractable = true;
    public bool IsInteractable => isInteractable;

    [SerializeField]
    ItemObject itemObject;
    [SerializeField]
    ItemObject keyObject = null;
    [SerializeField]
    MeshFilter openMesh;
    bool getItem = false;

    GameObject displayObj;
    Animator animator;
    int openHash = Animator.StringToHash("open");

    [Header("Dialogue")]
    public Dialogue needKeyDialogue;
    public Dialogue getItemDialogue;
    public Dialogue afterItemDialogue;
    bool isStartInteract = false;
    GameObject interactGO = null;
#endregion Variables


#region Methods
    void OnEnable() 
    {
        displayObj = transform.Find("object")?.gameObject;
        animator = displayObj?.GetComponent<Animator>();
    }

    public void Interact(GameObject other)
    {
        if (isStartInteract)    return;

        PlayerCharacter playerCharacter = other.GetComponent<PlayerCharacter>();

        // ** 아이템 획득 여부 확인 **
        getItem = playerCharacter.HavingItem(itemObject.data.id);   

        float calcDistance = Vector3.Distance(transform.position, other.transform.position);
        if (calcDistance > distance)    return;

        interactGO = other;
        DialogueManager.Instance.OnEndDialogue += OnEndDialogue;
        isStartInteract = true;
        
        //GetComponent<MeshFilter>().mesh = openMesh.mesh;

        if (getItem) {  // ** 1. 아이템을 인벤토리에 가지고 있을 경우
            DialogueManager.Instance.StartDialogue(afterItemDialogue);
        } else if (keyObject != null) { // ** 2. 열쇠가 필요할 경우
             if (!playerCharacter.HavingItem(keyObject.data.id) && !playerCharacter.HadItem(keyObject.data.id)) {   // * 2-1. 키를 획득한 적 없는 경우
                Debug.Log("키 가진적 없음");
                DialogueManager.Instance.StartDialogue(needKeyDialogue);
            } else if (playerCharacter.HavingItem(keyObject.data.id)) {    // * 2-2. 열쇠 가지고 있는 경우
                if (playerCharacter?.PickupItem(itemObject) ?? false) { 
                    Debug.Log("키 가지고 있음 획득함");
                    GetComponent<MeshFilter>().mesh = openMesh.mesh;

                    InventorySlot slot = playerCharacter.Inven.FindItemInInventory(keyObject.data);
                    slot.RemoveItem();
                    
                    animator?.SetTrigger(openHash); // 내용물 애니메이션
                    if (animator != null)   Destroy(displayObj, 2f);

                    DialogueManager.Instance.StartDialogue(getItemDialogue);
                }
            } else if (playerCharacter.HadItem(keyObject.data.id)) {    // * 2-3. 열쇠 사용 후 불러오기 했을 경우(이미 사용한 경우)
                Debug.Log("키 이미 사용함");
                DialogueManager.Instance.StartDialogue(afterItemDialogue);
                GetComponent<MeshFilter>().mesh = openMesh.mesh;
            }
        } else {    // ** 3. 열쇠 필요 없는 경우
            if (playerCharacter.HavingItem(itemObject.data.id) || playerCharacter.HadItem(itemObject.data.id)) {    
                Debug.Log("키 필요없고 이미 가지고 있음 or 가져본적 있음");
                GetComponent<MeshFilter>().mesh = openMesh.mesh;
                DialogueManager.Instance.StartDialogue(afterItemDialogue);
            } else if (playerCharacter?.PickupItem(itemObject) ?? false) {  
                Debug.Log("키 필요없고 획득함");
                animator?.SetTrigger(openHash); // 내용물 애니메이션
                if (animator != null)
                    Destroy(displayObj, 2f);

                DialogueManager.Instance.StartDialogue(getItemDialogue);
                GetComponent<MeshFilter>().mesh = openMesh.mesh;
            }
        }
    }

    public void StopInteract(GameObject other)
    {
        isStartInteract = false;
    }

    void OnEndDialogue()
    {
        StopInteract(interactGO);
    }

    void OnValidate() {
    #if UNITY_EDITOR
        openMesh = transform.Find("open").GetComponent<MeshFilter>();
    #endif
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distance);
    }
#endregion Methods
}
