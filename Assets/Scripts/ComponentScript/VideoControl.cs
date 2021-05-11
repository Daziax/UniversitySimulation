using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class VideoControl:MonoBehaviour
{
    GameObject videoPlayer;
    VideoPlayer vPlayer;
    RawImage rImgContainer;
    void Awake()
    {
        videoPlayer = gameObject;
        videoPlayer.SetActive(false);
        vPlayer = videoPlayer.GetComponent<VideoPlayer>();
        rImgContainer = vPlayer.GetComponent<RawImage>();
       
    }
    public void Play(string act)
    {
        videoPlayer.SetActive(true);
        switch(act)
        {
            case "Eat":
                vPlayer.clip = Resources.Load<VideoClip>("Animator/Eat");
                break;
            case "Study":
                vPlayer.clip = Resources.Load<VideoClip>("Animator/Study");
                break;
            case "Sleep":
                vPlayer.clip = Resources.Load<VideoClip>("Animator/Sleep");
                break;
            case "Exercise":
                vPlayer.clip = Resources.Load<VideoClip>("Animator/Exercise");
                break;
            case "ParttimeJob":
                vPlayer.clip = Resources.Load<VideoClip>("Animator/ParttimeJob");
                break;
            case "Place":
                vPlayer.clip = Resources.Load<VideoClip>("Animator/Place");
                break;


        }
        vPlayer.Play();
        StartCoroutine(StopPlay((float)vPlayer.length));
    }
    IEnumerator StopPlay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        vPlayer.Stop();
        videoPlayer.SetActive(false);
    }
}
