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
        //vPlayer.clip = null;
        //string tempAct;
        Debug.Log(act);
        if(vPlayer.isPlaying||vPlayer.clip!=null)
        {
            StartCoroutine(WaitLast(act));
        }
        else
        {
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
                case "Backetball":
                    vPlayer.clip = Resources.Load<VideoClip>("Animator/Bascketball");
                    break;
            }
            videoPlayer.SetActive(true);
            Debug.Log("显示");
            vPlayer.Prepare();
            StartCoroutine(StopPlay(vPlayer,null));
        }
       
    }
    IEnumerator StopPlay(VideoPlayer vPlayer,string nextVideo)
    {
        float seconds = (float)vPlayer.length;
        //while (!vPlayer.isPrepared)
        //{
        //    yield return new WaitForSeconds(1);
        //}

        Debug.Log("开始播放");
        vPlayer.Play();
        if(!vPlayer.isPlaying)
            Debug.Log("vPlayer is not playing");
        while (!vPlayer.isPlaying)
        {
            yield return null;
        }
        while(vPlayer.isPlaying)
        {
            Debug.Log("vPlayer is playing");
            yield return 0;
        }
        //yield return new WaitForSeconds(seconds);
        Debug.Log("vPlayer is played");
        vPlayer.Stop();
        vPlayer.clip = null;
        videoPlayer.SetActive(false);
        //if (nextVideo != null)
        //{
        //    Play(nextVideo);
        //}
            
    }
    IEnumerator WaitLast(string act)
    {
        Debug.Log("Start play next"+act+ vPlayer.length);

        //yield return new WaitForSeconds((float)vPlayer.length);
        while (!vPlayer.isPlaying)
        {
            yield return null;
        }
        while (vPlayer.isPlaying)
        {
            Debug.Log("last vPlayer is playing");
            yield return 0;
        }
        while (videoPlayer.activeSelf)
            yield return 0;
        Debug.Log("Stop play next");
      
        Play(act);
        StopCoroutine("WaitLast");

    }
}
