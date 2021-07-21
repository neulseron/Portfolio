using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTriggerArea : MonoBehaviour
{
    public DoorEventObject doorEventObject;
    public DoorController doorController;

    public bool autoClose = false;


    void OnTriggerEnter(Collider other) {
        Debug.Log("문 부딛혔다");
        doorEventObject.OpenDoor(doorController.id);
    }

    void OnTriggerExit(Collider other) {
        doorEventObject.CloseDoor(doorController.id);
    }
}
