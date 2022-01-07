using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using BackEnd;

public class CommunityManager : MonoBehaviour
{
[Header("Post Post")]
    public GameObject PostView;
    public GameObject PostFramePrefab;
    GameObject[] PostFrames;

[Header("Write Post")]
    public Text PostWriter;
    public InputField PostTitle;
    public InputField PostContent;
    public GameObject WritingPanel;

#region Unity Methods
    void Awake() {
        GeneratePostFrames();
    }

    void Start()
    {
        LoadPosts();
    }

#endregion Unity Methods

    void LoadPosts()
    {
        DisablePostFrame();

        var bro = Backend.GameData.Get("Post", new Where());

        if (!bro.IsSuccess()) {
            Debug.Log("불러오기 실패");
            //Error(bro.GetErrorCode(), "loadData");
            return;
        } else if (bro.GetReturnValuetoJSON()["rows"].Count <= 0) {
            Debug.Log("조회할 데이터 없음");
            //Debug.Log(bro);
            return;
        }

        for(int i = 0; i < bro.Rows().Count && i < 10; ++i) {
            GameObject newPost = EnablePostFrame();

            Text newWriter = newPost.transform.Find("WriterTxt").gameObject.GetComponent<Text>();
            Text newTitle = newPost.transform.Find("BtnContent").Find("TitleTxt").gameObject.GetComponent<Text>();
            Text newContent = newPost.transform.Find("BtnContent").Find("ContentTxt").gameObject.GetComponent<Text>();

            var writerInDate = bro.Rows()[i]["owner_inDate"]["S"].ToString();
            var getWriterNickname = Backend.Social.GetUserInfoByInDate(writerInDate);

            string writer = getWriterNickname.GetReturnValuetoJSON()["row"]["nickname"].ToString();
            var title = bro.Rows()[i]["title"]["S"].ToString();
            var content = bro.Rows()[i]["content"]["S"].ToString();

            newWriter.text = writer;
            newTitle.text = title;
            newContent.text = content;
        }
    }

    public void WritePost()
    {
        WritingPanel.SetActive(true);
    }

    public void Posting()
    {
        Param param = new Param();
        param.Add("title", PostTitle.text);
        param.Add("content", PostContent.text);

        var bro = Backend.GameData.Insert("Post", param);
        if (bro.IsSuccess()) {
            Debug.Log("포스팅 성공");
        } else {
            Debug.LogError("포스팅 할 수 없음");
        }

        LoadPosts();
        WritingPanel.SetActive(false);
    }

#region Frame Pooling
    void GeneratePostFrames()
    {
        PostFrames = new GameObject[10];
        for (int i = 0; i < PostFrames.Length; i++) {
            PostFrames[i] = Instantiate(PostFramePrefab, PostView.transform);
            PostFrames[i].SetActive(false);
        }
    }

    GameObject EnablePostFrame()
    {
        for (int i = 0; i < PostFrames.Length; i++) {
            if (!PostFrames[i].activeSelf) {
                PostFrames[i].SetActive(true);
                return PostFrames[i];
            }
        }

        return null;
    }

    void DisablePostFrame()
    {
        for (int i = 0; i < PostFrames.Length; i++)
            PostFrames[i].SetActive(false);
    }
#endregion Frame Pooling

    public void LoadMainScene()
    {
        SceneManager.LoadScene(2);
    }

    public void LoadWorldViewScene()
    {
        SceneManager.LoadScene(3);
    }

    public void LoadSeriesScene()
    {
        SceneManager.LoadScene(4);
    }

    public void LoadMypageScene()
    {
        SceneManager.LoadScene(6);
    }
}
