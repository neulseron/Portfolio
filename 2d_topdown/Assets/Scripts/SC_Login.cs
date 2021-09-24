using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SC_Login : MonoBehaviour
{
    public Inventory inventory;
    public SearchCom searchCom;
    //------------------------------------------------------
    public GameObject ingMsg;
    public Text wellcomeMsg;
    //------------------------------------------------------
    
    void Start() {
        searchCom.currentScreen = 0;
    }

    void OnEnable() {
        inventory.nowUsing = true;
    }

    void Update() {
        if (inventory.selectedItemIndex != 0 && inventory.selectedItemIndex == 2) {
            inventory.selectedItemIndex = 0;
            StartCoroutine(Login());
        } else if (inventory.selectedItemIndex != 0 && inventory.selectedItemIndex == 3) {
            inventory.selectedItemIndex = 0;
            StartCoroutine(LoginHS());
        } else if (inventory.selectedItemIndex != 0) {
            inventory.selectedItemIndex = 0;
            StartCoroutine(LoginFail());
        }
    }

    IEnumerator Login()
    {
        ingMsg.SetActive(true);
        yield return new WaitForSeconds(0.8f);
        ingMsg.SetActive(false);

        wellcomeMsg.text = "반갑습니다. 서재하님.";
        wellcomeMsg.gameObject.SetActive(true);

        yield return new WaitForSeconds(1.5f);
        //------------------------------------------------------
        searchCom.loginEnd = true;
        wellcomeMsg.gameObject.SetActive(false);
        //------------------------------------------------------
        inventory.nowUsing = false;
        gameObject.SetActive(false);
        StopAllCoroutines();
    }

    IEnumerator LoginHS()
    {
        ingMsg.SetActive(true);
        yield return new WaitForSeconds(0.8f);
        ingMsg.SetActive(false);

        wellcomeMsg.text = "반갑습니다. 차현석님.";
        wellcomeMsg.gameObject.SetActive(true);

        yield return new WaitForSeconds(1.5f);
        //------------------------------------------------------
        searchCom.loginEnd = true;
        wellcomeMsg.gameObject.SetActive(false);
        //------------------------------------------------------
        inventory.nowUsing = false;
        gameObject.SetActive(false);
        StopAllCoroutines();
    }

    IEnumerator LoginFail()
    {
        ingMsg.SetActive(true);
        yield return new WaitForSeconds(0.8f);
        ingMsg.SetActive(false);

        wellcomeMsg.text = "등록되지 않은 사용자 입니다.";
        wellcomeMsg.gameObject.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        wellcomeMsg.gameObject.SetActive(false);
        
        yield return null;
        StopAllCoroutines();
    }
}
