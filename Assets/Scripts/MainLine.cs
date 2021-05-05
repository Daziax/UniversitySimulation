using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainLine
{
    Model model;
    Activity activity;
    PlayerValue playerValue;
    EnvironmentValue env;
    bool isReExam,isDated;
    /// <summary>
    /// 今天是否已经被约
    /// </summary>
    public bool IsDated
    {
        get => isDated;
        set
        {
            isDated = value;
        }
    }

    // Start is called before the first frame update
    private MainLine()
    {
        Initialization();
    }
    private static MainLine instance = new MainLine();
    public static MainLine Instance => instance;
    void Initialization()
    {
        model = Model.ModelInstance();
        activity = new Activity();
        playerValue = PlayerValue.PlayerInstance();
        env = EnvironmentValue.EnvInstance;
        
    }
    void NewTerm()
    {
        ReExam();
    }
    public void NewDay()
    {
        //生活费
        if (env.Day % 3 == 0)//30
        {
            activity.GetlivingCosts();
        }
        //考试和补考
        if(env.Day% 2==0)//105
        {
            for(int i=0;i<100;++i)
            {
                Debug.LogWarning(activity.Exam());
            }
            //isReExam = activity.Exam() ? false : true;
        }
        isDated = false;
    }
    void ReExam()
    {
        if (!isReExam)
            return;
        isReExam = activity.Exam();
    }
}
