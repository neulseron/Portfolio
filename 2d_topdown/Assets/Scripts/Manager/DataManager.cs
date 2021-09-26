using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataManager : MonoBehaviour
{
#region Singletone
    static GameObject container;
    static GameObject Container => container;
    
    static DataManager instance;
    public static DataManager Instance
    {
        get {
            if (!instance) {
                container = new GameObject();
                container.name = "DataManager";
                instance = container.AddComponent(typeof(DataManager)) as DataManager;
                DontDestroyOnLoad(container);
            }
            return instance;
        }
    }
#endregion Singletone


#region Game Data
    public string GameDataFileName = "SaveFile.json";

    public GameData _gameData;
    public GameData gameData
    {
        get {
            if (_gameData == null) {
                LoadGameData();
                SaveGameData();
            }
            return _gameData;
        }
    }

    public void LoadGameData()
    {
        string filePath = Application.persistentDataPath + GameDataFileName;

        if (File.Exists(filePath)) {
            Debug.Log("불러오기 성공");
            string FromJsonData = File.ReadAllText(filePath);
            _gameData = JsonUtility.FromJson<GameData>(FromJsonData);
        } else {
            Debug.Log("새 파일 생성");
            _gameData = new GameData();
        }
    }

    public void SaveGameData()
    {
        string ToJsonData = JsonUtility.ToJson(gameData);
        string filePath = Application.persistentDataPath + GameDataFileName;

        File.WriteAllText(filePath, ToJsonData);
        Debug.Log("저장완료");
    }
#endregion Game Data


#region Switch Data
    public Dictionary<string, SwitchData> switchDic;

    public void InitializeDic()
    {
        switchDic = new Dictionary<string, SwitchData>();

        for (int i = 0; i < gameData.switchList.Count; i++) {
            switchDic.Add(gameData.switchList[i].name, gameData.switchList[i]);
        }
    }
#endregion Switch Data
}
