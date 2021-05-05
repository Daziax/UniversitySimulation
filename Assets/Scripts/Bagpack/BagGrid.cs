using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagGrid
{
    protected int mIndex = -1;
    /// <summary>
    /// 格子索引，默认为-1
    /// </summary>
    public int Index
    {
        get
        {
            return mIndex;
        }
    }

    protected BaseItem mItem = null;
    /// <summary>
    /// 格子下的物品对象，没有物品是为空
    /// </summary>
    public BaseItem Item
    {
        get
        {
            return mItem;
        }
    }

    protected int mAmount = 0;
    /// <summary>
    /// 格子里的物品数量，没有物品时为0
    /// </summary>
    public int Amount
    {
        get
        {
            return mAmount;
        }
    }

    /// <summary>
    /// 格子是否为空
    /// </summary>
    public bool IsEmpty
    {
        get { return mItem == null; }
    }

    public BagGrid(int index)
    {
        mIndex = index;
        Clear();
    }

    /// <summary>
    /// 设置物品
    /// </summary>
    /// <param name="item">物品对象</param>
    /// <param name="amount">数量</param>
    public void SetItem(BaseItem item, int amount)
    {
        if (item != mItem)
            mItem = item;
        mAmount = amount;
    }

    /// <summary>
    /// 添加数量
    /// </summary>
    /// <param name="amount">增加的个数</param>
    /// <returns>格子中的物品数量</returns>
    public int AddAmount(int amount)
    {
        mAmount += amount;
        return mAmount;
    }

    /// <summary>
    /// 减少数量
    /// </summary>
    /// <param name="amount">减少的个数</param>
    /// <returns>格子中剩余的物品数量</returns>
    public void SubAmount(int amount)
    {
        mAmount -= amount;
        if (mAmount <= 0)
            Clear();
    }

    // 清空格子
    protected virtual void Clear()
    {
        mItem = null;
        mAmount = 0;
    }

    public override string ToString()
    {
        return string.Format("Index:{0}, Item [{1}], Amount:{2}", mIndex, mItem.ToString(), mAmount);
    }
}
