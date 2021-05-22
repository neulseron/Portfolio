using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // 매니저
    public GameManager gameManager;
    public ObjectManager objectManager;
    SpriteRenderer spriteRenderer;
    //---------------------
    // enemy 변수
    public string enemyName;
    public string bulletName;
    public float speed;
    int health;
    int enemyScore;
    public Sprite[] sprites;
    //---------------------
    // 외부변수
    public GameObject player;
    //---------------------
    // 코드 변수
    string[] items;
    float maxShotDelay;
    float curShotDelay;
    //=======================
    // 시스템 함수
    void Awake() 
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        items = new string[] { "ItemBoom", "ItemPower", "ItemCoin" };
    }

    void OnEnable() {
        switch (enemyName) {
            case "L":
                health = 50;
                speed = 1;
                enemyScore = 500;
                maxShotDelay = 0.7f;
                break;
            case "M":
                health = 10;
                speed = 3;
                enemyScore = 200;
                //maxShotDelay = 1;
                break;
            case "S":
                health = 5;
                speed = 1;
                enemyScore = 100;
                maxShotDelay = 3;
                break;
        }  
    }

    void Update()
    {   
        if (enemyName != "M") {
            Fire();
            Reload();
        }
    }
    
    //---------------------
    // 커스텀 함수

    // ** 일반 **
    void Fire()
    {
        if (curShotDelay < maxShotDelay)
            return;
        
        GameObject bullet = objectManager.MakeObj(bulletName);
        bullet.transform.position = transform.position;

        Rigidbody2D bulletRigid = bullet.GetComponent<Rigidbody2D>();
        
        Vector2 dirVec;
        if (enemyName == "S")
            dirVec = Vector2.down;
        else
            dirVec = player.transform.position - transform.position;
        bulletRigid.AddForce(dirVec.normalized * 8, ForceMode2D.Impulse); 
        
        curShotDelay = 0;
    }

    void Reload()
    {
        curShotDelay += Time.deltaTime;
    }

    void ReturnSprite()
    {
        spriteRenderer.sprite = sprites[0];
    }

    // ** 공통 **
    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.tag == "BorderBullet")
            gameObject.SetActive(false);
        else if (other.gameObject.tag == "PlayerBullet") {
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
        
        spriteRenderer.sprite = sprites[1];
        Invoke("ReturnSprite", 0.1f);
 
        if (health <= 0) {
            Player playerLogic = player.GetComponent<Player>();
            playerLogic.score += enemyScore;

            // item drop
            int ran = Random.Range(1, 11);
            if (ran == 10) { // boom
                GameObject boom = objectManager.MakeObj(items[0]);
                boom.transform.position = transform.position;
            } else if (ran % 3 == 0) { // power
                GameObject power = objectManager.MakeObj(items[1]);
                power.transform.position = transform.position;
            } else if (ran % 2 == 0) { // coin
                GameObject coin = objectManager.MakeObj(items[2]);
                coin.transform.position = transform.position;
            }

            gameObject.SetActive(false);
            gameManager.CallExplosion(transform.position, enemyName);
        }
    }
}
