using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.IO;

public class VideoController : MonoBehaviour
{
    public RawImage image;

    VideoPlayer videoPlayer;
    VideoSource videoSource;
    
    void Start()
    {
        Application.runInBackground = true;
        StartCoroutine(playVideo());
        //title0.url = Application.dataPath + "/StreamingAssets" +"/title1.ogv";

        //title0.Play();
    }

    private void Update() {
    }

    IEnumerator playVideo()
    {
        string url = "file://" + Application.streamingAssetsPath +"/title1.ogv";

        videoPlayer = gameObject.AddComponent<VideoPlayer>();
        videoPlayer.playOnAwake = false;
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = url;
        videoPlayer.isLooping = true;
        videoPlayer.playbackSpeed = 1.5f;
        videoPlayer.Prepare();

        while(!videoPlayer.isPrepared) {
            Debug.Log("preparing");
            yield return null;
        }
        Debug.Log("done");

        image.texture = videoPlayer.texture;
        videoPlayer.aspectRatio = VideoAspectRatio.FitVertically;
        videoPlayer.Play();
    }
}
