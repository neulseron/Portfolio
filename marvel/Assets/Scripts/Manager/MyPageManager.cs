using System.Collections;
using System.Collections.Generic;
using System.IO;

using DImage = System.Drawing.Image;
using Bitmap = System.Drawing.Bitmap;
using ImageFormat = System.Drawing.Imaging.ImageFormat;

using UnityEngine;
using UnityEngine.UI;
using BackEnd;
using LitJson;


public class MyPageManager : MonoBehaviour
{
    [Header("Profile")]
    public Image ProfileImg;
    public Text NicknameTxt;


    [Header("Modify")]
    public InputField NewNickName;
    public GameObject ModifyPanel;
    public Image NewProfileImg;
    byte[] imgBytes = null;


    [Header("Following Heros")]
    public GameObject[] HeroBtns;
    public Text NowFollowingNum;
    int following = 0;


#region Unity Methods
    void Start() {
        LoadProfile();
        LoadInterestData();
    }
#endregion Unity Methods


#region Load Profile
    void LoadProfile()
    {
        /* nickname */
        NicknameTxt.text = ApplicationManager.Instance.GetNickName();

        /* profile image */
        var bro = Backend.GameData.GetMyData("ProfileImg", new Where());

        if (!bro.IsSuccess()) {
            Debug.Log("불러오기 실패");
            return;
        }

        var data = bro.Rows()[0]["profile"][0];
        byte[] byteData = System.Convert.FromBase64String((string)data);
        Texture2D texture = new Texture2D(512, 512, TextureFormat.ARGB32, false, true);
        texture.LoadImage(byteData);

        Rect rect = new Rect(0, 0, texture.width, texture.height);
        ProfileImg.sprite = Sprite.Create(texture, rect, new Vector2(1f, texture.height / (float)texture.width));
    }

    void LoadInterestData()
    {
        var bro = Backend.GameData.GetMyData("Interest", new Where());

        if (!bro.IsSuccess()) {
            Debug.Log("불러오기 실패");
            return;
        } else if (bro.GetReturnValuetoJSON()["rows"].Count <= 0) {
            Debug.Log("조회할 데이터 없음");
            return;
        }

        foreach (var btn in HeroBtns) {
            var data = bro.Rows()[0][btn.name][0];
            btn.SetActive((bool)data);
            if ((bool)data) ++following;
        }

        NowFollowingNum.text = following.ToString();
    }
#endregion Load Profile
    

#region Modify Profile
    public void OpenModifyPanel()
    {
        ModifyPanel.SetActive(true);

        NewNickName.text = ApplicationManager.Instance.GetNickName();
        NewProfileImg.sprite = ProfileImg.sprite;
    }

    public void CloseModifyPanel()
    {
        ModifyPanel.SetActive(false);
    }

    public void BtnModifySave()
    {
        ModifyProfile();

        LoadProfile();
        CloseModifyPanel();
    }

    public void UpdateToNewImg()
    {
        /* 변경할 이미지 미리보기 */
        if (NativeGallery.IsMediaPickerBusy())  return;
        PickImageFromGallery(512);
    }

    void PickImageFromGallery(int maxSize)
    {
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery( (path) =>
        {
            //Debug.Log("Image path : " + path);
            if (path != null) {
                Texture2D texture = NativeGallery.LoadImageAtPath(path, maxSize, false);
                if (texture == null) {
                    Debug.Log("Couldn't load texture from " + path);
                    return;
                }

                Rect rect = new Rect(0, 0, texture.width, texture.height);
                NewProfileImg.sprite = Sprite.Create(texture, rect, new Vector2(1f, texture.height / (float)texture.width));
                
                imgBytes = texture.EncodeToPNG();
            }
        } );

        //Debug.Log("Permission result : " + permission);
    }

    void ModifyProfile()
    {
        /* 닉네임 변경 */
        Backend.BMember.UpdateNickname(NewNickName.text);

        /* 실제 프로필 이미지를 새 이미지로 변경 */
        Param param = new Param();
        param.Add("profile", imgBytes);

        ApplicationManager.Instance.UpdateData("ProfileImg", param);
    }

#endregion Modify Profile


#region Modify Following
    public void ModifyFollowingInterest()
    {
        ApplicationManager.Instance.isFromMyPage = true;
        ApplicationManager.Instance.LoadNextScene("Interest");
    }
#endregion Modify Following

    public void LoadMenuScene(int idx)
    {
        ApplicationManager.Instance.LoadNextScene(idx);
    }
}
