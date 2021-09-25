using System.Collections;
using UnityEngine;

public class DoorController : MonoBehaviour
{
#region Variables
    public DoorEventObject doorEventObject;
    public int id = 0;

    public float openOffset = 4f;
    public float closeOffset = -2f;
#endregion Variables


#region Methods
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
        if (id != this.id)      return;

        StopAllCoroutines();
        StartCoroutine(OpenDoor());
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
                calz += 1f;
                transform.Rotate(new Vector3(0, 0, calz) * Time.deltaTime);

                yield return null;
            }
        } else {
            while (calz > openOffset) {
                calz -= 1f;
                transform.Rotate(new Vector3(0, 0, calz) * Time.deltaTime);

                yield return null;
            }
        }
    }

    IEnumerator CloseDoor()
    {
        float calz = transform.rotation.z;
        while (calz > closeOffset) {
            calz -= 1f;
            transform.Rotate(new Vector3(0, 0, calz) * Time.deltaTime);

            yield return null;
        }
    }
#endregion Methods
}
