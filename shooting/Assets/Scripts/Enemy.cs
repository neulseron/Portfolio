using UnityEngine;

public class Enemy : MonoBehaviour
{
#region Variables
    public GameObject player;

    #region Enemy
    SpriteRenderer spriteRenderer;
    [SerializeField]
    string enemyName;
    [SerializeField]
    string bulletName;
    public float speed;
    int health;
    int enemyScore;
    [SerializeField]
    Sprite[] sprites;
    #endregion Enemy

    #region Fire
    float maxShotDelay;
    float curShotDelay;
    #endregion Fire

    string[] items;
#endregion Variables
    

#region Unity Methods
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
                maxShotDelay = 5;
                break;
            case "M":
                health = 10;
                speed = 4;
                enemyScore = 200;
                //maxShotDelay = 1;
                break;
            case "S":
                health = 5;
                speed = 1.5f;
                enemyScore = 100;
                maxShotDelay = 3;
                break;
        }  
    }

    void Update()
    {   
        if (enemyName != "M") { // M은 지나가기만
            Fire();
            Reload();
        }
    }

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
#endregion Unity Methods

    
#region Methods
    void Fire()
    {
        if (curShotDelay < maxShotDelay)
            return;
        
        GameObject bullet = ObjectManager.Instance.MakeObj(bulletName);
        bullet.transform.position = transform.position;

        Rigidbody2D bulletRigid = bullet.GetComponent<Rigidbody2D>();
        
        Vector2 dirVec;
        if (enemyName == "S")   // 작은 적은 직선 발사
            dirVec = Vector2.down;
        else    // 큰 적은 플레이어에게로 유도 발사
            dirVec = player.transform.position - transform.position;

        bulletRigid.AddForce(dirVec.normalized * 7, ForceMode2D.Impulse); 
        
        curShotDelay = 0;
    }

    public void OnHit(int damage)
    {
        if (health <= 0)
            return;

        health -= damage;
        
        // 맞은 이펙트
        spriteRenderer.sprite = sprites[1];
        Invoke("ReturnSprite", 0.1f);
 
        if (health <= 0) {
            Player playerLogic = player.GetComponent<Player>();
            playerLogic.score += enemyScore;

            // item 확률 생성
            int ran = Random.Range(1, 11);
            if (ran == 10) { // boom
                GameObject boom = ObjectManager.Instance.MakeObj(items[0]);
                boom.transform.position = transform.position;
            } else if (ran % 3 == 0) { // power
                GameObject power = ObjectManager.Instance.MakeObj(items[1]);
                power.transform.position = transform.position;
            } else if (ran % 2 == 0) { // coin
                GameObject coin = ObjectManager.Instance.MakeObj(items[2]);
                coin.transform.position = transform.position;
            }

            gameObject.SetActive(false);
            GameManager.Instance.CallExplosion(transform.position, enemyName);
        }
    }

    void Reload() => curShotDelay += Time.deltaTime; 

    void ReturnSprite() => spriteRenderer.sprite = sprites[0]; 
#endregion Methods
}
