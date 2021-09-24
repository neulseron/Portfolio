using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FindComa : MonoBehaviour
{
    public ComaData comaData;
    //------------------------------------------------------
    public GameObject ingMsg;
    public InputField inputNum;
    //------------------------------------------------------
    
    public void EndInput()
    {
        if (inputNum.text.Length == 6) {
            StartCoroutine(Searching());
        } else {
            ingMsg.GetComponent<Text>().text = "올바른 번호를 입력해 주세요.";
            ingMsg.SetActive(true);
        }
    }

    IEnumerator Searching()
    {
        ingMsg.GetComponent<Text>().text = "조회 중...";
        ingMsg.SetActive(true);
        comaData.inputComaId = inputNum.text;
        Debug.Log(comaData.inputComaId);
        yield return new WaitForSeconds(2f);
        //------------------------------------------------------
        inputNum.text = "";
        ingMsg.SetActive(false);
        comaData.searchComaEnd = true;
        //------------------------------------------------------
        gameObject.SetActive(false);
        StopAllCoroutines();
    }
}
