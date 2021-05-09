using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 好感度类
/// </summary>
public class Relationship
{
    //private static Relationship relationship = new Relationship();

    private static List<Person> people;
    public static List<Person> People
    {
        get => people;
        set
        {
            people = value;
        }
    }

    //private Relationship() { }
    //public int XiaoMing
    //{
    //    get; set;
    //}
    //public int LiLei
    //{
    //    get; set;
    //}
    //public int HanMeiMei //女性
    //{
    //    get; set;
    //}
    //public int XiaoHong
    //{
    //    get;
    //    set;
    //}

    //public void IncreaseFavorability(string name, float score)
    //{
    //    System.Reflection.FieldInfo fieldInfo = GetType().GetField(name);
    //    fieldInfo.SetValue(this, (int)fieldInfo.GetValue(this) + score);
    //}
    //public void ReduceFavorability(string name, float score)
    //{
    //    System.Reflection.FieldInfo fieldInfo = GetType().GetField(name);
    //    fieldInfo.SetValue(this, score - (int)fieldInfo.GetValue(this));
    //}
}
public struct Person
{

    public string Name { get; set; }
    public int FriendValue { get; set; }

    public int Age { get; set; }
    /// <summary>
    /// 性别，男true,女false
    /// </summary>
    public bool Sex { get; set; }
    public bool IsFriend { get; set; }
    public string ImgPath { get; set; }

}
