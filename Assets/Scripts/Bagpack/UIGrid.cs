using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using System.IO;
using LitJson;

public class UIGrid : MonoBehaviour, IPointerDownHandler
{
    public int index;
    public BagPanel panel;
    public Text txtAmount;
    public Image imgIcon;
    public string kind;

    // 初始化，传入格子索引和背包面板对象
    public void Init(int index, BagPanel panel)
    {
        this.index = index;
        this.panel = panel;
        txtAmount = transform.Find("TxtAmount").GetComponent<Text>();
        imgIcon = transform.Find("Sprite").GetComponent<Image>();
        kind = BagManager.Instance.Kind;
        Clear();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // 鼠标左键按下，显示物品信息
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            panel.ShowInfo(index);
        }
        // 不是鼠标右键按下就减少数量
        else
        {
            Activity activity = new Activity();
            ViewBase viewBase = new ViewBase();
            BaseItem item = BagManager.Instance.FindGridByIndex(index).Item;

            BagManager.Instance.SubItemByIndex(index);
            if (kind == "bag") //卖
            {
                float money = float.Parse(item.Price) * Random.Range(0.3f, 0.6f);
                activity.GetMoney(money);
                viewBase.StartShowMessage($"根据市场行情，您卖了{money}元。");
            }
            else if (kind == "shop") //买
            {
                List<BagData> bagList = Load("bag");
                SaveBag(AddInBag( bagList,item));
                float money = float.Parse(item.Price);
                activity.SpendMoney(money);
                viewBase.StartShowMessage($"您花了{money}元购买了{item.Name}。");
            }
            

            Save(kind);
        }
        List<BagData> Load(string savedFileName)
        {
            string path = Application.streamingAssetsPath + $"/{savedFileName}.json";
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
        void SaveBag(List<BagData> list)
        {
            string json = LitJson.JsonMapper.ToJson(list);
            string path = Application.streamingAssetsPath + $"/bag.json";
            StreamWriter writer = new StreamWriter(path);
            writer.Write(json);
            writer.Close();
        }
        List<BagData> AddInBag( List<BagData> list, BaseItem good,int amount = 1)
        {
            //BagData data = FindGridByIndex(index,list);
            //if (data == null) return list;
            //if(data.Amount<1)
            //{
            //    list.RemoveAt(index);
            //    for (int i=index;i<list.Count;i++)
            //    {
            //        list[index].Index = i;
            //    }
            //    return list;
            //}
            //data.Amount -= 1;
            //return list;

            BagData data = FindGridByID(good.ID, list);
            if (data == null)
            {
                list.Add(new BagData() { ID = good.ID, Amount = 1, Index = list.Count });
            }
            else
                data.Amount += 1;
            return list;
        }
        BagData FindGridByID(int id,List<BagData> list)
        {
            //if (list == null || index < 0 || index >= list.Count)
            //    return null;
            foreach(BagData item in list)
            {
                if (item.ID == id)
                {
                    return item;
                }
            }
            return null;
        }

    }
    private void SortIndex(ref List<BagData> list)
    {
        for(int i=0;i<list.Count;++i)
        {
            list[i].Index = i;
        }
    }

    /// <summary>
    /// 保存背包
    /// </summary>
    private void Save(string kind)
    {
        List<BagData> list = BagManager.Instance.GetBagDatas();
        SortIndex(ref list);
        string json = LitJson.JsonMapper.ToJson(list);
        string path = Application.streamingAssetsPath + $"/{kind}.json";
        StreamWriter writer = new StreamWriter(path);
        writer.Write(json);
        writer.Close();
    }

    // 清空格子显示
    public void Clear()
    {
        imgIcon.gameObject.SetActive(false);
        imgIcon.sprite = null;
        txtAmount.text = "";
        txtAmount.gameObject.SetActive(false);
    }

    // 刷新显示
    public void RefreshUI(Sprite sprite, int amount)
    {
        if (imgIcon.gameObject.activeSelf == false)
        {
            imgIcon.sprite = sprite;
            imgIcon.gameObject.SetActive(true);
        }
        RefreshUI(amount);
    }

    // 刷新显示
    public void RefreshUI(int amount)
    {
        if (amount <= 1)
        {
            txtAmount.gameObject.SetActive(false);
        }
        else
        {
            if (txtAmount.gameObject.activeSelf == false)
                txtAmount.gameObject.SetActive(true);
            txtAmount.text = "x" + amount;
        }
    }
}
