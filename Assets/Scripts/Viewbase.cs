using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Timers;


public class ViewBase
{
    GameObject canvas;
    GameObject imgTxtBack;
    Timer timer;
    GameObject TxtMessage;
    UIController uiController;
    delegate void CloseHandler(GameObject imgTxtBack);
    event CloseHandler Close;
    public ViewBase() 
    {
        canvas = GameObject.Find("UniversityScene").transform.Find("Canvas").gameObject;
        uiController =canvas.GetComponent<UIController>();
        Close += uiController.StartCloseTalk;
    }

    public void ShowTalk(string text)//显示对话
    {
        imgTxtBack = canvas.transform.Find("ImgTxtBack").gameObject;
        TxtMessage = imgTxtBack.transform.Find("TxtTalk").gameObject;
        imgTxtBack.SetActive(true);
        TxtMessage.GetComponent<Text>().text = text;
        Close(imgTxtBack);
       
    } 
    public void ShowBlackMessage(string text)//显示过场
    {
        imgTxtBack = canvas.transform.Find("ImgBlackBackground").gameObject;
        TxtMessage = imgTxtBack.transform.Find("TxtTalk").gameObject;
        imgTxtBack.SetActive(true);
        TxtMessage.GetComponent<Text>().text = text;
        Close(imgTxtBack);
    }
}
