using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MailAttach : MonoBehaviour
{
    public GameManager gameManager;

    public Monitor monitor;
    public Inventory inventory;
    public GameObject mail6;
    public GameObject mail8;
    public GameObject mail7;

    void Update()
    {
        if (inventory.selectedItemIndex == 4) {
            inventory.selectedItemIndex = 0;

            monitor.switchManager.switchdata["SecondF_mailAttached"].on = true;
            mail6.SetActive(false);
            mail8.SetActive(true);
            mail7.SetActive(true);


            gameObject.SetActive(false);
            monitor.ReceiveMailSet.SetActive(true);
            monitor.currWindow = 0;

            inventory.nowUsing = false;
        } else if (inventory.selectedItemIndex != 0) {
            inventory.selectedItemIndex = 0;
            StartCoroutine(ErrorMsg());

            inventory.nowUsing = false;
        }
    }

    IEnumerator ErrorMsg()
    {
        gameManager.currSceneIndex = 200;
        gameManager.currCutIndex = 0;
        gameManager.SystemAction();

        yield return new WaitUntil(() => gameManager.isSystem == false);
        StopAllCoroutines();
    }
}
