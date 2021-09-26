using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    //=====================================================
    // ** 매니저 **
    //=====================================================
    // ** 컴포넌트 **
    public string myName;
    //------------------------------------------------------
    Animator animator;
    Rigidbody2D rigid;
    //=====================================================
    // ** 움직임 **
    public float speed;
    float h, v;
    bool isHorizonMove;
    Vector3 dirVec;
    //------------------------------------------------------
    public Vector3[] wayPoint;
    int waypointsIndex = 0;
    bool isHorizontal = false;
    public bool isOn = false;
    public bool isOff = false;
    //=====================================================
    // ** 전역변수 **
    GameObject scanObj;
    Door to;
    string tmpName;
    //=====================================================

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    
    void Update()
    {
        if (isOn)   CustomMove();
        else        Move();
            
        // ** 오브젝트 + 스페이스 바 **
        if (Input.GetButtonDown("Jump")) {
            if (scanObj != null && !ChkDontMove()) {
                switch(scanObj.tag) {
                    case "Door":
                            to = scanObj.GetComponent<Door>();

                            if (to.anim != null) { // ** 열리는 문
                                to.anim.SetTrigger("isOpen");
                                if (to.AnimationDone()) {
                                    Invoke("DelayCall", 0.5f);
                                }
                            } else // ** 구멍
                                GameManager.Instance.ChangeMap(to.moveToMapIndex, myName, to.movePos, to.turn, to.showMap);
                        break;
                    case "Switch":
                        if (!SwitchManager.Instance.ing && !SwitchManager.Instance.switchdata[scanObj.name].off) {
                            tmpName = scanObj.name;
                            SwitchManager.Instance.switchdata[scanObj.name].on = true;
                            StartCoroutine(WaitSwitchOn());
                        }
                        break;
                    case "Item":
                            //gameManager.AddItem();
                        break;
                }
                
                ChkAction();
            } else {
                ChkAction();
            }
        }
    }
    
    void DelayCall()
    {
        GameManager.Instance.ChangeMap(to.moveToMapIndex, myName, to.movePos, to.turn, to.showMap);
    }

    IEnumerator WaitSwitchOn() 
    {
        yield return new WaitForSeconds(0.2f);
        if (SwitchManager.Instance.switchdata[tmpName].on) {
            SwitchManager.Instance.switchdata[tmpName].on = false;
        }
        
        yield return null;
    }

    void ChkAction()
    {
        if (GameManager.Instance.isInterAction) {
            GameManager.Instance.InterAction();
        } else if (GameManager.Instance.isSystem) {
            GameManager.Instance.SystemAction();
        } else if (GameManager.Instance.isTalking) {
            GameManager.Instance.Action();
        }
    }

    bool ChkDontMove()
    {
        if (GameManager.Instance.isTalking || GameManager.Instance.isInterAction || GameManager.Instance.isSystem || GameManager.Instance.isSelecting || GameManager.Instance.dontMove)
            return true;
        
        return false;
    }

    void FixedUpdate() 
    {
        // Move
        Vector2 moveVec = isHorizonMove ? new Vector2(h, 0) : new Vector2(0, v);
        rigid.velocity = moveVec * speed;

        // Ray (조사이벤트)
        Vector3 rayPos = new Vector3(rigid.position.x, rigid.position.y - 0.3f, 0);
        //Debug.DrawRay(rayPos, dirVec * 1f, Color.green);
        RaycastHit2D rayHit = Physics2D.Raycast(rayPos, dirVec, 2f, LayerMask.GetMask("Object"));
        if (rayHit.collider != null) {  // 스캔한 오브젝트가 있으면
            scanObj = rayHit.collider.gameObject;
            GameManager.Instance.scanObj = scanObj;
        } else
            scanObj = null;
    }

    void Move()
    {
        // ** 대화 재생중이면 움직이지 않게끔
        h = ChkDontMove() ? 0 : Input.GetAxisRaw("Horizontal");
        v = ChkDontMove() ? 0 : Input.GetAxisRaw("Vertical");

        // ** check button down & up
        bool hDown = ChkDontMove() ? false : Input.GetButtonDown("Horizontal");
        bool vDown = ChkDontMove() ? false : Input.GetButtonDown("Vertical");
        bool hUp = ChkDontMove() ? false : Input.GetButtonUp("Horizontal");
        bool vUp = ChkDontMove() ? false : Input.GetButtonUp("Vertical");

        if (hDown)
            isHorizonMove = true;
        else if (vDown)
            isHorizonMove = false;
        else if (hUp || vUp)
            isHorizonMove = h != 0;

        // ** animatoration
        if(animator.GetInteger("hAxisRaw") != h) {
            animator.SetBool("isChange", true);
            animator.SetInteger("hAxisRaw", (int)h);
        }
        else if(animator.GetInteger("vAxisRaw") != v) {
            animator.SetBool("isChange", true);
            animator.SetInteger("vAxisRaw", (int)v);
        } else
            animator.SetBool("isChange", false);

        // ** 조사 Direction
        if (vDown && v == 1) // 위쪽
            dirVec = Vector3.up;
        else if (vDown && v == -1) // 위쪽
            dirVec = Vector3.down;
        if (hDown && h == 1) // 위쪽
            dirVec = Vector3.right;
        if (hDown && h == -1) // 위쪽
            dirVec = Vector3.left;
    }

    public void CustomMove()
    {
        rigid.velocity = Vector2.zero;

        Vector3 currPos = transform.position;

        if (waypointsIndex < wayPoint.Length) {
            Vector3 nextPos = wayPoint[waypointsIndex];
            Vector3 dir = nextPos - currPos;

            // ** 애니메이션 방향 **
            if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y)) { // 수평
                isHorizontal = true;
            } else {
                isHorizontal = false;
            }

            // ** animation **
            int x = 0, y = 0;
            if (dir.x < 0)      x = -1;
            else if (dir.x > 0) x = 1;
            if (dir.y < 0)      y = -1;
            else if (dir.y > 0) y = 1;

            if (isHorizontal && (animator.GetInteger("hAxisRaw") != x)) {
                animator.SetBool("isChange", true);
                animator.SetInteger("hAxisRaw", x);
                animator.SetInteger("vAxisRaw", 0);
            } else if (!isHorizontal && (animator.GetInteger("vAxisRaw") != y)) {
                animator.SetBool("isChange", true);
                animator.SetInteger("vAxisRaw", y);
                animator.SetInteger("hAxisRaw", 0);
            } else
                animator.SetBool("isChange", false);

            // ** move **
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(currPos, nextPos, step);

            //Debug.Log(Vector3.Distance(nextPos, currPos));
            if (Vector3.Distance(nextPos, currPos) <= 0.1f)
                waypointsIndex++;
        } else {
            isOn = false;
            isOff = true;

            animator.SetInteger("vAxisRaw", 0);
            animator.SetInteger("hAxisRaw", 0);

            waypointsIndex = 0;
        }
    }


    // * 자동 실행
    void OnTriggerEnter2D(Collider2D other) {
        switch (other.tag) {
            case "AutoSwitch":
                if (SwitchManager.Instance.switchdata[scanObj.name].off || SwitchManager.Instance.ing)     
                    return;
                else {                    
                    tmpName = scanObj.name;
                    SwitchManager.Instance.switchdata[scanObj.name].on = true;
                }
                
                StartCoroutine(WaitSwitchOn());
                break;
        }
        
    }

    public void Turn(string _dir)
    {
        animator.SetBool("isChange", true);
        
        switch (_dir) {
            case "up":
                animator.SetInteger("vAxisRaw", 1);
                break;
            case "down":
                animator.SetInteger("vAxisRaw", -1);
                break;
            case "left":
                animator.SetInteger("hAxisRaw", -1);
                break;
            case "right":
                animator.SetInteger("hAxisRaw", 1);
                break;
        }

        Invoke("TurnFace", 0.2f);
    }

    void TurnFace() {
        animator.SetInteger("vAxisRaw", 0);
        animator.SetInteger("hAxisRaw", 0);
    }

    public void SetWaypoint(int _size, params Vector3[] _val)
    {
        wayPoint = new Vector3[_size];

        for (int i = 0; i < _size; i++) {
            wayPoint[i] = _val[i];
        }
    }
}
