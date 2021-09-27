using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
#region Singletone
    static GameManager instance;
    public static GameManager Instance => instance;
    GameData data;
    public GameData Data => data;
#endregion Singletone


#region Variables
    public MainCamera main_camera;
    public FadeController fadeEffect;

    #region Player
    [Header("[Player]")]
    public GameObject player;
    public GameObject scanObj;
    public bool dontMove;
    public bool canSleep;
    #endregion Player

    #region Interact
    [Header("[Interact]")]
    public Animator talkBox;
    public bool isTalking;
    public int currSceneIndex;
    public int currCutIndex;
    public bool isInterAction;
    public int objIndex;
    public int objCutIndex;
    public Animator systemBox;
    public bool isSystem;
    // [선택지 창]
    public GameObject selectBox;
    public int selectIndex;
    public bool isSelecting;
    #endregion Interact

    #region Map
    [Header("[Map]")]
    public GameObject[] maps;
    public int currMapIndex = 0;
    public int currShowMinimapIndex;
    public GameObject mapUI;
    #endregion Map

    #region Menu
    public GameObject menuSet;
    public GameObject inventory;
    public GameObject saveSlot;
    public bool dontSave;
    bool isSave;
    public int currTime = 0;
    string[] saveMsg;
    #endregion Menu
#endregion Variables
        

#region Unity Methods
    void Awake() {
        instance = this;
        
        GameLoad();
        saveMsg = new string[9] { "2030-03-06(수) 오후  2:10:00", "2030-03-06(수)  오후  3:41:00", "2030-03-06(수)  오후  5:13:00", "2030-03-06(수)  오후  9:20:00", "2030-03-07(목)  오전  9:30:00", "2030-03-07(목)  오전  9:50:00", "2030-03-07(목) 오후  1:40:00", "2030-03-07(목) 오후  4:52:00", "2030-03-06(수) 오후  3:10:00" };
    }

    void Start() {
        // ** 초기화 **
        currTime = 0;
        
        if (DataManager.Instance.gameData.itemList.Count > 0 && DataManager.Instance.gameData.playerName != "P_HyeonSeok") {
            inventory.GetComponent<Inventory>().InitInven(DataManager.Instance.gameData.itemList);
        } else if (DataManager.Instance.gameData.playerName == "P_HyeonSeok") {
            inventory.GetComponent<Inventory>().InitInven(new List<int>(3));
        }

        inventory.SetActive(false);

        currShowMinimapIndex = 1;
        currMapIndex = DataManager.Instance.gameData.mapIndex;

        // ** 세이브 슬롯 **
        if (DataManager.Instance.gameData.time != -1) {
            string playerName = "";
            if (DataManager.Instance.gameData.playerName == "P_Jaei") {
                playerName = "서재이";
            } else if (DataManager.Instance.gameData.playerName == "P_Jaeha") {
                playerName = "서재하";
            } else if (DataManager.Instance.gameData.playerName == "P_HyeonSeok") {
                playerName = "차현석";
            }
            saveSlot.transform.Find("Load1").gameObject.transform.Find("player").gameObject.GetComponent<Text>().text = playerName;
            saveSlot.transform.Find("Load1").gameObject.transform.Find("time").gameObject.GetComponent<Text>().text = saveMsg[currTime];
        } else {
            saveSlot.transform.Find("Load1").gameObject.transform.Find("player").gameObject.GetComponent<Text>().text = "-";
            saveSlot.transform.Find("Load1").gameObject.transform.Find("time").gameObject.GetComponent<Text>().text = "-";
        }

        // ** 게임 시작 **
        if (!SwitchManager.Instance.switchdata["JR_callDr"].off) {      // 저장된 데이터 없으면
            SwitchManager.Instance.AddDic(new SwitchData("JR_callDr", false, false));
            SwitchManager.Instance.switchdata["JR_callDr"].on = true;
        }
    }

    void Update() {
        // ** 인벤토리 (I) **
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (inventory.activeSelf) {
                dontMove = false;
                inventory.SetActive(false);
            }
            else {
                dontMove = true;
                inventory.SetActive(true);
            }
        }

        // ** 맵 (M) **
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (mapUI.activeSelf)
                mapUI.SetActive(false);
            else
                mapUI.SetActive(true);
        }

        // ** 메뉴 (ESC) **
        if (Input.GetButtonDown("Cancel"))
        {
            if (menuSet.activeSelf) {
                dontMove = false;
                menuSet.SetActive(false);
            }
            else {
                dontMove = true;
                menuSet.SetActive(true);
            }

            if (menuSet.activeSelf && saveSlot.activeSelf) {
                saveSlot.SetActive(false);
            }
            
        }
    }
