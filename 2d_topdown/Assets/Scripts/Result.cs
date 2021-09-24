using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    public ComaData comaData;
    public Text nameTxt;
    public Text birthTxt;
    public Text idTxt;
    public Text responseTxt;
    public Text dateTxt;
    public Text contentTxt;
    public GameObject nextBtn;
    public GameObject prevBtn;

    medicalDataForm mdf;
    int pageIndex;

    private void OnEnable() {
        pageIndex = 0;
        prevBtn.GetComponent<Button>().interactable = false;
        nextBtn.GetComponent<Button>().interactable = true;
    }

    void Update()
    {
        mdf = comaData.medicalID(comaData.inputComaId);

        if (mdf.date.Count == 1) {
            nextBtn.GetComponent<Button>().interactable = false;
        }

        nameTxt.text = "이름 : " + mdf.name;
        birthTxt.text = "주민번호 : " + mdf.birth;
        idTxt.text = "환자번호 : " + mdf.id;
        responseTxt.text = "담당자 : " + mdf.response;
        dateTxt.text = "[진료기록] " + mdf.date[pageIndex];
        contentTxt.text = mdf.content[pageIndex];
    }

    public void BtnNext()
    {
        if (pageIndex < mdf.date.Count) {
            pageIndex++;
            prevBtn.GetComponent<Button>().interactable = true;
        }
        
        if (pageIndex == mdf.date.Count - 1) {
            nextBtn.GetComponent<Button>().interactable = false;
        }
    }

    public void BtnPrev()
    {
        if (pageIndex > 0) {
            pageIndex--;
            nextBtn.GetComponent<Button>().interactable = true;
        }
        
        if (pageIndex == 0) {
            prevBtn.GetComponent<Button>().interactable = false;
        }
    }
}
