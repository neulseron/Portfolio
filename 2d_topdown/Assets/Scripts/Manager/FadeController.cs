using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 외부에서 호출하고 싶은 Fade Effect
public enum FadeState { FadeIn = 0, FadeOut, FadeInOut, FadeLoop, FadeOut2 }

public class FadeController : MonoBehaviour
{
    [SerializeField]
    [Range(0.01f, 10f)]
    float fadeTime;

    [SerializeField]
    AnimationCurve fadeCurve;

    public Image FadeImg;
    FadeState fadeState;
    public bool endFade;


    public SimpleTypingEffect nameTxtTE;
    public SimpleTypingEffect timeTxtTE;
    public bool nameTyping = false;
    public string nameStr;
    public string timeStr;

    void Awake() {
        FadeImg = GetComponent<Image>();

        //OnFade(FadeState.FadeOut);
    }

    public void OnFade(FadeState _state)
    {
        fadeState = _state;
        endFade = false;
        GameManager.Instance.dontSave = true;
        GameManager.Instance.dontMove = true;

        switch (fadeState) {
            case FadeState.FadeIn:
                StartCoroutine(Fade(1, 0));
                break;
            case FadeState.FadeOut:
                StartCoroutine(Fade(0, 1));
                break;
            case FadeState.FadeOut2:
                StartCoroutine(Fade2(0, 1));
                break;
            case FadeState.FadeInOut:
            case FadeState.FadeLoop:
                StartCoroutine(FadeInOut());
                break;
        }
    }

    IEnumerator Fade(float _start, float _end)
    {
        float currTime = 0.0f;
        float percent = 0.0f;

        while (percent < 1)
        {
            currTime += Time.deltaTime;
            percent = currTime / fadeTime;
            //Debug.Log(percent);

            Color color = FadeImg.color;
            color.a = Mathf.Lerp(_start, _end, fadeCurve.Evaluate(percent));
            FadeImg.color = color;

            if (_start == 0 && percent > 0.9 && !nameTyping) {
                nameTyping = true;
                StartCoroutine(SetTxt(nameStr, timeStr));
            }
            if (_start == 1 && percent > 0.9) {
                nameTyping = false;
                nameTxtTE.gameObject.SetActive(false);
                timeTxtTE.gameObject.SetActive(false);
            }

            yield return null;
        }
    }

    IEnumerator Fade2(float _start, float _end)
    {
        float currTime = 0.0f;
        float percent = 0.0f;

        while (percent < 1)
        {
            currTime += Time.deltaTime;
            percent = currTime / fadeTime;

            Color color = FadeImg.color;
            color.a = Mathf.Lerp(_start, _end, fadeCurve.Evaluate(percent));
            FadeImg.color = color;

            yield return null;
        }
    }

    IEnumerator FadeInOut()
    {
        while (true) {
            yield return StartCoroutine(Fade(1, 0));    // Fade In
            yield return StartCoroutine(Fade(0, 1));    // Fade Out

            if (fadeState == FadeState.FadeInOut)       break;
        }
    }

    public IEnumerator SetTxt(string _name, string _time)
    {
        nameTxtTE.gameObject.SetActive(true);
        nameTxtTE.SetMsg(_name);
        yield return new WaitUntil(() => nameTxtTE.isAnim == false);
        timeTxtTE.gameObject.SetActive(true);
        timeTxtTE.SetMsg(_time);
        yield return new WaitUntil(() => timeTxtTE.isAnim == false);

        yield return new WaitForSeconds(0.5f);
        _time = _time.Replace(":00", ":01");
        timeTxtTE.GetComponent<Text>().text = _time;
        yield return new WaitForSeconds(1f);
        _time = _time.Replace(":01", ":02");
        timeTxtTE.GetComponent<Text>().text = _time;
        yield return new WaitForSeconds(1f);
        _time = _time.Replace(":02", ":03");
        timeTxtTE.GetComponent<Text>().text = _time;
        endFade = true;

        yield return StartCoroutine(Fade(1, 0));    // Fade In
        GameManager.Instance.dontSave = false;
        GameManager.Instance.dontMove = false;
        
        Debug.Log(GameManager.Instance.currTime);
    }

}
