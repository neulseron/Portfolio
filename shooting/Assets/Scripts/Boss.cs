using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("매니저")]
    public GameManager gameManager;
    public ObjectManager objectManager;
    public GameObject player;

    [Header("보스")]
    Animator animator;
    public float speed;
    int health;
    int enemyScore;

    [Header("패턴")]
    public int[] maxPatternCount;   // 패턴 당 반복 횟수
    int currPatternCount;
    int patternIndex;

    //=======================
    void Awake() 
    {
        animator = GetComponent<Animator>();
    }

    void OnEnable() {
        patternIndex = -1;
        health = 1000;
        speed = 1.5f;
        enemyScore = 2000;
        Invoke("Stop", 2);
    }

    void Stop()
    {
        if (!gameObject.activeSelf)
            return;

        Rigidbody2D rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = Vector2.zero;

        Invoke("Think", 2);
    }

    void Think()    // ** 다음 패턴 **
    {
        currPatternCount = 0;
        patternIndex = patternIndex == 3 ? 0 : patternIndex + 1;

        switch (patternIndex) {
            case 0:
                FireForward();
                break;
            case 1:
                FireShot();
                break;
            case 2:
                FireArc();
                break;
            case 3:
                FireAround();
                break;
        }
    }

    void BossFire(string name, Vector3 posVec, Vector2 dirVec)
    {
        GameObject bullet = objectManager.MakeObj(name);
        bullet.transform.position = transform.position + posVec;

        Rigidbody2D bulletRigid = bullet.GetComponent<Rigidbody2D>();
        
        bulletRigid.AddForce(dirVec.normalized * 8, ForceMode2D.Impulse);
    }

    void FireForward()
    {
        if (health <= 0)
            return;

        currPatternCount++;

        BossFire("BossBulletB", Vector3.right * 0.3f, Vector2.down);
        BossFire("BossBulletB", Vector3.right * 0.7f, Vector2.down);
        BossFire("BossBulletB", Vector3.left * 0.3f, Vector2.down);
        BossFire("BossBulletB", Vector3.left * 0.7f, Vector2.down);

        if (currPatternCount < maxPatternCount[patternIndex])
            Invoke("FireForward", 2);
        else
            Invoke("Think", 2);
    }

    void FireShot()
    {
        if (health <= 0)
            return;

        currPatternCount++;

        for (int i = 0; i < 5; i++) {
            Vector2 dir = player.transform.position - transform.position;
            Vector2 ran = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(0, 2f));
            BossFire("EnemyBulletBB", Vector3.zero, (dir + ran).normalized);
        }
        
        if (currPatternCount < maxPatternCount[patternIndex])
            Invoke("FireShot", 2);
        else
            Invoke("Think", 2);
    }

    void FireArc()
    {
        if (health <= 0)
            return;

        currPatternCount++;
        
        Vector2 dir = new Vector2(Mathf.Sin(15 * Mathf.PI * currPatternCount/maxPatternCount[patternIndex]), -1);
        BossFire("EnemyBulletA", Vector3.zero, dir.normalized);

        if (currPatternCount < maxPatternCount[patternIndex])
            Invoke("FireArc", 0.15f);
        else
            Invoke("Think", 2);
    }

    void FireAround()
    {
        if (health <= 0)
            return;

        currPatternCount++;

        int roundA = 50, roundB = 40;   // 개수 다르게 해서 가만히 있어도 피하는거 방지
        int roundNum = currPatternCount % 2 == 0 ? roundA : roundB;

        for (int i = 0; i < roundNum; i++) {
            GameObject bullet = objectManager.MakeObj("BossBulletA");
            bullet.transform.position = transform.position;
            bullet.transform.rotation = Quaternion.identity;

            Rigidbody2D bulletRigid = bullet.GetComponent<Rigidbody2D>();
            Vector2 dirVec = new Vector2(Mathf.Cos(2 * Mathf.PI * i / roundNum), Mathf.Sin(2 * Mathf.PI * i / roundNum));
            
            bulletRigid.AddForce(dirVec.normalized * 2, ForceMode2D.Impulse);
            Vector3 rotVec = Vector3.forward * 360 * i / roundNum + Vector3.forward * 90;
            bullet.transform.Rotate(rotVec);
        }
        
        if (currPatternCount < maxPatternCount[patternIndex])
            Invoke("FireAround", 2f);
        else
            Invoke("Think", 3);
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.tag == "PlayerBullet") {
            Bullet bullet = other.gameObject.GetComponent<Bullet>();
            OnHit(bullet.dmg);

            other.gameObject.SetActive(false);
        }    
    }

    public void OnHit(int damage)
    {
        if (health <= 0)
            return;

        health -= damage;

        animator.SetTrigger("OnHit");
        
        if (health <= 0) {
            objectManager.DeleteAllObj("Boss");
            
            Player playerLogic = player.GetComponent<Player>();
            playerLogic.score += enemyScore;

            gameObject.SetActive(false);
            CancelInvoke();
            gameManager.CallExplosion(transform.position, "B");

            if (gameManager.stage != 99)    // 무한 모드가 아니면 다음 스테이지로
                gameManager.GameClear();
        }
    }
}
