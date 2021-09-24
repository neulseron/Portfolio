using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public int moveToMapIndex;
    public Vector3 movePos;
    public string turn;
    public int showMap;

    public Animator anim;

    private void Awake() {
        anim = GetComponent<Animator>();
    }

    public bool AnimationDone()
    {
        if(anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f) {
            return true;
        }

        return false;
    }   

}
