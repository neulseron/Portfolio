using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchManager : MonoBehaviour
{
#region Singletone
    static SwitchManager instance;
    public static SwitchManager Instance => instance;
#endregion Singletone


    public Dictionary<string, SwitchData> switchdata;
    public bool ing;
    //======================================
    //** Meeting Room **
    public Switch MR_StartExam;
    public Switch MR_turn2;
    public Switch MR_turn3;
    public string L7 = "";
    //======================================
    //** A 1 Maze **
    public string A1MazeAns = "LULRLUUR";
    public string L5 = "";
    //======================================
    //** D 1 **
    public Switch D1_TalkSumin;
    public Switch D1_movePoint;
    //======================================
    
    private void Awake() {
        instance = this;
        
        switchdata = new Dictionary<string, SwitchData>();
    }

    private void Start() {
        AddDic(new SwitchData("Tutorial", false, false));
        AddDic(new SwitchData("Tutorial_Space", false, false));
        AddDic(new SwitchData("Tutorial_DataCom", false, false));
        AddDic(new SwitchData("Tutorial_OffMarkDataCom", false, false));
        AddDic(new SwitchData("Tutorial_SearchCom", false, false));
        AddDic(new SwitchData("Tutorial_OffMarkSearchCom", false, false));

        AddDic(new SwitchData("JR_callDr", false, false));
        AddDic(new SwitchData("A1Maze_InitTalk", false, false));
        AddDic(new SwitchData("A1MazeOn", false, false));
        AddDic(new SwitchData("A1_Closet", false, false));
        AddDic(new SwitchData("A1_End", false, false));
        AddDic(new SwitchData("JR_answerMail", false, false));
        AddDic(new SwitchData("D_Bot", false, false));
        AddDic(new SwitchData("SecondF_movingSM", false, false));
        AddDic(new SwitchData("SecondF_strangeSM", false, false));
        AddDic(new SwitchData("SecondF_appearHS", false, false));
        AddDic(new SwitchData("SecondF_dischargedJH", false, false));
        AddDic(new SwitchData("DR_callHS", false, false));
        AddDic(new SwitchData("JR_ChkMail", false, false));
        AddDic(new SwitchData("D_FirstSearch", false, false));
        AddDic(new SwitchData("Searchcom_signupJihyeon", false, false));

        AddDic(new SwitchData("D_Bed", false, false));
        AddDic(new SwitchData("JR_Bed", false, false));

        AddDic(new SwitchData("D_Monitor", false, false));
        AddDic(new SwitchData("JR_Monitor", false, false));
        AddDic(new SwitchData("SecondF_DataCom", false, false));
        AddDic(new SwitchData("RR_rightScreen", false, false));
        AddDic(new SwitchData("ThirdF_SearchCom", false, false));
        AddDic(new SwitchData("A_Screen", false, false));

        AddDic(new SwitchData("FirstF_RecordRoom", false, false));
        AddDic(new SwitchData("SecondF_SunwooRoomDoor", false, false));
        AddDic(new SwitchData("SecondF_HyeonseokRoomDoor", false, false));
        AddDic(new SwitchData("ThirdF_DirectRoomDoor", false, false));
        AddDic(new SwitchData("ThirdF_MeetingRoomDoor", false, false));
        AddDic(new SwitchData("ThirdF_EmptyRoomDoor", false, false));
        AddDic(new SwitchData("HyeongSeoMaze", false, false));

        AddDic(new SwitchData("FR_BookShelf", false, false));
        AddDic(new SwitchData("FR_BookShelf2", false, false));
        AddDic(new SwitchData("FR_BookShelf3", false, false));
        AddDic(new SwitchData("SecondF_SuMin", false, false));
        AddDic(new SwitchData("SecondF_ChaeEun", false, false));
        AddDic(new SwitchData("SecondF_HyeongSeo", false, false));
        AddDic(new SwitchData("SecondF_JiHyeon", false, false));
        AddDic(new SwitchData("ThirdF_CabinetHyeonSeok", false, false));
        AddDic(new SwitchData("ThirdF_CabinetJiun", false, false));
        AddDic(new SwitchData("ThirdF_CabinetJiEun", false, false));
        AddDic(new SwitchData("ThirdF_CabinetJaeHa", false, false));
        AddDic(new SwitchData("ThirdF_CabinetSuHyeon", false, false));
        AddDic(new SwitchData("ThirdF_CabinetArum", false, false));
        AddDic(new SwitchData("ThirdF_CabinetYeonu", false, false));
        AddDic(new SwitchData("ThirdF_CabinetJihyeong", false, false));
        AddDic(new SwitchData("ThirdF_SearchingJiun", false, false));
        AddDic(new SwitchData("ThirdF_SearchingSunwoo", false, false));
        AddDic(new SwitchData("HR_ChkCalender", false, false));
        AddDic(new SwitchData("HR_Calender", false, false));
        AddDic(new SwitchData("HR_Album", false, false));
        AddDic(new SwitchData("JR_BookShelf", false, false));
        AddDic(new SwitchData("RR_Bookshelf", false, false));
        AddDic(new SwitchData("RR_Bookshelf2", false, false));
        AddDic(new SwitchData("RR_Bookshelf3", false, false));
        AddDic(new SwitchData("RR_rightUsing", false, false));
        AddDic(new SwitchData("RR_leftUsing", false, false));

        AddDic(new SwitchData("SecondF_mail9", false, false));
        AddDic(new SwitchData("SecondF_mail10", false, false));
        AddDic(new SwitchData("SecondF_mailAttached", false, false));
        AddDic(new SwitchData("SecondF_readMail7", false, false));
        AddDic(new SwitchData("SecondF_readMail9", false, false));
        AddDic(new SwitchData("SecondF_readMail10", false, false));

        AddDic(new SwitchData("DataCom_JHsearchEnd", false, false));    // 지현 수색종료
        AddDic(new SwitchData("DataCom_Chk103015", false, false));    // 윤재이 검색완료

        AddDic(new SwitchData("Item_JaeiId", false, false));

        AddDic(new SwitchData("Record_Page1", false, false));
        AddDic(new SwitchData("Record_Page2", false, false));
    }

    public void AddDic(SwitchData _data)
    {
        bool chk = switchdata.ContainsKey(_data.name);

        if (!chk) {
            switchdata.Add(_data.name, _data);
        }
    }
}
