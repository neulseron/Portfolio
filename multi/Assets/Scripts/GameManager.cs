using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GameManager : MonoBehaviourPunCallbacks
{
#region Singletone
    static GameManager instance;
    public static GameManager Instance => instance;
#endregion Singletone


#region Variables
    #region Flag
    [Header("[Flag]")]

    [SerializeField]
    Text RoundTxt;

    bool touchFlag;
    public bool TouchFlag => touchFlag;

    float flagTime;
    int nextFlagTime;
    #endregion Flag

    #region Game
    [Header("[Game]")]

    [SerializeField]
    Text FruitTxt;

    bool gameStart;
    public bool GameStart => gameStart;
    bool gameEnd;

    [SerializeField]
    GameObject winPanel;

    int gameStartTime;
    float startTimeCnt;

    PlayerController master;

    [SerializeField]
    Button readyBtn;
    bool readyClicked;
    #endregion Game

    #region Etc
    [Header("[Etc]")]
    public Transform[] spawnPoints;
    public GameObject BtnExit;
    public PhotonView PV;
    GameObject currMap;
    #endregion Etc

    #region Fruit
    [Header("[Fruit]")]
    Transform[] fruitSpawnPoints;
    string[] fruitArr = { "Apple", "Banana", "Cherrie", "Kiwi", "Pineapple" };
    int nextFruitTime;
    float fruitTime;
    bool needFruit;
    #endregion Fruit

    #region Chat
    [Header("[Chat]")]

    [SerializeField]
    GameObject ChatBox;
    [SerializeField]
    Text[] ChatTxt = new Text[6];
    [SerializeField]
    InputField InputTxt;
    #endregion Chat
#endregion Variables


#region Unity Methods
    public override void OnEnable() {
        nextFruitTime = 8; // 과일 아이템 쿨타임
        nextFlagTime = 30;  // 깃발 쿨타임
        gameStartTime = 5;

        readyClicked = false;
        gameStart = false;

        InputTxt.text = "";
        SetMap();
    }

    private void Awake() {
        instance = this;
    }

    private void Start() {
        for (int i = 0; i < ChatTxt.Length; i++)    
            ChatTxt[i].text = "";
    }

    private void Update() 
    {
        #region # Chk Master #
        if ((GameObject)(PhotonNetwork.CurrentRoom.GetPlayer(PhotonNetwork.CurrentRoom.masterClientId).TagObject) == null)  return;
        master = ((GameObject)(PhotonNetwork.CurrentRoom.GetPlayer(PhotonNetwork.CurrentRoom.masterClientId).TagObject)).GetComponent<PlayerController>();

        if (PhotonNetwork.IsMasterClient) {
            readyBtn.transform.Find("Text").GetComponent<Text>().text = "시작하기";
            readyBtn.transform.Find("readyCnt").gameObject.SetActive(true);

            ChkSelect();
            ChkReady();
        }
        #endregion Chk Master

        #region # Chk Ready #
        if (PhotonNetwork.CurrentRoom.MaxPlayers == PhotonNetwork.PlayerList.Length && master.isGameStart() 
            && !gameStart && (bool)PhotonNetwork.CurrentRoom.CustomProperties["allDecide"]) {

            startTimeCnt += Time.deltaTime;
            RoundTxt.text = "게임 시작까지... " + ((int)(gameStartTime - startTimeCnt)).ToString();
            FruitTxt.text = ((int)(gameStartTime - startTimeCnt)).ToString() + "초 후 출발 지점으로 이동됩니다.";

            if (startTimeCnt > gameStartTime) {
                for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
                    ((GameObject)(PhotonNetwork.PlayerList[i].TagObject)).transform.position = spawnPoints[i].position;

                RoundTxt.text = "깃발을 잡아라!";
                startTimeCnt = 0f;

                gameStart = true;   // 로컬 변수
                needFruit = false;
                gameEnd = false;
            }
        } else if (PhotonNetwork.CurrentRoom.MaxPlayers != PhotonNetwork.PlayerList.Length && !gameStart) {    // 인원이 다 차지 않았고 게임 중도 아닐때
            master.SetGameStart(false);
            RoundTxt.text = "다른 플레이어를 기다리는 중...";
        }
        #endregion Chk Ready

        #region # Game Start #
        if (gameStart) {
            readyBtn.gameObject.SetActive(false);

            #region ~ Chk Game End ~
            // 새로 깃발을 잡았으면
            if (!touchFlag && (bool)PhotonNetwork.CurrentRoom.CustomProperties["flagging"]) {
                //** 게임 끝 여부(누군가 두번 잡았는지) 확인 **//
                gameEnd = (bool)PhotonNetwork.CurrentRoom.CustomProperties["win"];

                if (!gameEnd) {
                    touchFlag = true;
                    flagTime = 0;
                }
            }
            #endregion Chk Game End

            #region ~ Game End ~
            if (gameEnd) {
                if (PhotonNetwork.IsMasterClient) {
                    if ((bool)PhotonNetwork.CurrentRoom.CustomProperties["flagging"]) {
                        Hashtable CP = PhotonNetwork.CurrentRoom.CustomProperties;
                        CP["flagging"] = false;
                        PhotonNetwork.CurrentRoom.SetCustomProperties(CP);

                        ResetSelect();
                        ResetReady();
                    }

                    FruitDestroy();
                }

                FruitTxt.text = "";
                GameObject.Find("Canvas").transform.Find("fruitInfo").gameObject.SetActive(false);
                winPanel.SetActive(true);

                return;
            }
            #endregion Game End

            #region ~ If Touch Flag ~
            // 누군가 깃발을 잡았으면 + 게임이 안끝났으면 =  깃발 쿨타임
            if (touchFlag) {
                flagTime += Time.deltaTime;
                RoundTxt.text = "남은 시간 : " + ((int)(nextFlagTime - flagTime)).ToString();

                // 쿨타임 끝났으면
                if (flagTime > nextFlagTime) {
                    touchFlag = false;
                    flagTime = 0f;
                    RoundTxt.text = "깃발을 잡아라!";

                    if ((bool)PhotonNetwork.CurrentRoom.CustomProperties["flagging"]) {
                        Hashtable CP = PhotonNetwork.CurrentRoom.CustomProperties;
                        CP["flagging"] = false;
                        PhotonNetwork.CurrentRoom.SetCustomProperties(CP);
                    }
                }
            }
            #endregion If Touch Flag
            
            #region ~ Fruit ~
            // ** 과일 총알 ** //
            if (!gameEnd) {
                var itemList = FindObjectsOfType<ItemFruit>();
                if (!needFruit && itemList.Length == 0) {  // 과일 하나도 없으면 쿨타임 시작
                    fruitTime = 0f;
                    needFruit = true;
                }

                if (needFruit) {
                    fruitTime += Time.deltaTime;
                    FruitTxt.text = "과일 생성까지 " + ((int)(nextFruitTime - fruitTime)).ToString() + "초";

                    if (fruitTime > nextFruitTime) {
                        // 방장이 한번에 생성
                        if (PhotonNetwork.IsMasterClient) {    
                            PV.RPC("SpawnFruitRPC", RpcTarget.All);
                        }
                    }
                }
            }
            #endregion Fruit
        }
        #endregion Game Start

        #region # Menu #
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (BtnExit.activeSelf)
                BtnExit.SetActive(false);
            else
                BtnExit.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.C)) {
            if (ChatBox.activeSelf)
                ChatBox.SetActive(false);
            else {
                ChatBox.SetActive(true);
                InputTxt.ActivateInputField();
            }
        }

        if (Input.GetKeyDown(KeyCode.Return)) {
            if (ChatBox.activeSelf)
                Send();
        }
        #endregion Menu
    }
