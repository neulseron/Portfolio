using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleTypingEffect : MonoBehaviour
{
    public int CharPerSeconds;
    public bool isAnim;

    //AudioSource audioSource;
    string targetMsg;
    Text msgText;
    int index;


    void Awake() {
        //audioSource = GetComponent<AudioSource>();
        msgText = GetComponent<Text>();
    }

    public void SetMsg(string msg)
    {
        if (isAnim) {
            msgText.text = targetMsg;
            CancelInvoke();
            EffectEnd();
        } else {
            targetMsg = msg;
            EffectStart();
        }
    }


    void EffectStart()
    {
        isAnim = true;

        msgText.text = "";
        index = 0;

        Invoke("Effecting", 1.0f / CharPerSeconds);
    }

    void Effecting()
    {
        if (msgText.text == targetMsg) {
            EffectEnd();
            return;
        }

        msgText.text += targetMsg[index];

        // Sound
        //if (targetMsg[index] != ' ' && targetMsg[index] != '.' && targetMsg[index] != '?')
            //audioSource.Play();
    
        index++;

        Invoke("Effecting", 1.0f / CharPerSeconds);
    }

    void EffectEnd()
    {
        isAnim = false;
    }
}
