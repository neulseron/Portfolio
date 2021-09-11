using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTriggerArea : MonoBehaviour
{
#region Variables
    public DoorEventObject doorEventObject;
    public DoorController[] doorControllers;

    [SerializeField]
    ItemObject keyObject = null;
    public bool autoClose = false;
    bool opened = false;

    [Header("Dialogue")]
    public Dialogue openDialogue;
    public Dialogue cannotOpenDialogue;
#endregion Variables


#region Methods
    void OnTriggerEnter(Collider other) 
    {
        if (opened)     return;
        if (keyObject != null) {
            PlayerCharacter playerCharacter = other.GetComponent<PlayerCharacter>();

            DialogueManager.Instance.OnEndDialogue += OnEndDialogue;

            if (!playerCharacter.HavingItem(keyObject.data.id) && !playerCharacter.HadItem(keyObject.data.id)) { // ** 1. 열쇠를 획득한 적 없는 경우
                DialogueManager.Instance.StartDialogue(cannotOpenDialogue);
            } else if (playerCharacter.HavingItem(keyObject.data.id)) {    // ** 2. 열쇠를 지금 가지고 있는 경우
                DialogueManager.Instance.StartDialogue(openDialogue);
                InventorySlot slot = playerCharacter.Inven.FindItemInInventory(keyObject.data);
                slot.RemoveItem();

                for (int i = 0; i < doorControllers.Length; i++) {
                    doorEventObject.OpenDoor(doorControllers[i].id);
                }
                opened = true;
            } else if (playerCharacter.HadItem(keyObject.data.id)) {    // ** 3. 열쇠 사용 후 불러오기 했을 경우
                for (int i = 0; i < doorControllers.Length; i++) {
                    doorEventObject.OpenDoor(doorControllers[i].id);
                }
                opened = true;
            } 
        }
        
    }

    void OnEndDialogue()
    {
    }

    void OnTriggerExit(Collider other) 
    {
        if (opened && !autoClose)     return;
        
        for (int i = 0; i < doorControllers.Length; i++) {
            doorEventObject.CloseDoor(doorControllers[i].id);
        }
    }
#endregion Methods
}
