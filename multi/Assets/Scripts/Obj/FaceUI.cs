using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class FaceUI : MonoBehaviourPunCallbacks, IPunObservable
{
#region Variables
    public GameObject player;
    public Image face;
    public Image hp;
    public int win;
    public Image[] score;
    public PhotonView PV;
#endregion Variables


#region Photon Methods
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting) { // isMine인 경우
            stream.SendNext(hp.fillAmount);
            stream.SendNext(face.color.a);
        } else {
            hp.fillAmount = (float)stream.ReceiveNext();
            face.color = new Color(face.color.r, face.color.g, face.color.b, (float)stream.ReceiveNext());
        }
    }
#endregion Photon Methods


#region Unity Methods
    public override void OnEnable() => InitFace();
#endregion Unity Methods


#region Methods
    public void InitFace()
    {
        PV.RPC("InitWin", RpcTarget.All, -1);
        hp.fillAmount = 1f;
        PV.RPC("InitScoreEx", RpcTarget.All);
    }

    #region HP
    public void Hit()
    {
        hp.fillAmount -= 0.1f;
        if (hp.fillAmount <= 0) {
            transform.Find("face").GetComponent<Image>().color = new Color(1, 1, 1, 0.4f);

            Invoke("Respawn", 4f);
        }
    }

    void Respawn()
    {
        hp.fillAmount = 1f;
        transform.Find("face").GetComponent<Image>().color = new Color(1, 1, 1, 1);

        player.SetActive(true);
    }
    #endregion HP

    #region Score
    [PunRPC]
    void InitScoreEx()
    {
        score[0].color = new Color(score[0].color.r, score[0].color.g, score[0].color.b, 0);
        score[1].color = new Color(score[1].color.r, score[1].color.g, score[1].color.b, 0);
    }

    public void Score()
    {
        PV.RPC("ScoreOn", RpcTarget.All);
    }

    [PunRPC]
    void ScoreOn()
    {
        if (win < 1)
            PV.RPC("InitWin", RpcTarget.All, win + 1);

        score[win].color = new Color(score[win].color.r, score[win].color.g, score[win].color.b, 1);

        
        if (win == 1) {
            PV.RPC("InitWin", RpcTarget.All, -1);
            Hashtable CP = PhotonNetwork.CurrentRoom.CustomProperties;
            CP["win"] = true;
            PhotonNetwork.CurrentRoom.SetCustomProperties(CP);
        }
    }

    [PunRPC]
    void InitWin(int w) => this.win = w;    
    #endregion Score
#endregion Methods
}
