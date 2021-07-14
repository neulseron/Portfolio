using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    public GameManager gameManager;
    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update() {
        if (!gameManager.touchFlag) {
            animator.SetBool("isOff", true);
        } else {
            animator.SetBool("isOff", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            animator.SetTrigger("isTouch");
        }
    }
}
