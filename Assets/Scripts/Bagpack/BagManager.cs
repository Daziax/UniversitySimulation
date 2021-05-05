using System;
using System.Collections.Generic;

public class BagManager
{
    #region 单例
    private static BagManager mInstance = null;
    public static BagManager Instance
    {
        get
        {
            if (mInstance == null)
                mInstance = new BagManager();
            return mInstance;
        }
    }
    private BagManager() { }
    #endregion
    //"bag" or "shop"
    public string Kind { get; set; }

    // 刷新UI的回调
    public Action<BagGrid, bool> OnRefreshGridUI = null;
    // 刷新UI的回调
    public Action RefreshGridsUI = null;
    // 背包已满回调
    public Action OnFilled = null;
    private Func<int, BaseItem> GetItemByIdHandler = null;
    private Func<string, BaseItem> GetItemByNameHandler = null;

    // 背包格子对象数组
    private BagGrid[] mBagGrids = null;
    public BagGrid[] BagGrids
    {
        get
        {
            return mBagGrids;
        }
    }
    void RefreshAllGrids()
    {
        foreach(BagGrid grid in mBagGrids)
        {
            OnRefreshGridUI(grid, true);
        }
    }
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="bagGridCount">背包格子数量</param>
    /// <param name="datas">背包数据</param>
    public virtual void Init(int bagGridCount, Func<int, BaseItem> getItemHandle, List<BagData> datas = null,string pathName="bag")
    {
        GetItemByIdHandler = getItemHandle;
        Kind = pathName;

        // 初始化背包格子数组
        mBagGrids = new BagGrid[bagGridCount];
        for (int i = 0; i < mBagGrids.Length; i++)
        {
            mBagGrids[i] = new BagGrid(i);
        }

        // 初始化背包物品数据
        if (datas != null)
        {
            foreach (BagData data in datas)
            {
                if (data == null) continue;
                BaseItem item = GetItemById(data.ID);
                if (item != null)
                {
                    mBagGrids[data.Index].SetItem(GetItemById(data.ID), data.Amount);
                }
            }
        }
    }

    /// <summary>
    /// 通过物品ID添加物品到背包
    /// </summary>
    /// <param name="id">物品ID</param>
    /// <param name="amount">数量</param>
    public virtual void AddItem(int id, int amount = 1)
    {
        BaseItem item = GetItemById(id);
        AddItem(item, amount);
    }

    /// <summary>
    /// 通过物品对象添加物品
    /// </summary>
    /// <param name="item">需要添加的物品对象</param>
    /// <param name="amount">数量</param>
    public virtual void AddItem(BaseItem item, int amount = 1)
    {
        if (item == null)
        {
            throw new System.Exception("Bag AddItem Exception:需要添加到背包的Item为null!");
        }
        else
        {
            BagGrid grid = null;
            while (amount > 0)
            {
                grid = FindUsableGrid(item.ID);
                if (grid != null)
                {
                    int remainCapacity = grid.IsEmpty ? item.Capacity : (grid.Item.Capacity - grid.Amount);
                    int tmp = amount;
                    if (tmp > remainCapacity)
                        tmp = remainCapacity;
                    if (grid.IsEmpty)
                    {
                        grid.SetItem(item, tmp);
                        //TODO 刷新UI显示，新的物品
                        OnRefreshGridUI(grid, true);
                    }
                    else
                    {
                        grid.AddAmount(tmp);
                        //TODO 刷新UI显示，刷新数量
                        OnRefreshGridUI(grid, false);
                    }
                    // 剩余没存的物品个数，如果<=0则已经存储完
                    amount -= remainCapacity;
                }
                else
                {
                    //TODO 背包已满
                    break;
                }
            }
        }
    }

    /// <summary>
    /// 根据格子索引减少物品数量
    /// </summary>
    /// <param name="index">格子索引</param>
    /// <param name="amount">物品数量</param>
    public virtual void SubItemByIndex(int index, int amount = 1)
    {
        BagGrid grid = FindGridByIndex(index);
        if (grid == null) return;
        grid.SubAmount(amount);
        OnRefreshGridUI(grid, false);
    }


