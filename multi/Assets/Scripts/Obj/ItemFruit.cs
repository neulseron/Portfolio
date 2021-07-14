using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

public class ItemFruit : MonoBehaviourPunCallbacks, IPunObservable
{
    AudioSource audioSource;
    public AudioClip audioItem;

    public PhotonView PV;
    Animator animator;
    float exitTime = 0.8f;
    GameObject localPlayer;
    GameObject fruitUI;
    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting) { // isMine인 경우
            stream.SendNext(transform.position);
        } else {
            transform.position = (Vector3)stream.ReceiveNext();
        }
    }

    private void Awake() {
        animator = GetComponent<Animator>();
        localPlayer = (GameObject)(PhotonNetwork.LocalPlayer.TagObject);
        audioSource = GetComponent<AudioSource>();
        fruitUI = GameObject.Find("Canvas").transform.Find("fruitInfo").gameObject;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            if (other.gameObject.name == localPlayer.name) {
                // 획득
                audioSource.clip = audioItem;
                audioSource.Play();
                // 화면에 획득한 과일, 개수 표시
                fruitUI.GetComponent<Image>().sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
                fruitUI.transform.Find("Cnt").GetComponent<Text>().text = "20";
                fruitUI.SetActive(true);

                animator.SetTrigger("isCollected");
                StartCoroutine(CheckAnimationState());

                // 플레이어가 발사할 과일 이미지 설정
                ((GameObject)(PhotonNetwork.LocalPlayer.TagObject)).GetComponent<PlayerController>().fruitName = (gameObject.name).Replace("(Clone)", "");
            }

        }
    }

    public void DestroyFruit()
    {
        PV.RPC("DestroyRPC", RpcTarget.All); 
    }

    [PunRPC]
    void DestroyRPC() => Destroy(gameObject);

    IEnumerator CheckAnimationState()
    {
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("itemFruit"));
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= exitTime);

        PV.RPC("DestroyRPC", RpcTarget.All);
    }
}
