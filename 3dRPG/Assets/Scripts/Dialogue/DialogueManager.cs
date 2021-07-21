using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    static DialogueManager instance;
    public static DialogueManager Instance => instance;

    public Text nameTxt;
    public Text dialogueTxt;

    public Animator animator = null;

    Queue<string> sentences;

    public event Action OnStartDialogue;
    public event Action OnEndDialogue;


    private void Awake() 
    {
        instance = this;
    }

    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        OnStartDialogue?.Invoke();

        animator?.SetBool("IsOpen", true);

        nameTxt.text = dialogue.name;
        sentences.Clear();
        foreach (string st in dialogue.sentences) {
            sentences.Enqueue(st);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0) {
            animator?.SetBool("IsOpen", false);
            OnEndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();

        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueTxt.text = string.Empty;

        yield return new WaitForSeconds(0.25f);

        foreach (char letter in sentence.ToCharArray()) {
            dialogueTxt.text += letter;
            yield return null;
        }
    }

    void EndDialogue()
    {
        Debug.Log("End dialogue입니다");
        //animator?.SetBool("IsOpen", false);

        OnEndDialogue?.Invoke();
    }
}
