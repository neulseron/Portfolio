using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class NetworkManager : MonoBehaviourPunCallbacks
{
#region Variables
    #region ConnectPanel
    [Header("[ConnectPanel]")]
    public GameObject connectPanel; 
    public InputField nicknameInput;
    #endregion ConnectPanel

    #region LobbyPanel
    [Header("[LobbyPanel]")]
    public GameObject lobbyPanel; 
    public InputField roomInput;
    public Text welcomeTxt;
    public Text lobbyInfoTxt;
    public Button[] cellBtns;
    public Button prevBtn;
    public Button nextBtn;
    public GameObject mapPanel;
    #endregion LobbyPanel

    #region Etc
    [Header("Etc")]
    public Text statusTxt;
    List<RoomInfo> rList = new List<RoomInfo>();
    int currPage = 1, maxPage, multiple;
    #endregion Etc
#endregion Variables


#region Unity Methods
    void Awake()
    {
        Screen.SetResolution(1024, 768, false);
    }

    void Update()
    {
        statusTxt.text = PhotonNetwork.NetworkClientState.ToString();
        lobbyInfoTxt.text = (PhotonNetwork.CountOfPlayers - PhotonNetwork.CountOfPlayersInRooms) + " 로비 / " + PhotonNetwork.CountOfPlayers + " 접속";
    }
#endregion Unity Methods


#region Methods
    #region # Connect Server #
    // ** 서버나 로비에 참가한 상태에서만 방에 참가할 수 있음
    public void BtnJoinServer()
    {
        if (nicknameInput.text == "")
            return;

        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();   
        print("서버 접속 완료");
    }

    public override void OnJoinedLobby()
    {
        connectPanel.SetActive(false);
        lobbyPanel.SetActive(true);

        if (nicknameInput.text != "")
            PhotonNetwork.LocalPlayer.NickName = nicknameInput.text;
        welcomeTxt.text = PhotonNetwork.LocalPlayer.NickName + "님 환영합니다.";

        rList.Clear();
    }

    public void BtnDisconnect() => PhotonNetwork.Disconnect();

    public override void OnDisconnected(DisconnectCause cause)
    { 
        print("연결 종료");
        connectPanel.SetActive(true);
        lobbyPanel.SetActive(false);
    }
    #endregion Connect Server

    #region # Create / Join Room #
    public void BtnCreateRoom() => mapPanel.SetActive(true);

    public void BtnRandom() => PhotonNetwork.JoinRandomRoom();

    public void CreateRoom(int mapNum)
    {
        RoomOptions roomOption = new RoomOptions();
        roomOption.MaxPlayers = 4;
        roomOption.CustomRoomProperties = new Hashtable() { { "0", -1 }, { "1", -1 }, { "2", -1 }, { "3", -1 },   // 캐릭터 : 플레이어 id
                                                            { "flagging", false }, { "map", mapNum }, { "win", false },
                                                            { "allDecide", false } };         

        PhotonNetwork.CreateRoom(roomInput.text == "" ? "Room" + Random.Range(0, 100) : roomInput.text, roomOption, null);
    }

    public override void OnJoinedRoom()
    {
        RoomManager.Instance.Spawn();
    }

    public override void OnCreateRoomFailed(short returnCode, string message) { roomInput.text = ""; BtnCreateRoom(); }
    public override void OnJoinRandomFailed(short returnCode, string message) { roomInput.text = ""; BtnCreateRoom(); }
    #endregion Create / Join Room

    #region # Room List #
    // 이전 : -2 / 다음 : -1 / 숫자 : 클릭 셀
    public void ClickRoomList(int _num)
    {
        switch (_num) {
            case -2:
                --currPage;
                break;
            case -1:
                ++currPage;
                break;
            default:
                PhotonNetwork.JoinRoom(rList[multiple + _num].Name);
                break;
        }

        rListRenewal();
    }

    void rListRenewal()
    {
        // cellBtns.Length : 한페이지에 표시할 방의 개수(4개)
        maxPage = (rList.Count % cellBtns.Length == 0) ? rList.Count / cellBtns.Length : rList.Count / cellBtns.Length + 1;

        prevBtn.interactable = (currPage <= 1) ? false : true;
        nextBtn.interactable = (currPage >= maxPage) ? false : true;

        multiple = (currPage - 1) * cellBtns.Length;
        for (int i = 0; i < cellBtns.Length; i++) {
            cellBtns[i].interactable = (multiple + i < rList.Count) ? true : false;
            cellBtns[i].transform.GetChild(0).GetComponent<Text>().text = (multiple + i < rList.Count) ? rList[multiple + i].Name : "";
            cellBtns[i].transform.GetChild(1).GetComponent<Text>().text = (multiple + i < rList.Count) ? rList[multiple + i].PlayerCount + "/" + rList[multiple + i].MaxPlayers: "";
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        int roomCount = roomList.Count;
        for (int i = 0; i < roomCount; i++) {
            if (!roomList[i].RemovedFromList) { // 존재하는 방이면
                if (!rList.Contains(roomList[i]))   // 없으면 리스트에 추가
                    rList.Add(roomList[i]);
                else
                    rList[rList.IndexOf(roomList[i])] = roomList[i];    // 있으면 인원수 등 갱신
            } else if (rList.IndexOf(roomList[i]) != -1)    // 리스트에 방이 존재하지 않을때 (네트워크에 존재하지 않는 방인데 리스트에서 지워지지 않았을때)
                rList.RemoveAt(rList.IndexOf(roomList[i]));
        }

        rListRenewal();
    }
    #endregion Room List
#endregion Methods


#region Context Menu
    [ContextMenu("정보")]
    void Info()
    {
        if (PhotonNetwork.InRoom) {
            print("현재 방 이름 : " + PhotonNetwork.CurrentRoom.Name);
            print("현재 방 인원수 : " + PhotonNetwork.CurrentRoom.PlayerCount);
            print("현재 방 최대 인원수 : " + PhotonNetwork.CurrentRoom.MaxPlayers);

            string playerStr = "방에 있는 플레이어 목록 : ";
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
                playerStr += PhotonNetwork.PlayerList[i].NickName + ",";
            print(playerStr);
        } else {
            print("접속한 인원수 : " + PhotonNetwork.CountOfPlayers);
            print("방 개수 : " + PhotonNetwork.CountOfRooms);
            print("모든 방에 있는 인원수 : " + PhotonNetwork.CountOfPlayersInRooms);
            print("로비에 있는지? : " + PhotonNetwork.InLobby);
            print("연결됐는지? : " + PhotonNetwork.IsConnected);
        }
    }
#endregion Context Menu
}
