using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public GameObject enemyLPrefab;
    public GameObject enemyMPrefab;
    public GameObject enemySPrefab;
    public GameObject bossPrefab;

    public GameObject itemCoinPrefab;
    public GameObject itemPowerPrefab;
    public GameObject itemBoomPrefab;

    public GameObject[] powerBulletPrefab;
    public GameObject followerBulletPrefab;

    public GameObject enemyBulletAPrefab;
    public GameObject enemyBulletBPrefab;
    public GameObject enemyBulletBBPrefab;
    public GameObject[] bossBulletPrefab;

    public GameObject explosionPrefab;
    public GameObject starPrefab;


    GameObject[] boss;
    GameObject[] enemyL;
    GameObject[] enemyM;
    GameObject[] enemyS;

    GameObject[] itemCoin;
    GameObject[] itemPower;
    GameObject[] itemBoom;

    GameObject[] power0Bullet;
    GameObject[] power1Bullet;
    GameObject[] power2Bullet;
    GameObject[] power3Bullet;
    GameObject[] followerBullet;

    GameObject[] enemyBulletA;
    GameObject[] enemyBulletB;
    GameObject[] enemyBulletBB;
    GameObject[] bossBulletA;
    GameObject[] bossBulletB;

    GameObject[] explosion;
    GameObject[] star;

    GameObject[] targetPool;

    void Awake() {
        boss = new GameObject[10];
        enemyL = new GameObject[10];
        enemyM = new GameObject[10];
        enemyS = new GameObject[10];

        itemCoin = new GameObject[10];
        itemPower = new GameObject[10];
        itemBoom = new GameObject[10];

        power0Bullet = new GameObject[100];
        power1Bullet = new GameObject[100];
        power2Bullet = new GameObject[100];
        power3Bullet = new GameObject[100];
        followerBullet = new GameObject[100];

        enemyBulletA = new GameObject[100];
        enemyBulletB = new GameObject[100];
        enemyBulletBB = new GameObject[100];
        bossBulletA = new GameObject[1000];
        bossBulletB = new GameObject[50];

        explosion = new GameObject[20];
        star = new GameObject[3];

        Generate();
    }

    void Generate()
    {
        // enemy
        for (int i = 0; i < boss.Length; i++) {
            boss[i] = Instantiate(bossPrefab);
            boss[i].SetActive(false);
        }

        for (int i = 0; i < enemyL.Length; i++) {
            enemyL[i] = Instantiate(enemyLPrefab);
            enemyL[i].SetActive(false);
        }

        for (int i = 0; i < enemyM.Length; i++) {
            enemyM[i] = Instantiate(enemyMPrefab);
            enemyM[i].SetActive(false);
        }

        for (int i = 0; i < enemyS.Length; i++) {
            enemyS[i] = Instantiate(enemySPrefab);
            enemyS[i].SetActive(false);
        }

        // item
        for (int i = 0; i < itemCoin.Length; i++) {
            itemCoin[i] = Instantiate(itemCoinPrefab);
            itemCoin[i].SetActive(false);
        }

        for (int i = 0; i < itemPower.Length; i++) {
            itemPower[i] = Instantiate(itemPowerPrefab);
            itemPower[i].SetActive(false);
        }

        for (int i = 0; i < itemBoom.Length; i++) {
            itemBoom[i] = Instantiate(itemBoomPrefab);
            itemBoom[i].SetActive(false);
        }

        // enemy bullet
        for (int i = 0; i < enemyBulletA.Length; i++) {
            enemyBulletA[i] = Instantiate(enemyBulletAPrefab);
            enemyBulletA[i].SetActive(false);
        }

        for (int i = 0; i < enemyBulletB.Length; i++) {
            enemyBulletB[i] = Instantiate(enemyBulletBPrefab);
            enemyBulletB[i].SetActive(false);
        }

        for (int i = 0; i < enemyBulletBB.Length; i++) {
            enemyBulletBB[i] = Instantiate(enemyBulletBBPrefab);
            enemyBulletBB[i].SetActive(false);
        }

        for (int i = 0; i < bossBulletA.Length; i++) {
            bossBulletA[i] = Instantiate(bossBulletPrefab[0]);
            bossBulletA[i].SetActive(false);
        }

        for (int i = 0; i < bossBulletB.Length; i++) {
            bossBulletB[i] = Instantiate(bossBulletPrefab[1]);
            bossBulletB[i].SetActive(false);
        }

        // player
        for (int i = 0; i < power0Bullet.Length; i++) {
            power0Bullet[i] = Instantiate(powerBulletPrefab[0]);
            power0Bullet[i].SetActive(false);
        }

        for (int i = 0; i < power1Bullet.Length; i++) {
            power1Bullet[i] = Instantiate(powerBulletPrefab[1]);
            power1Bullet[i].SetActive(false);
        }

        for (int i = 0; i < power2Bullet.Length; i++) {
            power2Bullet[i] = Instantiate(powerBulletPrefab[2]);
            power2Bullet[i].SetActive(false);
        }

        for (int i = 0; i < power3Bullet.Length; i++) {
            power3Bullet[i] = Instantiate(powerBulletPrefab[3]);
            power3Bullet[i].SetActive(false);
        }

        for (int i = 0; i < followerBullet.Length; i++) {
            followerBullet[i] = Instantiate(followerBulletPrefab);
            followerBullet[i].SetActive(false);
        }


        for (int i = 0; i < explosion.Length; i++) {
            explosion[i] = Instantiate(explosionPrefab);
            explosion[i].SetActive(false);
        }

        for (int i = 0; i < star.Length; i++) {
            star[i] = Instantiate(starPrefab);
            star[i].SetActive(false);
        }
    }

    public GameObject MakeObj(string type)
    {
        switch (type) {
            case "Boss":
                targetPool = boss;
                break;
            case "EnemyL":
                targetPool = enemyL;
                break;
            case "EnemyM":
                targetPool = enemyM;
                break;
            case "EnemyS":
                targetPool = enemyS;
                break;
            case "ItemCoin":
                targetPool = itemCoin;
                break;
            case "ItemPower":
                targetPool = itemPower;
                break;
            case "ItemBoom":
                targetPool = itemBoom;
                break;
            case "FollwerBullet":
                targetPool = followerBullet;
                break;
            case "Power0Bullet":
                targetPool = power0Bullet;
                break;
            case "Power1Bullet":
                targetPool = power1Bullet;
                break;
            case "Power2Bullet":
                targetPool = power2Bullet;
                break;
            case "Power3Bullet":
                targetPool = power3Bullet;
                break;
            case "EnemyBulletA":
                targetPool = enemyBulletA;
                break;
            case "EnemyBulletB":
                targetPool = enemyBulletB;
                break;
            case "EnemyBulletBB":
                targetPool = enemyBulletBB;
                break;
            case "BossBulletA":
                targetPool = bossBulletA;
                break;
            case "BossBulletB":
                targetPool = bossBulletB;
                break;
            case "Explosion":
                targetPool = explosion;
                break;
            case "Star":
                targetPool = star;
                break;
        }

        for (int i = 0; i < targetPool.Length; i++) {
            if (!targetPool[i].activeSelf) {
                targetPool[i].SetActive(true);
                return targetPool[i];
            }
        }

        return null;
    }

    public GameObject[] GetPool(string type)
    {
        switch (type) {
            case "Boss":
                targetPool = boss;
                break;
            case "EnemyL":
                targetPool = enemyL;
                break;
            case "EnemyM":
                targetPool = enemyM;
                break;
            case "EnemyS":
                targetPool = enemyS;
                break;
            case "ItemCoin":
                targetPool = itemCoin;
                break;
            case "ItemPower":
                targetPool = itemPower;
                break;
            case "ItemBoom":
                targetPool = itemBoom;
                break;
            case "FollwerBullet":
                targetPool = followerBullet;
                break;
            case "Power0Bullet":
                targetPool = power0Bullet;
                break;
            case "Power1Bullet":
                targetPool = power1Bullet;
                break;
            case "Power2Bullet":
                targetPool = power2Bullet;
                break;
            case "Power3Bullet":
                targetPool = power3Bullet;
                break;
            case "EnemyBulletA":
                targetPool = enemyBulletA;
                break;
            case "EnemyBulletB":
                targetPool = enemyBulletB;
                break;
            case "EnemyBulletBB":
                targetPool = enemyBulletBB;
                break;
            case "BossBulletA":
                targetPool = bossBulletA;
                break;
            case "BossBulletB":
                targetPool = bossBulletB;
                break;
            case "Explosion":
                targetPool = explosion;
                break;
            case "Star":
                targetPool = star;
                break;
        }
        
        return targetPool;
    }

    public void DeleteAllObj(string type)
    {
        if(type == "Boss"){
            for (int i = 0; i < bossBulletA.Length; i++)
            bossBulletA[i].SetActive(false);

            for (int index=0; index<bossBulletB.Length; index++)
            bossBulletB[index].SetActive(false);
        }
    }
}
