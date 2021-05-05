using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PerfectSlider : MonoBehaviour
{
    GameObject fillArea;
    private void Awake()
    {
        //fillArea = GameObject.Find("Fill Area");
        fillArea = gameObject.transform.Find("Fill Area").gameObject;
        fillArea.SetActive(false);
    }
    public void PerfectShow(Slider slider)
    {
        
        if (slider.value == 0)
            fillArea.SetActive(false);
        else if(!fillArea.activeSelf)
                fillArea.SetActive(true);
    }
}
