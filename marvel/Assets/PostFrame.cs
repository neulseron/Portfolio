using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostFrame : MonoBehaviour
{
    CommunityManager communityManager;

    void Awake() {
        communityManager = GameObject.Find("CommunityManager").GetComponent<CommunityManager>();
    }

    public void BtnShowPost()
    {
        communityManager.LoadPostFromInDate(int.Parse(gameObject.name));
    }
}
