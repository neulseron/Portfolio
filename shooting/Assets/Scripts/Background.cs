using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public float speed;
    public int startIndex;      // 2
    public int endIndex;        // 0
    //---------------------
    public Transform[] sprites;
    //---------------------
    float viewHeight;

    private void Awake() {
        viewHeight = Camera.main.orthographicSize * 2;  // 10
    }

    void Update()
    {
        Move();
        Scrolling();
    }

    void Move()
    {
        Vector3 curPos = transform.position;
        Vector3 nextPos = Vector3.down * speed * Time.deltaTime;
        transform.position = curPos + nextPos;
    }

    void Scrolling()
    {
        if (sprites[endIndex].position.y < viewHeight * (-1)) {
            Vector3 backSpritesPos = sprites[startIndex].transform.localPosition;
            sprites[endIndex].transform.localPosition = backSpritesPos + Vector3.up * viewHeight;   // startIndex위에 갖다 붙임

            // 인덱스 교체
            int tmp = startIndex;
            startIndex = endIndex;
            endIndex = tmp - 1 == -1 ? sprites.Length - 1 : tmp - 1;
        }
    }
}
