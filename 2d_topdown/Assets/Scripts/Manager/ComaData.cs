using System.Collections.Generic;
using UnityEngine;

public class comaDataForm
{
    public string name;
    public string birth;
    public string dis;
    public string response;
    public string search;
    public string note;
    public string state;
}

public class medicalDataForm
{
    public string name;
    public string birth;
    public string id;
    public string response;
    public List<string> date;
    public List<string> content;
}

public class ComaData : MonoBehaviour
{
#region Variables
    public string inputComaId;
    public bool searchComaEnd = false;

    Dictionary<string, comaDataForm> ComaDic;
    Dictionary<string, medicalDataForm> MedicalDic;
    List<int> idList;
#endregion Variables


#region Unity Methods
    void Start() {
        idList = new List<int>();
        ComaDic = new Dictionary<string, comaDataForm>();
        MedicalDic = new Dictionary<string, medicalDataForm>();

        GenerateIdList();
        GenerateComaData();
        GenerateMeicalData();
    }
#endregion Unity Methods


#region Generate Data
    void GenerateIdList()
    {
        idList.Add(378951); //이수민
        idList.Add(740682); //김채은
        idList.Add(521791); //박형서
        idList.Add(456871); //이지현
        idList.Add(103015); //서권(윤재이)
        idList.Add(214893); //서재이
        idList.Add(154283); //차현석
        idList.Add(175139); //강선우
        idList.Add(256492); //송지운
    }

    void GenerateComaData()
    {
        comaDataForm data = new comaDataForm();
        data.name = "이수민(여)";
        data.birth = "2012.04.08(만 17세)";
        data.dis = "A";
        data.response = "차현석(프로젝트 미아)";
        data.search = "-";
        data.note = "성적표";
        data.state = "입원(수색대기)";
        ComaDic.Add("378951", data);

        data = new comaDataForm();
        data.name = "김채은(여)";
        data.birth = "2012.06.22(만 17세)";
        data.dis = "B";
        data.response = "서재이(프로젝트 미아)";
        data.search = "채지은(수색)";
        data.note = "아버지가 선물해주신 장난감 기차";
        data.state = "입원(수색종료)";
        ComaDic.Add("740682", data);

        data = new comaDataForm();
        data.name = "박형서(남)";
        data.birth = "2014.10.13(만 15세)";
        data.dis = "B";
        data.response = "강선우(프로젝트 미아)";
        data.search = "강선우(프로젝트 미아)";
        data.note = "어릴적 집 앞에 있던 공원";
        data.state = "입원(수색종료)";
        ComaDic.Add("521791", data);

        data = new comaDataForm();
        data.name = "이지현(여)";
        data.birth = "2013.08.11(만 16세)";
        data.dis = "A";
        data.response = "서재이(프로젝트 미아)";
        data.search = "-";
        data.note = "옷장";
        data.state = "입원(수색대기)";
        ComaDic.Add("456871", data);
    }

