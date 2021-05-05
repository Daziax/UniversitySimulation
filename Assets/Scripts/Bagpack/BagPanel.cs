using UnityEngine;
using UnityEngine.UI;

public class BagPanel : MonoBehaviour
{
    public UIGrid[] uiGrids;
    public GameObject mInfo;
    private void Start()
    {

    }
    private void Awake()
    {
        // 注册背包管理器中的刷新UI回调
        BagManager.Instance.OnRefreshGridUI += OnRefreshGrid;
        BagManager.Instance.RefreshGridsUI += RefreshGridsUI;
        // 获取所有的格子UI
        Transform content = transform.Find("Content");
        uiGrids = new UIGrid[content.childCount];
        for (int i = 0; i < content.childCount; i++)
        {
            // 给每个格子挂载上UIGrid脚本，并初始化
            uiGrids[i] = content.GetChild(i).gameObject.AddComponent<UIGrid>();
            uiGrids[i].Init(i, this);
        }

        // 刷新所有格子的显示
        var tmp = BagManager.Instance.BagGrids;
        foreach (var item in tmp)
        {
            if (item != null && !item.IsEmpty)
            {
                OnRefreshGrid(item, true);
            }
        }
    }
    public void RefreshGridsUI()
    {
        // 获取所有的格子UI
        Transform content = transform.Find("Content");
        uiGrids = new UIGrid[content.childCount];
        for (int i = 0; i < content.childCount; i++)
        {
            // 给每个格子挂载上UIGrid脚本，并初始化
            UIGrid grid = content.GetChild(i).gameObject.GetComponent<UIGrid>();
            if (grid != null)
                Destroy(grid);
            uiGrids[i] = content.GetChild(i).gameObject.AddComponent<UIGrid>();
            uiGrids[i].Init(i, this);
        }

        // 刷新所有格子的显示
        var tmp = BagManager.Instance.BagGrids;
        foreach (var item in tmp)
        {
            if (item != null && !item.IsEmpty)
            {
                OnRefreshGrid(item, true);
            }
        }
    }
    // 显示物品信息，传入点击的格子所有
    public void ShowInfo(int index = -1)
    {
        if (index != -1)
        {
            BagGrid grid = BagManager.Instance.FindGridByIndex(index);
            if (!grid.IsEmpty)
            {
                BaseItem item = grid.Item;
                string info = string.Format("名称: {0}\n价格: {1}\n说明: {2}", item.Name, item.Price, item.Description);
                mInfo.GetComponentInChildren<Text>().text = info;
                mInfo.SetActive(true);
            }
            else
            {
                mInfo.SetActive(false);
            }
        }
    }

    // 刷新格子显示，传入格子数据对象，isNew表示该格子是否为新添加物品（原本没有物品）
    public void OnRefreshGrid(BagGrid grid, bool isNew)
    {
        foreach (var item in uiGrids)
        {
            if (item.index == grid.Index)
            {
                if (grid.IsEmpty)
                {
                    item.Clear();
                    if (mInfo.gameObject.activeSelf)
                    {
                        mInfo.gameObject.SetActive(false);
                    }
                }
                else
                {
                    if (isNew)
                    {
                        Sprite sprite = Resources.Load<Sprite>(grid.Item.Icon);
                        item.RefreshUI(sprite, grid.Amount);
                    }
                    else
                    {
                        item.RefreshUI(grid.Amount);
                    }
                }
                break;
            }
        }
    }
    
}