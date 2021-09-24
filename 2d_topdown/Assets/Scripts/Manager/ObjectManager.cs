using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    // ** Prefab **
    public GameObject[] PlayerPrefabs;
    public GameObject[] NPCPrefabs;
    public GameObject[] RandomNPCPrefabs;

    // ** Save Arr **
    GameObject[] players;
    GameObject[] npcs;

    GameObject[] randomNpcs0;
    GameObject[] randomNpcs1;
    GameObject[] randomNpcs2;
    GameObject[] randomNpcs3;
    GameObject[] randomNpcs4;
    GameObject[] randomNpcs5;
    GameObject[] randomNpcs6;
    GameObject[] randomNpcs7;
    GameObject[] randomNpcs8;


    GameObject[] targetPool;
    GameObject target;

    public GameManager gameManager;
    public SwitchManager switchManager;


    // ** 초기화 **
    void Awake() {
        players = new GameObject[4];
        npcs = new GameObject[13];

        randomNpcs0 = new GameObject[3];
        randomNpcs1 = new GameObject[3];
        randomNpcs2 = new GameObject[3];
        randomNpcs3 = new GameObject[3];
        randomNpcs4 = new GameObject[3];
        randomNpcs5 = new GameObject[3];
        randomNpcs6 = new GameObject[3];
        randomNpcs7 = new GameObject[3];
        randomNpcs8 = new GameObject[3];

        Generate();
    }

    void Generate()
    {
        // ** player
        PlayerAction playerLogic;
        for (int i = 0; i < players.Length; i++) {
            players[i] = Instantiate(PlayerPrefabs[i]);

            playerLogic = players[i].GetComponent<PlayerAction>();
            playerLogic.gameManager = gameManager;
            playerLogic.switchManager = switchManager;

            players[i].SetActive(false);
        }

        for (int i = 0; i < npcs.Length; i++) {
            npcs[i] = Instantiate(NPCPrefabs[i]);
            npcs[i].SetActive(false);
        }

        for (int i = 0; i < randomNpcs0.Length; i++) {
            randomNpcs0[i] = Instantiate(RandomNPCPrefabs[0]);
            randomNpcs0[i].SetActive(false);
        }
        for (int i = 0; i < randomNpcs1.Length; i++) {
            randomNpcs1[i] = Instantiate(RandomNPCPrefabs[1]);
            randomNpcs1[i].SetActive(false);
        }
        for (int i = 0; i < randomNpcs2.Length; i++) {
            randomNpcs2[i] = Instantiate(RandomNPCPrefabs[2]);
            randomNpcs2[i].SetActive(false);
        }
        for (int i = 0; i < randomNpcs3.Length; i++) {
            randomNpcs3[i] = Instantiate(RandomNPCPrefabs[3]);
            randomNpcs3[i].SetActive(false);
        }
        for (int i = 0; i < randomNpcs4.Length; i++) {
            randomNpcs4[i] = Instantiate(RandomNPCPrefabs[4]);
            randomNpcs4[i].SetActive(false);
        }
        for (int i = 0; i < randomNpcs5.Length; i++) {
            randomNpcs5[i] = Instantiate(RandomNPCPrefabs[5]);
            randomNpcs5[i].SetActive(false);
        }
        for (int i = 0; i < randomNpcs6.Length; i++) {
            randomNpcs6[i] = Instantiate(RandomNPCPrefabs[6]);
            randomNpcs6[i].SetActive(false);
        }
        for (int i = 0; i < randomNpcs7.Length; i++) {
            randomNpcs7[i] = Instantiate(RandomNPCPrefabs[7]);
            randomNpcs7[i].SetActive(false);
        }
        for (int i = 0; i < randomNpcs8.Length; i++) {
            randomNpcs8[i] = Instantiate(RandomNPCPrefabs[8]);
            randomNpcs8[i].SetActive(false);
        }
    }

    public GameObject MakeObj(string type)
    {
        switch (type) {
            case "P_Jaeha"      :       target = players[0];    break;
            case "P_Jaei"       :       target = players[1];    break;
            case "P_Sunwoo"     :       target = players[2];    break;
            case "P_HyeonSeok"  :       target = players[3];    break;
            case "N_Jaeha"      :       target = npcs[0];       break;
            case "N_Jaei"       :       target = npcs[1];       break;
            case "N_Sunwoo"     :       target = npcs[2];       break;
            case "N_Sumin1"     :       target = npcs[3];       break;
            case "N_Sumin2"     :       target = npcs[4];       break;
            case "N_HS_S"       :       target = npcs[5];       break;
            case "N_HS_R"       :       target = npcs[6];       break;
            case "N_SMmom"      :       target = npcs[7];       break;
            case "N_SMdad"      :       target = npcs[8];       break;
            case "N_Director"   :       target = npcs[9];       break;
            case "N_Jiun"       :       target = npcs[10];       break;
            case "N_Jihyeon"    :       target = npcs[11];       break;
            case "N_Sumin2Coma" :       target = npcs[12];       break;
        }

        target.SetActive(true);

        return target;
    }

    public GameObject MakeNPC(string type)
    {
        switch (type) {
            case "NPC0"     :       targetPool = randomNpcs0;      break;
            case "NPC1"     :       targetPool = randomNpcs1;      break;
            case "NPC2"     :       targetPool = randomNpcs2;      break;
            case "NPC3"     :       targetPool = randomNpcs3;      break;
            case "NPC4"     :       targetPool = randomNpcs4;      break;
            case "NPC5"     :       targetPool = randomNpcs5;      break;
            case "NPC6"     :       targetPool = randomNpcs6;      break;
            case "NPC7"     :       targetPool = randomNpcs7;      break;
            case "NPC8"     :       targetPool = randomNpcs8;      break;
        }

        for (int i = 0; i < targetPool.Length; i++) {
            if (!targetPool[i].activeSelf) {
                targetPool[i].SetActive(true);
                return targetPool[i];
            }
        }

        return null;
    }

    public GameObject[] GetPool(string type)
    {
        
        
        return targetPool;
    }
}
