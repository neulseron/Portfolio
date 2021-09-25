using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int dmg;
    
    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.tag == "BorderBullet") {
            gameObject.SetActive(false);
        }
    }
}
