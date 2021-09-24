using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OldList : MonoBehaviour
{
    public SwitchManager switchManager;
    public GameManager gameManager;

    public GameObject[] lists;
    public GameObject nextBtn;
    public GameObject prevBtn;

    int pageIndex;

    void OnEnable() {
        pageIndex = 0;
        prevBtn.GetComponent<Button>().interactable = false;
        nextBtn.GetComponent<Button>().interactable = true;
        lists[0].SetActive(true);

        
    }

    private void Start() {
        if (!switchManager.switchdata["Record_Page1"].off) {
            switchManager.switchdata["Record_Page1"].on = true;
        }
    }

    public void BtnPrev()
    {
        lists[pageIndex].SetActive(false);

        if (pageIndex > 0 && !switchManager.ing) {
            pageIndex--;
            nextBtn.GetComponent<Button>().interactable = true;
        }
        
        if (pageIndex == 0) {
            prevBtn.GetComponent<Button>().interactable = false;
        }

        lists[pageIndex].SetActive(true);
    }

    public void BtnNext()
    {
        lists[pageIndex].SetActive(false);

        if (pageIndex < lists.Length && !switchManager.ing) {
            pageIndex++;
            prevBtn.GetComponent<Button>().interactable = true;
        }
        
        if (pageIndex == lists.Length - 1) {
            nextBtn.GetComponent<Button>().interactable = false;
        }
        
        lists[pageIndex].SetActive(true);

        if (pageIndex == 1 && !switchManager.switchdata["Record_Page2"].off) {
            switchManager.switchdata["Record_Page2"].on = true;
        }
    }

    public void BtnExit()
    {
        lists[pageIndex].SetActive(false);

        pageIndex = 0;

        gameObject.SetActive(false);
        gameManager.dontMove = false;
    }
}
