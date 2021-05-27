using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class Model
{
    private static Model modelInstance = new Model();
    PlayerValue playerValue = PlayerValue.PlayerInstance();
    EnvironmentValue envValue = EnvironmentValue.EnvInstance;
    private Model() { }

    public static Model ModelInstance()
    {
        return modelInstance;
    }
    /// <summary>
    /// 存档
    /// </summary>
    public void SaveData()
    {
        PlayerPrefs.SetFloat("IQ", playerValue.IQ);
        PlayerPrefs.SetFloat("Charm", playerValue.Charm);
        PlayerPrefs.SetFloat("Energy", playerValue.Energy);
        PlayerPrefs.SetFloat("EQ", playerValue.EQ);
        PlayerPrefs.SetFloat("Vanity", playerValue.Vanity);
        PlayerPrefs.SetFloat("Money", playerValue.Money);
        PlayerPrefs.SetInt("Day", envValue.Day);
        //PlayerPrefs.SetString("Scene", GameObject.Find("UniversityScene").transform.Find("Canvas").Find("Background").GetComponent<UnityEngine.UI.Image>().sprite)
        //PlayerPrefs.SetString("SavedScene", SceneManager.GetActiveScene().name);//存储当前场景
        Debug.Log(Relationship.People[1].Name + Relationship.People[1].FriendValue);
        string json = LitJson.JsonMapper.ToJson(Relationship.People);
        StreamWriter writer = new StreamWriter(Application.streamingAssetsPath + "/relationship.json");
        writer.Write(json);
        writer.Close();
    }
    /// <summary>
    /// 读档
    /// </summary>
    public void ReadData()
    {
        playerValue.IQ = PlayerPrefs.GetFloat("IQ", 0);
        playerValue.Charm = PlayerPrefs.GetFloat("Charm", 0);
        playerValue.Energy = PlayerPrefs.GetFloat("Energy", 0);
        playerValue.EQ = PlayerPrefs.GetFloat("EQ", 0);
        playerValue.Vanity = PlayerPrefs.GetFloat("Vanity", 0);
        playerValue.Money = PlayerPrefs.GetFloat("Money", 200);
        envValue.Day = PlayerPrefs.GetInt("Day", 1);
        //SceneManager.LoadSceneAsync(PlayerPrefs.GetString("SavedScene"), LoadSceneMode.Single);//读取存储的场景

        string path = Application.streamingAssetsPath + "/relationship.json";

        if (File.Exists(path))
        {
            StreamReader reader = new StreamReader(path);
            string json = reader.ReadToEnd();
            reader.Close();
            Relationship.People = LitJson.JsonMapper.ToObject<List<Person>>(json);
        }
        else
        {
            Debug.LogWarning("未找到RelationshipConfig，将重新创建。");
            Relationship.People = new List<Person>();
            Relationship.People.Add(new Person { Name = "小明", Age = 18,Sex=true }); 
            Relationship.People.Add(new Person { Name = "小红", Age = 18, Sex = false });
            Relationship.People.Add(new Person { Name = "韩梅梅", Age = 18, Sex = false });
            Relationship.People.Add(new Person { Name = "李雷", Age = 18, Sex = true });
        }
        Debug.Log("读档成功");
    }
}



public class EnvironmentValue
{
    Activity activity = new Activity();

    private static EnvironmentValue environment = new EnvironmentValue();
    public static EnvironmentValue EnvInstance => environment;

    private EnvironmentValue() { }
    ValueUI valueUI = ValueUI.PlayerValueUIInstance();

    public Time CurTime { get; set; }
    private int day = 0;
    public int Day
    {
        get => day;
        set
        {
            valueUI.txtDayValue.text = value.ToString();
            day = value;

        }
    }
}



interface IChangeValue
{
    void IncreaseFavorability(string name, float score);
    void ReduceFavorability(string name, float score);
}


struct Goods
{
    public int Price { get; set; }
    public string name;
    public string description;
}
class GoodList
{
    private GoodList() { }
    private static GoodList goodList = new GoodList();
    public static GoodList GoodListInstance()
    {
        return goodList;
    }
    public Goods gameboy = new Goods() { Price = 2200, name = "最新游戏机" };
    public Goods books = new Goods() { Price = 50, name = "书籍" };
    public Goods earphone = new Goods() { Price = 500, name = "爆款耳机" };
    public Goods clothes = new Goods() { Price = 200, name = "热门时尚服装" };
    public Goods phone = new Goods() { Price = 5000, name = "最新鸭梨手机" };
}


