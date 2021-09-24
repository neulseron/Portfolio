using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class DataManager : MonoBehaviour
{
    //=====================================================
    // ** 싱글톤 **
    static GameObject _container;
    static GameObject Container
    {
        get {
            return _container;
        }
    }
    static DataManager _instance;
    public static DataManager Instance
    {
        get {
            if (!_instance) {
                _container = new GameObject();
                _container.name = "DataManager";
                _instance = _container.AddComponent(typeof(DataManager)) as DataManager;
                DontDestroyOnLoad(_container);
            }
            return _instance;
        }
    }
    //=====================================================
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

    public Dictionary<string, SwitchData> switchDic;

    public void InitializeDic()
    {
        switchDic = new Dictionary<string, SwitchData>();

        for (int i = 0; i < gameData.switchList.Count; i++) {
            switchDic.Add(gameData.switchList[i].name, gameData.switchList[i]);
        }
    }

/* 자동저장
    private void OnApplicationQuit() {
        SaveGameData();
    }
    */


}
