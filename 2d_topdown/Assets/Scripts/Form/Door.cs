using UnityEngine;

public class Door : MonoBehaviour
{
#region Variables
    public int moveToMapIndex;
    public Vector3 movePos;
    public string turn;
    public int showMap;

    public Animator anim;
#endregion Variables


#region Methods
    void Awake() {
        anim = GetComponent<Animator>();
    }

    public bool AnimationDone()
    {
        if(anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f) {
            return true;
        }

        return false;
    }   
#endregion Methods
}
