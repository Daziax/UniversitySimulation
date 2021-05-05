using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class Test : MonoBehaviour
{
    // 背包格子数量
    public int gridCount = 25;
    // 物品字典
    private Dictionary<int, BaseItem> mItemDic = null;

    private void Awake()
    {
        GoodsInitialize();
       
    }

    private void GoodsInitialize()
    {
        LoadItemCfg();

        //存档物品数据
        List<BagData> dataList = Load("bag");

        // 初始化背包管理器
        BagManager.Instance.Init(gridCount, GetItem, dataList);
    }
    private void Update()
    {
        // 按Q添加物品
        if (Input.GetKeyDown(KeyCode.Q))
        {
            int id = UnityEngine.Random.Range(1001, 1005);
            BagManager.Instance.AddItem(id);
            Debug.Log("添加物品,ID:" + id);
        }

        // 按A输出背包信息
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log(this.ToString());
        }

        // 按W减少物品
        if (Input.GetKeyDown(KeyCode.W))
        {
            BagManager.Instance.SubItemByIndex(0);
            Debug.Log("减少物品");
        }

        // 按E键存档
        if (Input.GetKeyDown(KeyCode.E))
        {
            // 存档
            Save();
        }
    }

    private void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 26;
        style.normal.textColor = Color.blue;
        
        GUILayout.Label("Q键添加物品", style);
        GUILayout.Label("鼠标左键点击物品显示详情", style);
        GUILayout.Label("鼠标右键出售物品", style);
        
    }

    /// <summary>
    /// 加载背包或商店存档数据
    /// </summary>
    /// <param name="savedFileName">存档文件名称，可为"bag"或"shop"</param>
    /// <returns></returns>
    private List<BagData> Load(string savedFileName)
    {
        string path = Application.streamingAssetsPath + $"/{savedFileName}.json";
        //string path = Application.streamingAssetsPath + "/bag.json";
        if (File.Exists(path))
        {
            StreamReader reader = new StreamReader(path);
            string json = reader.ReadToEnd();
            reader.Close();
            List<BagData> list = JsonMapper.ToObject<List<BagData>>(json);
            return list;
        }
        else
        {
            return null;
        }
    }

    // 存档
    private void Save()
    {
        List<BagData> list = BagManager.Instance.GetBagDatas();
        string json = LitJson.JsonMapper.ToJson(list);
        string path = Application.streamingAssetsPath + "/bag.json";
        StreamWriter writer = new StreamWriter(path);
        writer.Write(json);
        writer.Close();
    }

    /// <summary>
    /// 读取所有物品配置文件
    /// </summary>
    private void LoadItemCfg()
    {
        mItemDic = new Dictionary<int, BaseItem>();

        string json = Resources.Load<TextAsset>("Configs/item").text;
        LitJson.JsonData configs = LitJson.JsonMapper.ToObject(json);
        foreach (LitJson.JsonData data in configs)
        {
            int id = int.Parse(data["id"].ToString());
            int capacity = int.Parse(data["capacity"].ToString());
            string name = data["name"].ToString();
            string icon = data["icon"].ToString();
            string des = data["description"].ToString();
            string price =  data["price"].ToString();
            BaseItem item = new BaseItem(id, name, capacity, icon, des,price);
            mItemDic.Add(id, item);
        }
    }

    // 获取物品数据
    private BaseItem GetItem(int id)
    {
        if (mItemDic.ContainsKey(id))
            return mItemDic[id];
        return null;
    }
}
