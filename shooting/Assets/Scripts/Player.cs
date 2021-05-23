using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("매니저")]
    public GameManager gameManager;
    public ObjectManager objectManager;


    [Header("Player")]
    Animator animator;
    SpriteRenderer spriteRenderer;
    public float speed;
    public int score;
    int life;
    public Transform firePos;
    public bool isHit;
    public bool isRespawnTime;


    [Header("Item")]
    public int power; // 파워 단계
    int MaxPower;
    public int boom; // 폭탄 개수
    bool isBoomTIme;
    int MaxBoom;
    public GameObject boomEffect;
    public GameObject[] followers;


    [Header("Fire")]
    // 코드 변수
    float curShotDelay;
    public float maxShotDelay;
    string[] powerBullets;


    [Header("Move")]
    bool isTop, isBottom, isLeft, isRight;      // 경계에 닿았는지 여부
    public bool[] joyControl;
    public bool isControl;
    public bool isBtnA, isBtnB;
    //---------------------
    public int currStarNum;
    //=====================
    void Awake() 
    {
        animator = GetComponent<Animator>();
        powerBullets = new string[] { "Power0Bullet", "Power1Bullet", "Power2Bullet", "Power3Bullet", "Power3Bullet", "Power3Bullet", "Power3Bullet" };
        spriteRenderer = GetComponent<SpriteRenderer>();
        MaxBoom = 2;
        MaxPower = 6;
        life = 3;
    }

    void OnEnable() {
        // 시작할때 무적
        Unbeatable();
        Invoke("Unbeatable", 3);
    }

    void Unbeatable()
    {
        isRespawnTime = !isRespawnTime;

        if (isRespawnTime) {
            spriteRenderer.color = new Color(1, 1, 1, 0.5f);
            for (int i = 0; i < followers.Length; i++) {
                followers[i].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
            }
        } else {
            spriteRenderer.color = new Color(1, 1, 1, 1);
            for (int i = 0; i < followers.Length; i++) {
                followers[i].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            }
        }
    }

    void Update()
    {
        Move();
        Fire();
        Reload();
        Boom();
    }

    public void JoyPanel(int type)
    {
        for (int i = 0; i < 9; i++) {
            joyControl[i] = i == type;
        }
    }

    public void JoyDown()    { isControl = true; }

    public void JoyUp()    { isControl = false; }

    void Move()
    {
        float h = 0, v = 0;

        // joypanel
        if (joyControl[0]) { h = -1; v = 1; }
        if (joyControl[1]) { h = 0; v = 1; }
        if (joyControl[2]) { h = 1; v = 1; }
        if (joyControl[3]) { h = -1; v = 0; }
        if (joyControl[4]) { h = 0; v = 0; }
        if (joyControl[5]) { h = 1; v = 0; }
        if (joyControl[6]) { h = -1; v = -1; }
        if (joyControl[7]) { h = 0; v = -1; }
        if (joyControl[8]) { h = 1; v = -1; }

        if ((isRight && h == 1) || (isLeft && h == -1) || !isControl)      
            h = 0;
        if ((isTop && v == 1) || (isBottom && v == -1) || !isControl)      
            v = 0;

        Vector3 curPos = transform.position;
        Vector3 nextPos = new Vector3(h, v, 0) * speed * Time.deltaTime;
        transform.position = curPos + nextPos;

        // animation
        if (h == 1 || h == -1 || h == 0) {
            animator.SetInteger("Input", (int)h);
        }
    }

    public void BtnADown()  { isBtnA = true; }
    public void BtnAUp()    { isBtnA = false; }
    public void BtnBDown()  { isBtnB = true; }
    public void BtnBUp()    { isBtnB = false; }

    void Fire()
    {
        if (!isBtnA)
            return;

        if (curShotDelay < maxShotDelay)
            return;

        // power
        GameObject bullet = objectManager.MakeObj(powerBullets[power]);
        bullet.transform.position = firePos.position;

        Rigidbody2D bulletRigid = bullet.GetComponent<Rigidbody2D>();
        bulletRigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

        curShotDelay = 0;
    }

    void Reload()    { curShotDelay += Time.deltaTime; }

    void Boom()
    {
        if (!isBtnB)
            return;

        if (isBoomTIme)
            return;

        if (boom == 0)
            return;

        boom--;
        isBoomTIme = true;
        gameManager.UpdateBoom(boom);

        boomEffect.SetActive(true);
        Invoke("OffBoomEffect", 2f);

        // 필드에 나와있는 몬스터 제거
        GameObject[] enemiesL = objectManager.GetPool("EnemyL");
        GameObject[] enemiesM = objectManager.GetPool("EnemyM");
        GameObject[] enemiesS = objectManager.GetPool("EnemyS");

        for (int i = 0; i < enemiesL.Length; i++) {
            if (enemiesL[i].activeSelf) {
                Enemy enemyLogic = enemiesL[i].GetComponent<Enemy>();
                enemyLogic.OnHit(1000);
            }
        }

        for (int i = 0; i < enemiesM.Length; i++) {
            if (enemiesM[i].activeSelf) {
                Enemy enemyLogic = enemiesM[i].GetComponent<Enemy>();
                enemyLogic.OnHit(1000);
            }
        }

        for (int i = 0; i < enemiesS.Length; i++) {
            if (enemiesS[i].activeSelf) {
                Enemy enemyLogic = enemiesS[i].GetComponent<Enemy>();
                enemyLogic.OnHit(1000);
            }
        }

        // 필드에 나와있는 총알 제거
        GameObject[] bulletsA = objectManager.GetPool("EnemyBulletA");
        GameObject[] bulletsB = objectManager.GetPool("EnemyBulletB");

        for (int i = 0; i < bulletsA.Length; i++) {
            if (bulletsA[i].activeSelf) 
                bulletsA[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < bulletsB.Length; i++) {
            if (bulletsB[i].activeSelf) 
                bulletsB[i].gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.tag == "Border") { // ** 경계
            switch(other.gameObject.name) {
                case "Top":
                    isTop = true;
                    break;
                case "Bottom":
                    isBottom = true;
                    break;
                case "Left":
                    isLeft = true;
                    break;
                case "Right":
                    isRight = true;
                    break;
            } 
        } else if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "EnemyBullet") { // ** 적
            if (isRespawnTime)
                return;

            if (isHit)
                return;

            isHit = true;
            life--;
            gameManager.UpdateLife(life);

            if (life == 0) {
                if (gameManager.stage == 99) {
                    gameManager.GameClear2();
                } else {
                    gameManager.GameOver();
                }

                gameManager.CallExplosion(transform.position, "P");
            } else
                gameManager.RespawnPlayer();
            
            gameObject.SetActive(false);
            other.gameObject.SetActive(false);
        } else if (other.gameObject.tag == "Item") { // ** 아이템
            Item item = other.gameObject.GetComponent<Item>();
            switch (item.type) {
                case "Coin":
                    score += 1000;
                    break;
                case "Power":
                    if (power == MaxPower)
                        score += 500;
                    else {
                        power++;
                        AddFollower();
                    }
                    break;
                case "Boom":
                    if (boom == MaxBoom)
                        boom += 500;
                    else {
                        boom++;
                        gameManager.UpdateBoom(boom);
                    }
                    break;
            }
            other.gameObject.SetActive(false);
        } else if (other.gameObject.tag == "Star") { // ** 별
            currStarNum++;
            other.gameObject.SetActive(false);
        }
    }

    void AddFollower()
    {
        if (power == 4)
            followers[0].SetActive(true);
        else if (power == 5)
            followers[1].SetActive(true);
        else if (power == 6)
            followers[2].SetActive(true);
    }

    void OffBoomEffect()
    {
        boomEffect.SetActive(false);
        isBoomTIme = false;
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.tag == "Border") {
            switch(other.gameObject.name) {
                case "Top":
                    isTop = false;
                    break;
                case "Bottom":
                    isBottom = false;
                    break;
                case "Left":
                    isLeft = false;
                    break;
                case "Right":
                    isRight = false;
                    break;
            }
        }
    }
}
