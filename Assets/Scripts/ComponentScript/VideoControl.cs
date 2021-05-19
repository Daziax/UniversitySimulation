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
        vPlayer.clip = null;
        switch (act)
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
            case "Exam":
                vPlayer.clip = Resources.Load<VideoClip>("Animator/Exam");
                break;
            case "PlayGame":
                vPlayer.clip = Resources.Load<VideoClip>("Animator/PlayGame");
                break;
            case "Ending2":
                vPlayer.clip = Resources.Load<VideoClip>("Animator/Ending2");
                break;


        }
        videoPlayer.SetActive(true);
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
