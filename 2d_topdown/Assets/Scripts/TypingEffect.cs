using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypingEffect : MonoBehaviour
{
    public bool isPortraitDiff = false;


    public int CharPerSeconds;
    public GameObject endCursor;
    public bool isAnim;

    AudioSource audioSource;
    string targetMsg;
    Text msgText;
    int index;
    public string allignment;


    void Awake() {
        msgText = GetComponent<Text>();
        audioSource = GetComponent<AudioSource>();
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
    public void SetMsg(string msg, int _length)
    {
        if (isAnim) {
            msgText.text = targetMsg;
            CancelInvoke();
            EffectEnd();
        } else {
            targetMsg = msg;

            if (isPortraitDiff) {
                msgText.text = "";
                index = _length;
                for (int i = 0; i < index; i++)
                    msgText.text += targetMsg[i];
            }

            EffectStart();
        }
    }

    void SetAllignment()
    {
        switch (allignment) {
            case "UL" :
                msgText.alignment = TextAnchor.UpperLeft;
                break;
            case "UC" :
                msgText.alignment = TextAnchor.UpperCenter;
                break;
        }
    }


    void EffectStart()
    {
        SetAllignment();

        if (!isPortraitDiff) {
            msgText.text = "";
            index = 0;
        }

        endCursor.SetActive(false);

        isAnim = true;

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
        if (targetMsg[index] != ' ' && targetMsg[index] != '.' && targetMsg[index] != '?')
            audioSource.Play();
    
        index++;

        Invoke("Effecting", 1.0f / CharPerSeconds);
    }

    void EffectEnd()
    {
        isAnim = false;
        endCursor.SetActive(true);
    }
}