    /// <summary>
    /// 通过物品ID减少物品数量
    /// </summary>
    /// <param name="id">物品ID</param>
    /// <param name="amount">amount</param>
    public virtual void SubItemById(int id, int amount = 1)
    {
        while (amount > 0)
        {
            BagGrid grid = FindGridById(id);
            if (grid == null) break;
            // 如果格子中的物品数量大于需要减少的数量
            if (grid.Amount >= amount)
            {
                grid.SubAmount(amount);
                amount = 0;
            }
            else
            {
                amount -= grid.Amount;
                grid.SubAmount(grid.Amount);
            }

            ////TODO 刷新UI
            //if (grid.IsEmpty)
            //{
            //    // 清空格子UI显示
            //    OnRefreshGridUI(grid, false);
            //}
            //else
            //{
            //    // 刷新数量显示
            //    OnRefreshGridUI(grid, false);
            //}
            OnRefreshGridUI(grid, false);
        }
    }

    /// <summary>
    /// 判断物品数量是否充足
    /// </summary>
    /// <param name="id">物品ID</param>
    /// <param name="amount">需进行比较的物品个数</param>
    /// <returns></returns>
    public bool IsEnough(int id, int amount)
    {
        return GetItemCount(id) >= amount;
    }

    /// <summary>
    /// 获取背包中对应ID物品的总个数
    /// </summary>
    /// <param name="id">物品ID</param>
    /// <returns></returns>
    public int GetItemCount(int id)
    {
        int count = 0;
        if (mBagGrids != null)
        {
            for (int i = 0; i < mBagGrids.Length; i++)
            {
                if (mBagGrids[i] == null || mBagGrids[i].IsEmpty) continue;
                if (mBagGrids[i].Item.ID == id)
                    count += mBagGrids[i].Amount;
            }
        }
        return count;
    }

    /// <summary>
    /// 获取背包中物品存储数据列表（存档用）
    /// </summary>
    /// <returns></returns>
    public List<BagData> GetBagDatas()
    {
        if (mBagGrids == null) return null;
        List<BagData> list = new List<BagData>();
        for (int i = 0; i < mBagGrids.Length; i++)
        {
            if (mBagGrids[i] == null || mBagGrids[i].IsEmpty) continue;
            list.Add(new BagData
            {
                ID = mBagGrids[i].Item.ID,
                Index = mBagGrids[i].Index,
                Amount = mBagGrids[i].Amount
            });
        }
        return list;
    }

    /// <summary>
    ///  通过物品ID获取物品
    /// </summary>
    /// <param name="id">物品ID</param>
    /// <returns></returns>
    public BaseItem GetItemById(int id)
    {
        if (GetItemByIdHandler != null)
        {
            return GetItemByIdHandler(id);
        }
        return null;
    }
    /// <summary>
    /// 通过名字获取物品
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public BaseItem GetItemByName(string name)
    {
        if (GetItemByNameHandler != null)
        {
            return GetItemByNameHandler(name);
        }
        return null;
    }


    /// <summary>
    /// 根据格子索引查找格子
    /// </summary>
    /// <param name="index">格子索引</param>
    /// <returns>查找到的格子</returns>
    public BagGrid FindGridByIndex(int index)
    {
        if (mBagGrids == null || index < 0 || index >= mBagGrids.Length)
            return null;
        return mBagGrids[index];
    }

    /// <summary>
    /// 根据物品ID查找格子(返回第一个查找到的格子)
    /// </summary>
    /// <param name="id">物品ID</param>
    /// <returns>查找到格子</returns>
    public BagGrid FindGridById(int id)
    {
        if (mBagGrids == null) return null;
        for (int i = 0; i < mBagGrids.Length; i++)
        {
            if (mBagGrids[i] == null || mBagGrids[i].IsEmpty) continue;
            if (mBagGrids[i].Item.ID == id)
                return mBagGrids[i];
        }
        return null;
    }

    /// <summary>
    /// 查找可用的格子，返回格子索引，没有找到返回-1
    /// </summary>
    /// <param name="id">物品ID</param>
    /// <returns>查找到的格子</returns>
    protected virtual BagGrid FindUsableGrid(int id)
    {
        BagGrid grid = null;
        for (int i = 0; i < mBagGrids.Length; i++)
        {
            if (mBagGrids[i] == null)
            {
                continue;
            }
            // 空格子
            else if (mBagGrids[i].IsEmpty)
            {
                if (grid == null)
                {
                    grid = mBagGrids[i];
                }
            }
            // 未放满
            else if (mBagGrids[i].Item.ID == id && mBagGrids[i].Amount < mBagGrids[i].Item.Capacity)
            {
                grid = mBagGrids[i];
                break;
            }
        }
        return grid;
    }



#region 测试
public override string ToString()
    {
        string info = "";
        foreach (var g in mBagGrids)
        {
            if (!g.IsEmpty)
            {
                info += (g.ToString() + "\n");
            }
        }
        return info;
    }
    //END
    #endregion
}