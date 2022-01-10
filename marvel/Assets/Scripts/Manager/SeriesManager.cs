using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;

public class SeriesManager : MonoBehaviour
{
    public void LoadMenuScene(int idx)
    {
        ApplicationManager.Instance.LoadNextScene(idx);
    }
}
