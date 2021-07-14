using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class RoomManager : MonoBehaviourPunCallbacks
{
    #region  싱글톤
    static GameObject _container;
    static GameObject Container
    {
        get {
            return _container;
        }
    }
    static RoomManager _instance;
    public static RoomManager Instance
    {
        get {
            if (!_instance) {
                _container = new GameObject();
                _container.name = "RoomManager";
                _instance = _container.AddComponent(typeof(RoomManager)) as RoomManager;
                DontDestroyOnLoad(_container);
            }
            return _instance;
        }
    }
    #endregion

    string[] typeArr = { "maskDude", "ninjaFrog", "pinkMan", "virtualGuy" };
    int spawnTurn;

    public void Spawn()
    {
        // ** 어느 캐릭터 생성할 차례인지 확인 **
        SpawnCharacter();

        // 맵 씬으로 이동
        PhotonNetwork.LoadLevel(1);

        StartCoroutine(sp());
    }

    void SpawnCharacter()
    {
        Hashtable CP = PhotonNetwork.CurrentRoom.CustomProperties;
        for (int i = 0; i < 4; i++) {
            // ** 캐릭터 생성 여부
            string key = i.ToString();
            int b = (int)CP[key];
            if (b == -1) {
                spawnTurn = i;      //  ** 생성할 차례(spawnTurn)
                
                // ** 캐릭터-플레이어 연결
                CP[key] = PhotonNetwork.LocalPlayer.ActorNumber;
                PhotonNetwork.CurrentRoom.SetCustomProperties(CP);
                break;
            }
        }
    }

    IEnumerator sp()
    {
        yield return new WaitUntil(() => PhotonNetwork.LevelLoadingProgress >= 1);

        GameObject pFace = PhotonNetwork.Instantiate("Character/" + typeArr[spawnTurn], Vector3.zero, Quaternion.identity).transform.Find("player"+spawnTurn).gameObject;
        pFace.transform.Find("name").GetComponent<Text>().text = PhotonNetwork.LocalPlayer.NickName;
        pFace.transform.SetParent(GameObject.Find("Canvas").transform, false);

        // ** 이미 들어와있던 사람들 얼굴 동기화 **
        StartCoroutine(SyncFace());
    }


    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // ** 새로 들어온 사람 얼굴 동기화 **
        GameObject.Find("GameManager").GetComponent<GameManager>().Notice(newPlayer.NickName, true);
        StartCoroutine(UpdateFace(newPlayer));
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        ((GameObject)(PhotonNetwork.MasterClient.TagObject)).GetComponent<PlayerController>().face.transform.Find("master").gameObject.SetActive(true);
        GameObject.Find("GameManager").GetComponent<GameManager>().Notice(otherPlayer.NickName, false);

        Hashtable CP = PhotonNetwork.CurrentRoom.CustomProperties;
        int actorId = otherPlayer.ActorNumber;
        for (int i = 0; i < 4; i++) {
            string key = i.ToString();
            int val = (int)CP[key];
            if (val == actorId) {
                CP[key] = -1;
                PhotonNetwork.CurrentRoom.SetCustomProperties(CP);

                GameObject.Find("Canvas").transform.Find("player"+i).gameObject.SetActive(false);
                Destroy(GameObject.Find("Canvas").transform.Find("player"+i).gameObject);

                return;
            }
        }
    }


    IEnumerator SyncFace()
    {
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++) {
            int actorId = PhotonNetwork.PlayerList[i].ActorNumber;
            Transform face = ((GameObject)(PhotonNetwork.PlayerList[i].TagObject)).transform.Find("player"+IdToCharacter(actorId));
            if (face == null)    continue;

            face.Find("name").GetComponent<Text>().text = PhotonNetwork.PlayerList[i].NickName;
            face.GetComponent<FaceUI>().InitFace();
            face.SetParent(GameObject.Find("Canvas").transform, false);
        }
    }
    IEnumerator UpdateFace(Player p)
    {
        yield return new WaitUntil(() => ((GameObject)(p.TagObject)) != null);
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++) {
            int actorId = PhotonNetwork.PlayerList[i].ActorNumber;
            Transform face = ((GameObject)(PhotonNetwork.PlayerList[i].TagObject)).transform.Find("player"+IdToCharacter(actorId));
            if (face == null)    continue;

            face.Find("name").GetComponent<Text>().text = PhotonNetwork.PlayerList[i].NickName;
            face.GetComponent<FaceUI>().InitFace();
            face.SetParent(GameObject.Find("Canvas").transform, false);
        }
    }
    
    public int IdToCharacter(int _id)   // 액터 아이디 -> 몇번째 캐릭터인지
    {
        Hashtable CP = PhotonNetwork.CurrentRoom.CustomProperties;
        for (int i = 0; i < 4; i++) {
            string key = i.ToString();
            int val = (int)CP[key];
            if (val == _id) {
                return i;
            }
        }

        return -1;
    }


}
