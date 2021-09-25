using UnityEngine;

public class LevelManager : MonoBehaviour
{
#region Variables
    public int stageNum;

    public int maxStage;    // 총 레벨 수

    public Sprite starOn;   // 별 획득했을때 이미지
    public Sprite clearStar;    // 별 전부 획득했을 때 이미지

    public int[] starCntArr;    // 레벨마다 별 획득한 개수 저장
    public int[] highScoreArr;  // 레벨마다 하이스코어 저장
    public bool[] allClearArr;  // 레벨마다 별 전부 회득 + 클리어 했는지 여부 저장
    public int unlimitScore;    // 무한 모드 하이스코어 저장
#endregion Variables

    private void Awake() {
        // 씬 전환시 계속 생성되는 것 방지
        var obj = FindObjectsOfType<LevelManager>();
        if (obj.Length == 1) {
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }

        starCntArr = new int[maxStage]; // 레벨 개수
        highScoreArr = new int[maxStage]; // 레벨 개수
        allClearArr = new bool[maxStage]; // 레벨 개수
    }
}
