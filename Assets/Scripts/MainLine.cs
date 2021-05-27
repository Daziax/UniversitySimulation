using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainLine
{
    Model model;
    Activity activity;
    PlayerValue playerValue;
    EnvironmentValue env;
    List<Person> roles;
    bool isDated, isDead, isLunch, isBreakfast, isDinner, isSlept;
    public bool IsReExam { get; set; }
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
        roles = Relationship.People;

    }
    void NewTerm()
    {
        ReExam();
    }
    public void NewDay()
    {
        ViewBase viewBase = new ViewBase();
        //刚入学
        if (env.Day == 2)
        {
            activity.GetlivingCosts();
        }
        //生活费
        if (env.Day == 3)//30
        {
            //activity.GetlivingCosts();
        }
        //考试和补考
        if (env.Day  == 5)//105
        {
            viewBase.StartShowMessage("今天您要参加本学期的期末考试，请及时前往教室参加考试。");
            //for(int i=0;i<100;++i)
            //{
            //    Debug.LogWarning(activity.Exam());
            //}

            //IsReExam = activity.Exam() ? false : true;
        }
        //结局
        if(env.Day ==6)
        {
            viewBase.StartShowMessage("今天是您大学生活的最后一天了，请及时前往教学楼参加毕业典礼。");
            //activity.Ending();
        }


    }
    public void EndOfDay()
    {
        isDated = false;
        isLunch = false;
        isBreakfast = false;
        isDinner = false;
        isSlept = true;
        NewDay();

    }
    void ReExam()
    {
        if (!IsReExam)
            return;
        IsReExam = activity.Exam();
    }
    void Eat()
    {
        if (isBreakfast)
            playerValue.Buff = 1.1f;

    }
}
