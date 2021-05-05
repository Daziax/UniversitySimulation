using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseItem
{
    public BaseItem(int id, string name, int capacity, string icon, string des,string price)
    {
        ID = id;
        Name = name;
        Capacity = capacity;
        Icon = icon;
        Description = des;
        Price = price;
    }

    /// <summary>
    /// 物品ID
    /// </summary>
    public int ID { get; protected set; }
    /// <summary>
    /// 物品名称
    /// </summary>
    public string Name 
    { 
        get;
        protected set; 
    }
    /// <summary>
    /// 容量: 每个格子能够容纳该物品的数量
    /// </summary>
    public int Capacity { get; protected set; }
    /// <summary>
    /// 图标路径
    /// </summary>
    public string Icon { get; protected set; }
    /// <summary>
    /// 物品描述
    /// </summary>
    public string Description { get; protected set; }
    /// <summary>
    /// 物品价格
    /// </summary>
    public string Price { get; protected set; }

    public override string ToString()
    {
        return string.Format("ID:{0},Name:{1},Capacity:{2},Icon:{3},Des:{4}", ID, Name, Capacity, Icon, Description);
    }

}
