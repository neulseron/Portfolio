using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System.IO;

public class GameManager : MonoBehaviour
{
#region Variables
    #region Singletone
    static GameManager instance;
    public static GameManager Instance => instance;
    GameData data;
    public GameData Data => data;
    #endregion Singletone


    #region Sync Position
    public Transform[] syncMiddlePos;
    public Transform startPos;
    public Transform endPos;
    #endregion Sync Position


    #region UI
    [SerializeField]
    GameObject settingWindow;
    public GameObject systemMsg;
    #endregion UI


    #region Player
    [SerializeField]
    GameObject player;
    [SerializeField]
    StatsObject playerStat;
    #endregion Player


    #region Inventory
    public GameObject EquipUI;
    [SerializeField]
    GameObject invenGO;
    [SerializeField]
    GameObject equipGO;
    [SerializeField]
    GameObject gemGO;
    InventoryObject inventory;
    public InventoryObject Inven => inventory;
    InventoryObject equipment;
    [SerializeField]
    InventoryObject gem;
    #endregion Inventory
#endregion Variables


#region Unity Methods
    void Awake() {
        instance = this;

        inventory = invenGO.GetComponent<DynamicInventoryUI>().inventoryObject;
        equipment = equipGO.GetComponent<StaticInventoryUI>().inventoryObject;
    }

    void Start()
    {
        MenuLoad();

        Invoke("OffUI", 0.1f);
    }
    void OffUI() => EquipUI.SetActive(false);