#endregion Unity Methods


#region Methods
    #region # Chk Ready State #
    public void BtnReadyClicked()
    {
        if (PhotonNetwork.IsMasterClient && !ChkReady())    return;
        if (master.isGameStart())     return;

        if (PhotonNetwork.IsMasterClient && /*!gameStart*/!master.isGameStart()) {  // 나머지 준비 끝났고 마스터 변수 on 안됐을 때
            master.SetGameStart(true);
            return;
        }
        
        // 마스터 제외 나머지
        readyClicked = !readyClicked;
        ((GameObject)(PhotonNetwork.LocalPlayer.TagObject)).GetComponent<PlayerController>().SetReady(readyClicked);
        readyBtn.image.color = readyClicked == true ? new Color(0.5f, 0.5f, 0.5f, 1) : new Color(1, 1, 1, 1);
    }

    bool ChkReady()     // 마스터 제외 나머지 사람들 준비 상태 확인
    {
        if (gameStart)  return false;

        bool isReady = true;
        int whoReady = 0;
        for (int i = 1; i < PhotonNetwork.PlayerList.Length; i++) {
            GameObject player = (GameObject)(PhotonNetwork.PlayerList[i].TagObject);
            if (player == null)    return false;

            PlayerController playerLogic = player.GetComponent<PlayerController>();
            if (playerLogic.ready == false) {
                master.SetGameStart(false);
            } else  whoReady++;
        }
        readyBtn.transform.Find("readyCnt").GetComponent<Text>().text = (1 + whoReady).ToString() + " / 4";
        if (!isReady)   return false;

        return true;
    }

    void ResetReady()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++) {
            PlayerController playerLogic = ((GameObject)(PhotonNetwork.PlayerList[i].TagObject)).GetComponent<PlayerController>();
            playerLogic.SetReady(false);
        }
    }
    #endregion Chk Ready State

    #region # Chk End Select State #
    bool ChkSelect()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++) {
            GameObject player = (GameObject)(PhotonNetwork.PlayerList[i].TagObject);
            if (player == null)    return false;

            PlayerController playerLogic = player.GetComponent<PlayerController>();
            if (playerLogic.decide == false) {
                return false;
            }
        }

        if (!master.isGameStart()) {
            Hashtable CP2 = PhotonNetwork.CurrentRoom.CustomProperties;
            CP2["allDecide"] = true;
            PhotonNetwork.CurrentRoom.SetCustomProperties(CP2);
        }

        return true;
    }

    void ResetSelect()
    {
        if (!(bool)PhotonNetwork.CurrentRoom.CustomProperties["allDecide"])     return;

        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++) {
            PlayerController playerLogic = ((GameObject)(PhotonNetwork.PlayerList[i].TagObject)).GetComponent<PlayerController>();
            playerLogic.SetDecide(false);
            readyClicked = false;
        }

        Hashtable CP = PhotonNetwork.CurrentRoom.CustomProperties;
        CP["allDecide"] = false;
        PhotonNetwork.CurrentRoom.SetCustomProperties(CP);
    }
    #endregion Chk End Select State

    #region # Menu #
    public void BtnContinue()
    {
        gameStart = false;
        master.SetGameStart(false);
        winPanel.SetActive(false);

        readyBtn.image.color = new Color(1f, 1f, 1f, 1);
        readyClicked = false;
        readyBtn.gameObject.SetActive(true);

        ((GameObject)(PhotonNetwork.LocalPlayer.TagObject)).GetComponent<PlayerController>().face.InitFace();
        RoundTxt.text = "다른 플레이어를 기다리는 중...";

        Hashtable CP = PhotonNetwork.CurrentRoom.CustomProperties;
        if (PhotonNetwork.IsMasterClient && (bool)CP["win"])    CP["win"] = false;
        PhotonNetwork.CurrentRoom.SetCustomProperties(CP);

        ((GameObject)(PhotonNetwork.LocalPlayer.TagObject)).GetComponent<PlayerController>().SetDecide(true);
    }

    public void BtnLeaveRoom()
    {
        BtnExit.SetActive(false);
        winPanel.SetActive(false);

        ((GameObject)(PhotonNetwork.LocalPlayer.TagObject)).GetComponent<PlayerController>().face.InitFace();

        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel(0);
    }
    #endregion Menu

    #region # Fruit #
    [PunRPC]
    void SpawnFruitRPC()
    {
        PhotonNetwork.Instantiate("Item/" + fruitArr[Random.Range(0, 5)], fruitSpawnPoints[Random.Range(0, 10)].position, Quaternion.identity);

        needFruit = false;
        FruitTxt.text = "과일이 생성되었습니다.";
    }

    void FruitDestroy()
    {
        var itemList = FindObjectsOfType<ItemFruit>();
        foreach (ItemFruit i in itemList) {
            i.DestroyFruit();
        }
    }
    #endregion Fruit

    #region # Chat #
    public void Send()
    {
        string msg = PhotonNetwork.LocalPlayer.NickName + " : " + InputTxt.text;
        PV.RPC("ChatRPC", RpcTarget.All, msg);
        InputTxt.text = "";
        InputTxt.ActivateInputField();
    }

    public void Notice(string player, bool isEnter)
    {
        string msg = isEnter ? "<color=yellow>" + player + " 님이 들어왔습니다.</color>" : "<color=yellow>" + player + " 님이 나갔습니다.</color>";
        PV.RPC("ChatRPC", RpcTarget.All, msg);
    }

    [PunRPC]
    void ChatRPC(string msg)
    {
        bool isInput = false;
        for (int i = 0; i < ChatTxt.Length; i++) {
            if (ChatTxt[i].text == "")
            {
                isInput = true;
                ChatTxt[i].text = msg;
                break;
            }
        }

        if (!isInput) {
            for (int i = 1; i < ChatTxt.Length; i++)
                ChatTxt[i-1].text = ChatTxt[i].text;
            
            ChatTxt[ChatTxt.Length-1].text = msg;
        }
    }
    #endregion Chat

    #region # Spawn #
    public void SetMap()
    {
        GameObject map1 = GameObject.Find("stage1").gameObject;
        GameObject map2 = GameObject.Find("stage2").gameObject;

        string mapStr = "stage" + ((int)PhotonNetwork.CurrentRoom.CustomProperties["map"]).ToString();
        map1.SetActive(map1.name == mapStr ? true : false);
        map2.SetActive(map2.name == mapStr ? true : false);

        currMap = map1.activeSelf == true ? map1 : map2;

        spawnPoints = new Transform[currMap.transform.Find("RespawnPoints").childCount];
        for (int i = 0; i < spawnPoints.Length; i++) {
            spawnPoints[i] = currMap.transform.Find("RespawnPoints").GetChild(i);
        }
        fruitSpawnPoints = new Transform[currMap.transform.Find("ItemSpawnPoints").childCount];
        for (int i = 0; i < fruitSpawnPoints.Length; i++) {
            fruitSpawnPoints[i] = currMap.transform.Find("ItemSpawnPoints").GetChild(i);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player") { // 리스폰
            Respawn(other.transform);
        }
    }

    public void Respawn(Transform _player)
    {
        _player.position = spawnPoints[Random.Range(0, 4)].position;
    }
    #endregion Spawn
#endregion Methods
}
