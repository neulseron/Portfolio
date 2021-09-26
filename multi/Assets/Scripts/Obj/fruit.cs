using UnityEngine;

using Photon.Pun;

public class fruit : MonoBehaviourPunCallbacks
{
    public PhotonView PV;
    int dir;


    void Start() => Destroy(gameObject, 2.5f);

    void Update()
    {
        transform.Translate(Vector3.right * 9 * Time.deltaTime * dir);
    }

    void OnTriggerEnter2D(Collider2D other) {
        // 맵 구조물에 부딪혔을 때
        if (other.tag == "Platform")
            PV.RPC("DestroyRPC", RpcTarget.All);

        // 상대가 맞은 판정일 때
        if (!PV.IsMine && other.tag == "Player" && other.GetComponent<PhotonView>().IsMine) {
            // faceUI 체력바 감소
            other.GetComponent<PlayerController>().face.Hit();
            // 플레이어 피격 효과
            other.GetComponent<PlayerController>().Hit();

            // 체력 다 떨어졌으면
            if (other.GetComponent<PlayerController>().face.hp.fillAmount <= 0) {
                other.GetComponent<PlayerController>().Deactive(false);
            }

            PV.RPC("DestroyRPC", RpcTarget.All);
        }
    }


    [PunRPC]
    void DirRPC(int dir) => this.dir = dir;

    [PunRPC]
    void DestroyRPC() => Destroy(gameObject);
}
