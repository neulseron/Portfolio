using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

using Cinemachine;

public class PlayerController : MonoBehaviourPunCallbacks, IPunObservable, IPunInstantiateMagicCallback
{
    #region Variables
    [Header("Game")]
    public bool decide;
    public bool gameStart;  // master 변수
    public bool ready;


    [Header("Audio")]
    public AudioClip audioJump;
    public AudioClip audioAttack;
    public AudioClip audioDamaged;


    [Header("Move")]
    public float maxSpeed;
    public float jumpPower;


    [Header("UI")]
    public Text nickNameTxt;
    public FaceUI face;


    [Header("Fruit")]
    public string fruitName;
    public bool active;


    [Header("Etc")]
    public PhotonView PV;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator animator;
    AudioSource audioSource;
    Vector3 currPos;
    #endregion Variables



    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting) { // isMine인 경우
            stream.SendNext(transform.position);
            stream.SendNext(face.transform.Find("master").gameObject.activeSelf);
        } else {
            currPos = (Vector3)stream.ReceiveNext();
            face.transform.Find("master").gameObject.SetActive((bool)stream.ReceiveNext());
        }
    }
    
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        info.Sender.TagObject = gameObject;
    }

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        decide = true;
        gameStart = false;
        ready = false;

        fruitName = "";
    }

    public override void OnEnable() {
        if (PhotonNetwork.IsMasterClient)   face.transform.Find("master").gameObject.SetActive(true);
        else    face.transform.Find("master").gameObject.SetActive(false);

        nickNameTxt.text = PV.IsMine ? PhotonNetwork.NickName : PV.Owner.NickName;
        nickNameTxt.color = PV.IsMine ? Color.green : Color.red;

        if (PV.IsMine) {
            var CM = GameObject.Find("CMcamera").GetComponent<CinemachineVirtualCamera>();
            CM.Follow = transform;
            CM.LookAt = transform;
        }

        active = true;
        Reactive();
    }

    void Update() // 단발적 키입력 (60프레임)
    {
        if (PV.IsMine) {
            // Jump(위 방향키)
            if (Input.GetButtonDown("Jump") && !animator.GetBool("isJumping")) {
                rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                animator.SetBool("isJumping", true);
                PlaySound("JUMP");
            }

            // Stop Speed
            if (Input.GetButtonUp("Horizontal")) {
                rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.25f, rigid.velocity.y); // nomalized : 방향 구할때
            }

            // ** 과일 발사
            GameObject fruitUI = GameObject.Find("Canvas").transform.Find("fruitInfo").gameObject;
            if (Input.GetKeyDown(KeyCode.Space) && fruitUI.activeSelf) {  
                PlaySound("ATTACK");

                PhotonNetwork.Instantiate("Bullet/" + fruitName, transform.position + new Vector3(spriteRenderer.flipX ? -1.5f : 1.5f, 0, 0), Quaternion.identity)
                .GetComponent<PhotonView>().RPC("DirRPC", RpcTarget.All, spriteRenderer.flipX ? -1 : 1);

                int cnt = int.Parse(fruitUI.transform.Find("Cnt").GetComponent<Text>().text) - 1;
                fruitUI.transform.Find("Cnt").GetComponent<Text>().text = cnt.ToString();

                if (cnt == 0) {
                    fruitUI.SetActive(false);
                }
            }

            // 방향 전환
            if (Input.GetButton("Horizontal")) {
                float axis = Input.GetAxisRaw("Horizontal");
                PV.RPC("FlipXRPC", RpcTarget.AllBuffered, axis);
            }
            

            // Animation
            if (Mathf.Abs(rigid.velocity.x) < 0.3) 
                animator.SetBool("isWalking", false);
            else
                animator.SetBool("isWalking", true);
        } else if ((transform.position - currPos).sqrMagnitude >= 100) {    // 위치 동기화 (많이 떨어져 있으면 순간이동)
            transform.position = currPos;
        } else {    
            // 별로 많이 안떨어져 있으면 부드럽게 이동
            transform.position = Vector3.Lerp(transform.position, currPos, Time.deltaTime * 10);
        }
    }

    [PunRPC]
    void FlipXRPC(float axis) => spriteRenderer.flipX = axis == -1;


    void FixedUpdate() // 지속적 키 입력 (50프레임)
    {
        if (PV.IsMine) {
            // Move Speed
            float axis = Input.GetAxisRaw("Horizontal");
            rigid.AddForce(Vector2.right * axis, ForceMode2D.Impulse);

            if (rigid.velocity.x > maxSpeed) // right
                rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
            else if (rigid.velocity.x < maxSpeed * (-1)) // left
                rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);

            // Landing Platform (Raycast 사용)
            if (rigid.velocity.y < 0) { // 하강하고 있을 때만
                if (isLandingPlatform()) {
                    animator.SetBool("isJumping", false);
                }
            }
        }
    }

    bool isLandingPlatform() {
        RaycastHit2D raycastHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));
        if (raycastHit.collider != null && raycastHit.distance < 0.96f) {
            return true;
        }
        return false;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (PV.IsMine) {
            // ** 깃발 **
            if (other.gameObject.tag == "Flag" && GameObject.Find("GameManager").GetComponent<GameManager>().gameStart && other.IsTouching(GetComponent<BoxCollider2D>())) {
                if ((bool)PhotonNetwork.CurrentRoom.CustomProperties["flagging"])     return;

                PV.RPC("SetWinner", RpcTarget.All, PhotonNetwork.LocalPlayer.NickName);

                Hashtable CP = PhotonNetwork.CurrentRoom.CustomProperties;
                CP["flagging"] = true;
                PhotonNetwork.CurrentRoom.SetCustomProperties(CP);

                //if ((bool)PhotonNetwork.CurrentRoom.CustomProperties["win"])     return;
                face.Score();
            } 
            // ** 떨어지는 발판 **
            else if (other.gameObject.tag == "FallingPlatform") {
                GetComponent<CapsuleCollider2D>().isTrigger = true;
            }
            // ** 맵 밖으로 떨어졌을 때 **
            else if (other.gameObject.tag == "Reposition") { 
                VelocityZero();
            }
        }
    }

    [PunRPC]
    void SetWinner(string name) => GameObject.Find("Canvas").transform.Find("WinPanel").transform.Find("WinMsg").GetComponent<Text>().text = "[" + name + "]이/가 승리했습니다!";


    void OnTriggerExit2D(Collider2D other) {
        if (PV.IsMine) {
            if (other.gameObject.tag == "FallingPlatform") {
                GetComponent<CapsuleCollider2D>().isTrigger = false;
            }
        }
    }

    #region active 상태 변경
    public void Deactive(bool b)
    {
        PV.RPC("SetActiveRPC", RpcTarget.All, false);
        PV.RPC("ActiveRPC", RpcTarget.All);
    }

    public void Reactive()
    {
        PV.RPC("SetActiveRPC", RpcTarget.All, true);
        PV.RPC("ActiveRPC", RpcTarget.All);

        if (PV.IsMine)
            FindObjectOfType<GameManager>().Respawn(gameObject.transform);
    }

    [PunRPC]
    void ActiveRPC() => gameObject.SetActive(active);

    [PunRPC]
    void SetActiveRPC(bool b) => active = b;
    #endregion

    #region 게임 상태
    public bool isGameStart()   { return this.gameStart; }

    public void SetDecide(bool d) => PV.RPC("SetDecideEx", RpcTarget.All, d);
    [PunRPC]
    public void SetDecideEx(bool d) => this.decide = d;

    public void SetReady(bool r) => PV.RPC("SetReadyEx", RpcTarget.All, r);
    [PunRPC]
    public void SetReadyEx(bool r) => this.ready = r;

    // master
    public void SetGameStart(bool g) => PV.RPC("SetGameStartEx", RpcTarget.All, g);
    [PunRPC]
    public void SetGameStartEx(bool g) => this.gameStart = g;
    #endregion


    public void Hit()
    {
        animator.SetTrigger("getDamaged");
        PlaySound("DAMAGED");
    }

    void PlaySound(string action) {
        switch (action) {
            case "JUMP":
                audioSource.clip = audioJump;
                break;
            case "ATTACK":
                audioSource.clip = audioAttack;
                break;
            case "DAMAGED":
                audioSource.clip = audioDamaged;
                break;
        }

        audioSource.Play();
    }

    

    public void VelocityZero()
    {
        rigid.velocity = Vector2.zero;
    }
}
