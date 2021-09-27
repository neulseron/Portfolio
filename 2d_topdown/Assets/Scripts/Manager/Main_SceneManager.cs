using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;

// [시작 화면]
public class Main_SceneManager : MonoBehaviour
{
#region Variables
    public Button continueBtn;
    public GameObject saveSlot;
    string btnName;
    string[] saveMsg;
#endregion Variables


#region Methods
    void Start()
    {
        if (File.Exists(Application.persistentDataPath + "SaveFile.json")) {
            continueBtn.interactable = true;
        } else
            continueBtn.interactable = false;

        saveMsg = new string[7] { "2030-03-06(수) 오후  2:10:00", "2030-03-06(수)  오후  3:41:00", "2030-03-06(수)  오후  5:13:00", "2030-03-06(수)  오후  9:20:00", "2030-03-07(목)  오전  9:30:00", "2030-03-07(목)  오전  9:50:00", "2030-03-07(목) 오후  1:40:00" };
    }

    void Update()
    {
        switch (btnName) {
            case "BtnStart":
                File.Delete(Application.persistentDataPath + "SaveFile.json");
                UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
                DataManager.Instance.LoadGameData();
                break;
            case "BtnContinue": 
                DataManager.Instance.LoadGameData();
                string playerName = "";
                if (DataManager.Instance.gameData.playerName == "P_Jaei") {
                    playerName = "서재이";
                } else if (DataManager.Instance.gameData.playerName == "P_Jaeha") {
                    playerName = "서재하";
                } else if (DataManager.Instance.gameData.playerName == "P_HyeonSeok") {
                    playerName = "차현석";
                }
                saveSlot.transform.Find("Load1").gameObject.transform.Find("player").gameObject.GetComponent<Text>().text = playerName;
                saveSlot.transform.Find("Load1").gameObject.transform.Find("time").gameObject.GetComponent<Text>().text = saveMsg[DataManager.Instance.gameData.time];

                saveSlot.SetActive(true);
                break;
            case "BtnEnd":
                Application.Quit();
                break;
        }

        if (Input.GetButtonDown("Cancel")) {
            if (saveSlot.activeSelf) {
                saveSlot.SetActive(false);
                btnName = "";
            }
        }
    }

    public void BtnClick()
    {
        GameObject btn = EventSystem.current.currentSelectedGameObject;
        btnName = btn.name;
    }

    public void SelectSave()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
        saveSlot.SetActive(false);
    }
#endregion Methods
}
