using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public DoorEventObject doorEventObject;
    public int id = 0;

    public float openOffset = 4f;
    public float closeOffset = -2f;


    void OnEnable()
    {
        doorEventObject.OnOpenDoor += OnOpenDoor;
        doorEventObject.OnCloseDoor += OnCloseDoor;
    }

    void OnDisable()
    {
        doorEventObject.OnOpenDoor -= OnOpenDoor;
        doorEventObject.OnCloseDoor -= OnCloseDoor;
    }

    public void OnOpenDoor(int id)
    {
        Debug.Log("id : " + id + ", this.id : " + this.id + ", is same? : " + (id == this.id ? true : false));
        if (id != this.id)      return;
        Debug.Log("Open!");

        StopAllCoroutines();
        Debug.Log("After Stop!");
        StartCoroutine(OpenDoor());
        Debug.Log("After Coroutine!");
    }

    public void OnCloseDoor(int id)
    {
        if (id != this.id)      return;

        StopAllCoroutines();
        StartCoroutine(CloseDoor());
    }

    IEnumerator OpenDoor()
    {
        float calz = 0f;
        if (openOffset > 0) {
            while (calz < openOffset) {
            //while (transform.rotation.z < openOffset) {
                //Debug.Log("문 : " + transform.rotation.z + ", 오프셋 : " + openOffset);
                calz += 1f;
                //transform.rotation = Quaternion.Euler(0, 0, calz);
                //transform.eulerAngles = new Vector3(0,0, calz);
                transform.Rotate(new Vector3(0, 0, calz) * Time.deltaTime);

                yield return null;
            }
        } else {
            while (calz > openOffset) {
            //while (transform.rotation.z < openOffset) {
                //Debug.Log("문 : " + transform.rotation.z + ", 오프셋 : " + openOffset);
                calz -= 1f;
                //transform.rotation = Quaternion.Euler(0, 0, calz);
                //transform.eulerAngles = new Vector3(0,0, calz);
                transform.Rotate(new Vector3(0, 0, calz) * Time.deltaTime);

                yield return null;
            }
        }
    }

    IEnumerator CloseDoor()
    {
        /*
        while (transform.position.y > closeOffset) {
            Vector3 calcPosition = transform.position;
            calcPosition.y -= 0.01f;
            transform.position = calcPosition;

            yield return null;
        }
        */

        float calz = transform.rotation.z;
        while (calz > closeOffset) {
            calz -= 1f;
            transform.Rotate(new Vector3(0, 0, calz) * Time.deltaTime);

            yield return null;
        }
    }
}
