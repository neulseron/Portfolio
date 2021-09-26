using UnityEngine;

public class Flag : MonoBehaviour
{
    Animator animator;


    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update() {
        if (!GameManager.Instance.TouchFlag) {
            animator.SetBool("isOff", true);
        } else {
            animator.SetBool("isOff", false);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            animator.SetTrigger("isTouch");
        }
    }
}
