using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WordviewManager : MonoBehaviour
{
    public void LoadMenuScene(int idx)
    {
        ApplicationManager.Instance.LoadNextScene(idx);
    }
}