#endregion Unity Methods


#region Methods
    public IEnumerator IntroFade()
    {
        dontSave = true;
        dontMove = true;
        yield return StartCoroutine(fadeEffect.SetTxt("서재이", "2030-03-06(수)\n오후  2:10:00"));
        dontSave = false;
        dontMove = false;
    }


    #region # Player #
    public void SpawnPlayer(string type, Vector3 pos)
    {
        if (player != null) {
            player.SetActive(false);
        }

        player = ObjectManager.Instance.MakeObj(type);
        player.transform.position = pos;
        main_camera.target = player.transform;
    }

    public void OffPlayer(GameObject _p)
    {
        _p.SetActive(false);
    }

    public GameObject SpawnNPC(string _type, Vector3 _pos)
    {
        GameObject npc = ObjectManager.Instance.MakeObj(_type);
        npc.transform.position = _pos;

        return npc;
    }

    public GameObject SpawnRandomNPC(string _type, Vector3 _pos)
    {
        GameObject npc = ObjectManager.Instance.MakeNPC(_type);
        npc.transform.position = _pos;

        return npc;
    }
    #endregion # Player #


    #region # Interaction #
    public void Action()
    {
        DialogManager.Instance.Talk(currSceneIndex, currCutIndex);
        talkBox.SetBool("isShow", isTalking);
    }

    public void InterAction()
    {
        isInterAction = true;
        DialogManager.Instance.ObjTalk(objIndex, objCutIndex);
        talkBox.SetBool("isShow", isInterAction);
    }

    public void SystemAction()
    {
        isSystem = true;
        DialogManager.Instance.SystemTalk(currSceneIndex, currCutIndex);
        systemBox.SetBool("isShow", isSystem);
    }

    public void AddItem(int _id)
    {
        Item item = ItemManager.Instance.itemDic[_id];
        inventory.GetComponent<Inventory>().AddItem(_id);
        Debug.Log(item.itemName);

        if (item.destroyable)
            scanObj.SetActive(false);

        currSceneIndex = 100;
        currCutIndex = 0;
        DialogManager.Instance.itemName = item.itemName;

        SystemAction();
    }
    #endregion # Interaction #


    #region # Map #
    public void ChangeMap(int _mapIndex, string _type, Vector3 _playerSpawnPos, string _turnDir, int _showMap)
    {
        maps[currMapIndex].SetActive(false);
        maps[_mapIndex].SetActive(true);
        currMapIndex = _mapIndex;
        currShowMinimapIndex = _showMap;

        SpawnPlayer(_type, _playerSpawnPos);

        player.GetComponent<PlayerAction>().Turn(_turnDir);
    }
    #endregion # Map #


    #region # Switch Manage #
    public void AddSwitchToManager(SwitchData _data)
    {
        bool chk = DataManager.Instance.switchDic.ContainsKey(_data.name);

        if (!chk) {
            DataManager.Instance.gameData.switchList.Add(_data);
            DataManager.Instance.switchDic.Add(_data.name, _data);
        }
    }

    public void SwitchManagerToDataManager()
    {
        foreach (KeyValuePair<string, SwitchData> p in SwitchManager.Instance.switchdata) {
            if (DataManager.Instance.switchDic.ContainsKey(p.Key)) {
                DataManager.Instance.switchDic[p.Key].on = p.Value.on;
                DataManager.Instance.switchDic[p.Key].off = p.Value.off;

                DataManager.Instance.gameData.switchList.Remove(p.Value);
                DataManager.Instance.gameData.switchList.Add(p.Value);
            } else
                AddSwitchToManager(p.Value);
        }
    }

    public void DataManagerToSwitchManager()
    {
        foreach (KeyValuePair<string, SwitchData> p in DataManager.Instance.switchDic) {
            if (SwitchManager.Instance.switchdata.ContainsKey(p.Key)) {
                SwitchManager.Instance.switchdata[p.Key].on = p.Value.on;
                SwitchManager.Instance.switchdata[p.Key].off = p.Value.off;
            } else
                SwitchManager.Instance.AddDic(p.Value);
        }
    }
    #endregion # Switch Manage #


    #region Save / Load
    public void BtnLoad()
    {
        saveSlot.SetActive(true);
        isSave = false;
    }

    public void BtnSave()
    {
        if (dontSave) {
            StartCoroutine(cantUseFunc());
            return;
        }

        saveSlot.SetActive(true);
        isSave = true;
    }

    public void BtnSaveSlot()
    {
        GameObject btn = EventSystem.current.currentSelectedGameObject;

        if (isSave) {   // 세이브
            string playerName = "";
            if (player.GetComponent<PlayerAction>().myName == "P_Jaei") {
                playerName = "서재이";
            } else if (player.GetComponent<PlayerAction>().myName == "P_Jaeha") {
                playerName = "서재하";
            } else if (player.GetComponent<PlayerAction>().myName == "P_HyeonSeok") {
                playerName = "차현석";
            }
            btn.transform.Find("player").gameObject.GetComponent<Text>().text = playerName;
            btn.transform.Find("time").gameObject.GetComponent<Text>().text = saveMsg[currTime];

            GameSave();
        } else {    // 로드
            if (DataManager.Instance.gameData.time == -1)    return;
            inventory.SetActive(true);
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void GameSave()
    {
        // ** 플레이어 **
        DataManager.Instance.gameData.playerX = player.transform.position.x;
        DataManager.Instance.gameData.playerY = player.transform.position.y;
        DataManager.Instance.gameData.playerName = player.GetComponent<PlayerAction>().myName;
        // ** 맵 **
        DataManager.Instance.gameData.mapIndex = currMapIndex;
        DataManager.Instance.gameData.time = currTime;
        // ** 아이템 **
        DataManager.Instance.gameData.itemList = inventory.GetComponent<Inventory>().itemIndexList;
        Debug.Log(inventory.GetComponent<Inventory>().itemIndexList);

        // ** 스위치 **
        SwitchManagerToDataManager();

        DataManager.Instance.SaveGameData();
        saveSlot.SetActive(false);
    }

    public void GameLoad()
    {
        // ** 플레이어 **
        player = ObjectManager.Instance.MakeObj(DataManager.Instance.gameData.playerName);
        float x = DataManager.Instance.gameData.playerX;
        float y = DataManager.Instance.gameData.playerY;
        player.transform.position = new Vector3(x, y, 0);
        main_camera.target = player.transform;
        // ** 맵 **
        maps[DataManager.Instance.gameData.mapIndex].SetActive(true);
        currTime = DataManager.Instance.gameData.time;
        // ** 스위치 **
        DataManager.Instance.InitializeDic();
        DataManagerToSwitchManager();
    }

    IEnumerator cantUseFunc()   // 저장 불가능한 지역일 때
    {
        SceneManager.Instance.PlaySystemTalk(10, 400);
        yield return new WaitUntil(() => isSystem == false);
    }
    #endregion Save / Load


    public void GameExit()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Title");
        //Application.Quit();
    }
#endregion Methods
}