    void GenerateMeicalData()
    {
        medicalDataForm data = new medicalDataForm();
        data.name = "이수민(여)";
        data.id = "378951";
        data.birth = "120408-4******";
        data.response = "차현석(프로젝트 미아)";

        data.date = new List<string>();
        data.date.Add("2023.12.11");
        data.date.Add("2026.08.16");
        data.date.Add("2026.08.19");
        data.date.Add("2026.08.21");
        data.date.Add("2026.08.28");
        data.date.Add("2027.02.05");
        data.date.Add("2027.02.06");
        data.date.Add("2027.02.10");
        data.date.Add("2030.02.25");
        data.date.Add("2030.03.06");

        data.content = new List<string>();
        data.content.Add("-프로젝트 미아 등록");
        data.content.Add("-수면제 과다복용(C구역 오픈)\n-왼쪽 손목 자해흔");
        data.content.Add("-수색종료\n-의식 미회복");
        data.content.Add("-의식 회복\n-재검사 결과 우울수치, 사회성을 비롯한 모든 항목 양호");
        data.content.Add("-이상 없음, 퇴원조치");
        data.content.Add("-자해증상 재발\n-의식 없으나 구역 오픈되지 않음");
        data.content.Add("-의식회복\n-검사 결과 이상 없음");
        data.content.Add("-특이사항 발견하지 못함, 퇴원조치");
        data.content.Add("-자해증상 재발\n-입원조치(송지운)\n-발작증세로 진정제 투여");
        data.content.Add("-자해성 발작 재발\n-진정제 투여");
        MedicalDic.Add("378951", data);
        //------------------------------------------------------
        data = new medicalDataForm();
        data.name = "김채은(여)";
        data.id = "740682";
        data.birth = "120622-4******";
        data.response = "서재이(프로젝트 미아)";

        data.date = new List<string>();
        data.date.Add("2020.07.21");
        data.date.Add("2030.03.04");

        data.content = new List<string>();
        data.content.Add("-프로젝트 미아 등록");
        data.content.Add("-선아고등학교 외벽 붕괴(B구역 오픈)\n-뇌진탕, 왼쪽 복사뼈 골절 및 복부 상처");
        MedicalDic.Add("740682", data);
        //------------------------------------------------------
        data = new medicalDataForm();
        data.name = "박형서(남)";
        data.id = "521791";
        data.birth = "141013-3******";
        data.response = "강선우(프로젝트 미아)";

        data.date = new List<string>();
        data.date.Add("2021.06.02");
        data.date.Add("2030.03.04");

        data.content = new List<string>();
        data.content.Add("-프로젝트 미아 등록");
        data.content.Add("-선아고등학교 건물 외벽 붕괴(B구역 오픈)\n-뇌진탕, 갈비뼈 골절");
        MedicalDic.Add("521791", data);
        //------------------------------------------------------
        data = new medicalDataForm();
        data.name = "이지현(여)";
        data.id = "456871";
        data.birth = "130811-4******";
        data.response = "서재이(프로젝트 미아)";

        data.date = new List<string>();
        data.date.Add("2025.04.22");
        data.date.Add("2030.03.04");

        data.content = new List<string>();
        data.content.Add("-프로젝트 미아 등록");
        data.content.Add("-선아고등학교 건물 외벽 붕괴(A구역 오픈)\n-뇌진탕, 왼쪽 쇄골 골절");
        MedicalDic.Add("456871", data);
        //------------------------------------------------------
        data = new medicalDataForm();
        data.name = "윤재이(여)";
        data.id = "103015";
        data.birth = "010506-4******";
        data.response = "서상호(프로젝트 미아)";

        data.date = new List<string>();
        data.date.Add("2007.03.10");
        data.date.Add("2007.04.03");
        data.date.Add("2007.04.07");
        data.date.Add("2007.04.09");
        data.date.Add("2007.04.23");
        data.date.Add("2007.05.16");
        data.date.Add("2010.01.03");

        data.content = new List<string>();
        data.content.Add("[데이터 초기화]");
        data.content.Add("-터널 차량 6중 추돌 사고(A구역 오픈), 동행자 전원 사망\n-전신 타박상\n-프로젝트 미아 등록(예약자)");
        data.content.Add("-의식 불완전 회복, 환청 증세\n-기억 온전하지 못함. 이름과 나이를 다르게 이야기 함.");
        data.content.Add("-담당자 변경(서상호)");
        data.content.Add("-의식 돌아오는 시간 증가했으나 환청 증세 차도 없음\n-두통 호소");
        data.content.Add("-사고 이전 기억 없음\n-발작 증세");
        data.content.Add("-관할 이전");
        MedicalDic.Add("103015", data);
        //------------------------------------------------------
        data = new medicalDataForm();
        data.name = "서재이(여)";
        data.id = "214893";
        data.birth = "010506-4******";
        data.response = "-";

        data.date = new List<string>();
        data.date.Add("2025.02.03");

        data.content = new List<string>();
        data.content.Add("-프로젝트 미아 등록(기등록자)");
        MedicalDic.Add("214893", data);
        //------------------------------------------------------
        data = new medicalDataForm();
        data.name = "차현석(남)";
        data.id = "154283";
        data.birth = "980315-1******";
        data.response = "-";

        data.date = new List<string>();
        data.date.Add("2008.10.11");

        data.content = new List<string>();
        data.content.Add("-프로젝트 미아 등록");
        MedicalDic.Add("154283", data);
        //------------------------------------------------------
        data = new medicalDataForm();
        data.name = "강선우(남)";
        data.id = "175139";
        data.birth = "011224-3******";
        data.response = "-";

        data.date = new List<string>();
        data.date.Add("2025.02.03");

        data.content = new List<string>();
        data.content.Add("-프로젝트 미아 등록");
        MedicalDic.Add("175139", data);
        //------------------------------------------------------
        data = new medicalDataForm();
        data.name = "송지운(여)";
        data.id = "256492";
        data.birth = "961128-2******";
        data.response = "-";

        data.date = new List<string>();
        data.date.Add("2023.06.23");

        data.content = new List<string>();
        data.content.Add("-프로젝트 미아 등록");
        MedicalDic.Add("256492", data);
    }
#endregion Generate Data


#region Methods
    public bool ChkID()
    {
        int id = int.Parse(inputComaId);

        foreach (int i in idList) {
            if (i == id)
                return true;
        }

        return false;
    }

    public comaDataForm comaID(string _id)
    {
        return ComaDic[_id];
    }

    public medicalDataForm medicalID(string _id)
    {
        return MedicalDic[_id];
    }
#endregion Methods
}
