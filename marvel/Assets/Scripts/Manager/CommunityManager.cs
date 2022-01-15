using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BackEnd;
using LitJson;

public class CommunityManager : MonoBehaviour
{
[Header("Load Post")]
    public GameObject PostView;
    public GameObject PostFramePrefab;
    GameObject[] PostFrames;
    List<Tuple<string, string>> PostLists = new List<Tuple<string, string>>();

[Header("Write Post")]
    public GameObject WritingPanel;
    public InputField WriteWriter;
    public InputField WriteTitle;
    public InputField WriteContent;

[Header("Read Post")]
    public GameObject ReadingPanel;
    public Text ReadWriter;
    public Text ReadTitle;
    public Text ReadContent;

#region Unity Methods
    void Awake() 
    {
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
        PostLists.Clear();

        var bro = Backend.GameData.Get("Post", new Where());
        if (!bro.IsSuccess()) {
            Debug.LogError("불러오기 실패");
            return;
        } else if (bro.GetReturnValuetoJSON()["rows"].Count <= 0) {
            Debug.Log("조회할 데이터 없음");
            return;
        }

        for(int i = 0; i < bro.Rows().Count && i < 10; ++i) {
            GameObject newPost = EnablePostFrame();
            Text newWriter = newPost.transform.Find("WriterTxt").gameObject.GetComponent<Text>();
            Text newTitle = newPost.transform.Find("BtnContent").Find("TitleTxt").gameObject.GetComponent<Text>();
            Text newContent = newPost.transform.Find("BtnContent").Find("ContentTxt").gameObject.GetComponent<Text>();

            var title = bro.Rows()[i]["title"]["S"].ToString();
            var content = bro.Rows()[i]["content"]["S"].ToString();
            var writer = bro.Rows()[i]["nickname"]["S"].ToString();

            var inDate = bro.Rows()[i]["inDate"]["S"].ToString();
            var ownerInDate = bro.Rows()[i]["owner_inDate"]["S"].ToString();
            PostLists.Add(new Tuple<string, string>(inDate, ownerInDate));

            /*
            var getWriterNickname = Backend.Social.GetUserInfoByInDate(writerInDate);
            string writer = getWriterNickname.GetReturnValuetoJSON()["row"]["nickname"].ToString();
            */

            newWriter.text = writer;
            newTitle.text = title;
            newContent.text = content;
        }

        Debug.Log("개수 : " + PostLists.Count);
    }

#region Read Post
    public void LoadPostFromInDate(int idx)
    {
        ReadingPanel.SetActive(true);
        /**/
        var bro = Backend.GameData.Get("Post", new Where());
        if (!bro.IsSuccess()) {
            Debug.LogError("불러오기 실패");
            return;
        } else if (bro.GetReturnValuetoJSON()["rows"].Count <= 0) {
            Debug.Log("조회할 데이터 없음");
            return;
        }

            var writer = bro.Rows()[idx]["nickname"]["S"].ToString();
            var title = bro.Rows()[idx]["title"]["S"].ToString();
            var content = bro.Rows()[idx]["content"]["S"].ToString();

            ReadWriter.text = writer;
            ReadTitle.text = title;
            ReadContent.text = content;
        /**/

        /*
        var bro = Backend.GameData.GetV2("Post", PostLists[idx].Item1, PostLists[idx].Item2);

        if (!bro.IsSuccess()) {
            Debug.LogError("불러오기 실패");
            return;
        } else if (bro.GetReturnValuetoJSON()["rows"].Count <= 0) {
            Debug.LogError("조회할 데이터 없음");
            return;
        }

        var writer = bro.Rows()[0]["nickname"]["S"].ToString();
        var title = bro.Rows()[0]["title"]["S"].ToString();
        var content = bro.Rows()[0]["content"]["S"].ToString();

        ReadWriter.text = writer;
        ReadTitle.text = title;
        ReadContent.text = content;
        */
    }

    public void BtnCloseReadingPanel()
    {
        ReadingPanel.SetActive(false);
    }
#endregion Read Post

#region Write New Post
    public void WriteNewPost()
    {
        WritingPanel.SetActive(true);
        WriteWriter.text = ApplicationManager.Instance.GetNickName();
    }

    public void Posting()
    {
        Param param = new Param();
        param.Add("nickname", WriteWriter.text);
        param.Add("title", WriteTitle.text);
        param.Add("content", WriteContent.text);

        var bro = Backend.GameData.Insert("Post", param);
        if (bro.IsSuccess()) {
            Debug.Log("포스팅 성공");
        } else {
            Debug.LogError("포스팅 할 수 없음");
        }

        LoadPosts();
        CloseWriteNewPost();
    }

    public void CloseWriteNewPost()
    {
        WriteTitle.text = "";
        WriteContent.text = "";
        WritingPanel.SetActive(false);
    }
#endregion Write New Post

#region Frame Pooling
    void GeneratePostFrames()
    {
        PostFrames = new GameObject[10];
        for (int i = 0; i < PostFrames.Length; i++) {
            PostFrames[i] = Instantiate(PostFramePrefab, PostView.transform);
            PostFrames[i].name = i.ToString();
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

    public void LoadMenuScene(int idx)
    {
        ApplicationManager.Instance.LoadNextScene(idx);
    }
}
