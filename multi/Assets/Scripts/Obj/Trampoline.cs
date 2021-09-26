using UnityEngine;

public class Trampoline : MonoBehaviour
{
    Animator animator;

    void Awake() {
        animator = GetComponent<Animator>();
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Player") {
            other.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 30, ForceMode2D.Impulse);
            animator.SetTrigger("isJump");
        }
    }
}
