using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Timers;

using System;
public class ViewBase
{
    GameObject canvas;
    GameObject imgTxtBack;
    Timer timer;
    GameObject TxtMessage;
    UIController uiController;
    //delegate void ShowMessageHandler(GameObject imgTxtBack);
    //event ShowMessageHandler ShowMessage;
    Action<string,GameObject,GameObject,float> ShowMessage;
    public ViewBase() 
    {
        
        canvas = GameObject.Find("UniversityScene").transform.Find("Canvas").gameObject;
        uiController =canvas.GetComponent<UIController>();
        ShowMessage += uiController.StartShowMessage;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param>
    /// <param name="form">形式,0是Message,1是BlackBack</param>
    /// <param name="isDelay"></param>
    public void StartShowMessage(string text, bool form = false, float isDelay=5)//显示对话
    {
        //Showtalk(text);
        //StartCoroutine(Showtalk(text,isDelay));
        //StartCoroutine(CloseTalk());
        if(form)
            imgTxtBack = canvas.transform.Find("ImgBlackBackground").gameObject;
        else 
            imgTxtBack = canvas.transform.Find("ImgTxtBack").gameObject;
        
        TxtMessage = imgTxtBack.transform.Find("TxtTalk").gameObject;
        if (canvas.transform.Find("ImgBlackBackground").gameObject.activeSelf ||
            canvas.transform.Find("ImgTxtBack").gameObject.activeSelf ||
            canvas.transform.Find("Video Player").gameObject.activeSelf)
            ShowMessage(text, imgTxtBack, TxtMessage, isDelay);
        else
            ShowMessage(text, imgTxtBack, TxtMessage,1);

    } 
    //private IEnumerator Showtalk(string text, bool isDelay = false)
    //{
    //    if (isDelay)
    //    {
    //        yield return new WaitForSeconds(3);
    //    }
    //    yield return 0;
        
    //    imgTxtBack.SetActive(true);
    //    TxtMessage.GetComponent<Text>().text = text;
    //    //Close(imgTxtBack);
    //}
    //public void StartShowMessage(string text)//显示过场
    //{
    //    imgTxtBack = canvas.transform.Find("ImgBlackBackground").gameObject;
    //    TxtMessage = imgTxtBack.transform.Find("TxtTalk").gameObject;
    //    ShowMessage(text, imgTxtBack, TxtMessage, false);
    //    //imgTxtBack.SetActive(true);
    //    //TxtMessage.GetComponent<Text>().text = text;
    //    //Close(imgTxtBack);
    //}

    //IEnumerator CloseTalk()
    //{
    //    yield return new WaitForSeconds(3);
    //    imgTxtBack = canvas.transform.Find("ImgTxtBack").gameObject;
    //    imgTxtBack.SetActive(false);
    //}
    ///// <summary>
    ///// 开启“关闭对话”的协程
    ///// </summary>
    ///// <param name="imgTxtBack"></param>
    //public void StartCloseTalk(GameObject imgTxtBack)
    //{
    //    StartCoroutine(CloseTalk());
    //}
}