    void Update()
    {
        // ** 인벤토리
        if (Input.GetKeyDown(KeyCode.I)) {
            if (invenGO.activeSelf) {
                invenGO.SetActive(false);
            } else {
                invenGO.SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.E)) {
            if (equipGO.activeSelf) {
                equipGO.SetActive(false);
                gemGO.SetActive(false);
            } else {
                equipGO.SetActive(true);
                gemGO.SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (settingWindow.activeSelf) {
                settingWindow.SetActive(false);
            } else {
                settingWindow.SetActive(true);
            }
        }
    }
#endregion Unity Methods


#region Menu
    public void MenuInven()
    {
        if (invenGO.activeSelf) {
            invenGO.SetActive(false);
        } else {
            invenGO.SetActive(true);
        }
    }

    public void MenuEquip()
    {
        if (equipGO.activeSelf) {
            equipGO.SetActive(false);
            gemGO.SetActive(false);
        } else {
            equipGO.SetActive(true);
            gemGO.SetActive(true);
        }
    }

    public void MenuSetting()
    {
        if (settingWindow.activeSelf) {
            settingWindow.SetActive(false);
        } else {
            settingWindow.SetActive(true);
        }
    }

    public void MenuSave()
    {
        inventory.Save();
        equipment.Save();
        gem.Save();
        playerStat.Save();
        QuestManager.Instance.Save();
        SavePosition();

        PlayerCharacter playerLogic = player.GetComponent<PlayerCharacter>();
        data.chkGetItem = playerLogic.hadGetList;

        SaveGameData();

        settingWindow.SetActive(false);

        systemMsg.transform.Find("Text").GetComponent<Text>().text = "저장했습니다.";
        systemMsg.SetActive(true);
        Invoke("disappearSystemMsg", 1f);
    }

    public void MenuLoad()
    {
        Debug.Log("=========[메뉴 로드]=========");
        LoadGameData();   
        Debug.Log("---------[게임 데이터 로드 완료]--------");

        PlayerCharacter playerLogic = player.GetComponent<PlayerCharacter>();
        playerLogic.hadGetList = data.chkGetItem;
        if (!playerLogic.anim.GetBool("IsAlive"))    playerLogic.anim.SetBool("IsAlive", true);

        inventory.Load();
        equipment.Load();
        gem.Load();
        Debug.Log("---------[인벤 로드 완료]--------");
        playerStat.Load();
        QuestManager.Instance.Load();
        Debug.Log("---------[퀘스트 로드 완료]--------");

        LoadPosition();
        Debug.Log("---------[위치 로드 완료]--------");

        

        settingWindow.SetActive(false);

        systemMsg.transform.Find("Text").GetComponent<Text>().text = "불러왔습니다.";
        systemMsg.SetActive(true);
        Invoke("disappearSystemMsg", 1f);

        Debug.Log("===========================");
    }

    public void MenuExit()
    {
        Application.Quit();
    }

    public void MenuRestart()
    {
        ResetGameData();

        settingWindow.SetActive(false);
    }

    void disappearSystemMsg()
    {
        systemMsg.SetActive(false);
    }
#endregion Menu


#region GameData Save/Load
    public string GameDataFileName = "SaveFile.json";

    public void SaveGameData()
    {
        string toJsonData = JsonUtility.ToJson(data);
        string filePath = Application.persistentDataPath + GameDataFileName;

        File.WriteAllText(filePath, toJsonData);

        Debug.Log("저장 완료");
    }

    public void LoadGameData()
    {
        string filePath = Application.persistentDataPath + GameDataFileName;

        if (File.Exists(filePath)) {
            Debug.Log("불러오기 성공");
            string FromJsonData = File.ReadAllText(filePath);
            data = JsonUtility.FromJson<GameData>(FromJsonData);
        } else {
            Debug.Log("새 파일 생성");
            data = new GameData();
        }
    }

    void ResetGameData()
    {
        string filePath = Application.persistentDataPath + GameDataFileName;
        if (File.Exists(filePath)) {
            Debug.Log("파일을 삭제했습니다.");
            File.Delete(filePath);
        }

        inventory.Clear();
        equipment.Clear();
        gem.Clear();
        player.GetComponent<PlayerCharacter>().hadGetList.Clear();
        playerStat.RestartInitAttribute();
        QuestManager.Instance.Init();

        data = new GameData();
        SaveGameData();
        LoadGameData();

        playerStat.OnChangedStats.Invoke(playerStat);

        MapManager.Instance.OnMesh();
        SyncPosition(startPos.position);
    }


    void SavePosition()
    {
        data.posX = player.transform.position.x;
        data.posY = player.transform.position.y;
        data.posZ = player.transform.position.z;
    }

    void LoadPosition()
    {
        Vector3 newPosition = new Vector3(data.posX, data.posY, data.posZ);
        
        Vector3 currPos = player.transform.position;
        // *** 저장 위치와 캐릭터 위치 동기화 ***
        // ** 1. 목적지~맵시작지점, 목적지~맵끝지점 까지의 거리 비교
        float fromStart = Vector3.Distance(newPosition, startPos.position);
        float fromEnd = Vector3.Distance(newPosition, endPos.position);
        // ** 1-1. 시작지점과 가까울 때
        if (fromStart < fromEnd) {
            SyncPosition(startPos.position);
            for (int i = 0; i < syncMiddlePos.Length; i++) {
                currPos = player.transform.position;
                float toDest = Vector3.Distance(currPos, newPosition);
                float nextDest = Vector3.Distance(currPos, syncMiddlePos[i].position);
                if (nextDest < toDest) {
                    SyncPosition(syncMiddlePos[i].position);
                } else {
                    break;
                }
            }
        // ** 1-2. 끝지점과 가까울 때
        } else {
            SyncPosition(endPos.position);
            for (int i = syncMiddlePos.Length - 1; i >= 0; i--) {
                currPos = player.transform.position;
                float toDest = Vector3.Distance(currPos, newPosition);
                float nextDest = Vector3.Distance(currPos, syncMiddlePos[i].position);
                if (nextDest < toDest) {
                    SyncPosition(syncMiddlePos[i].position);
                } else {
                    break;
                }
            }
        }
        SyncPosition(newPosition);

        player.GetComponent<NavMeshAgent>().updatePosition = true;

        MapManager.Instance.OffMesh();
    }

    public void SyncPosition(Vector3 newPos)
    {
        player.GetComponent<CharacterController>().enabled = false;

        player.transform.position = newPos;

        player.GetComponent<NavMeshAgent>().nextPosition = newPos;
        player.GetComponent<NavMeshAgent>().ResetPath();

        player.GetComponent<CharacterController>().enabled = true;
    }

#endregion GameData Save/Load
}