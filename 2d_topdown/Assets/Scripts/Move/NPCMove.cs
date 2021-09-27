using UnityEngine;

public class NPCMove : MonoBehaviour
{
#region Variables
    Rigidbody2D rigid;
    Animator animator;
    public Vector3[] wayPoint;

    Vector3 currPos;
    int waypointsIndex = 0;
    public float speed = 2.3f;

    public bool isOn = false;
    public bool isOff = false;
    public bool isTurn = false;
    bool isHorizontal = false;
#endregion Variables

    void Awake() {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update() {
        if (isOn)   Move();
    }

    public void Move() {
        currPos = transform.position;

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

            if (isHorizontal && animator.GetInteger("hAxisRaw") != x) {
                animator.SetBool("isChange", true);
                animator.SetInteger("hAxisRaw", x);
                animator.SetInteger("vAxisRaw", 0);
            } else if (!isHorizontal && animator.GetInteger("vAxisRaw") != y) {
                animator.SetBool("isChange", true);
                animator.SetInteger("vAxisRaw", y);
                animator.SetInteger("hAxisRaw", 0);
            } else
                animator.SetBool("isChange", false);

            // ** move **
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(currPos, nextPos, step);

            if (Vector3.Distance(nextPos, currPos) == 0f)
                waypointsIndex++;
        } else {
            isOn = false;
            isOff = true;

            animator.SetInteger("vAxisRaw", 0);
            animator.SetInteger("hAxisRaw", 0);

            waypointsIndex = 0;
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
        isTurn = false;
    }

    public void SetWaypoint(int _size, params Vector3[] _val)
    {
        wayPoint = new Vector3[_size];

        for (int i = 0; i < _size; i++) {
            wayPoint[i] = _val[i];
        }
    }
}
