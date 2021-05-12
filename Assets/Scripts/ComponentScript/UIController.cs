using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using LitJson;
using UnityEngine.Events;

public class UIController : MonoBehaviour
{
    public Sprite[] Background;
    private GameObject university;
    private GameObject background;
    private Image imgBackground;
    private GameObject btnMapSecond, btnActivitySecond;
    private GameObject btnMain, btnMain2;
    string btnMainText, btnMain2Text;
    ActivityFactory activityFactory;
    UnityAction action, action2;
    UnityAction<string> PlayVideo;
    GameObject[] secondMenus;
    GameObject[] roles;//0是roles,1是bagrole
    void Awake()
    {

        Initialization();
        AwakeGoodsInit();
        AddMapSecondClickEvent();
        Model.ModelInstance().ReadData();
        AddUIBtnClickEvent();
    }
    /// <summary>
    /// 初始化变量
    /// </summary>
    void Initialization()
    {
        university = GameObject.Find("UniversityScene").transform.Find("Canvas").gameObject;
        background = university.transform.Find("Background").gameObject;
        imgBackground = background.GetComponent<Image>();
        btnMapSecond = university.transform.Find("UIButton").Find("BtnMap").Find("BtnSecond").gameObject;
        btnActivitySecond = university.transform.Find("UIButton").Find("BtnActivity").Find("BtnSecond").gameObject;

        btnMain = btnActivitySecond.transform.Find("BtnMain").gameObject;
        btnMain2 = btnActivitySecond.transform.Find("BtnMain2").gameObject;
        activityFactory = new ActivityFactory();
        
        roles = GameObject.FindGameObjectsWithTag("Roles");
        foreach(GameObject role in roles)
        {
            role.SetActive(false);
        }

        PlayVideo = new UnityAction<string>(GameObject.Find("Canvas").transform.Find("Video Player").GetComponent<VideoControl>().Play);//注册Play时间
        
        //初始化格子ui
        //bagPanel = GameObject.Find("BagPanel");
        //uiGrids = bagPanel.GetComponentsInChildren<UIGrid>();
        //uiGrids = GameObject.FindGameObjectsWithTag("BagGrid");

    }
    void Start()
    {

        //初始化SecondMenu
        secondMenus = GameObject.FindGameObjectsWithTag("SecondMenu");
        foreach (GameObject second in secondMenus)
        {
            second.SetActive(false);
        }
        ShowPerson(Relationship.People[0], 0);

    }
    void Update()
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

    /// <summary>
    /// “关闭对话”协程
    /// </summary>
    /// <param name="imgTxtBack"></param>
    /// <returns></returns>
    IEnumerator CloseMessage(GameObject imgTxtBack)
    {
        yield return new WaitForSeconds(3);
        imgTxtBack.SetActive(false);

    }
    private IEnumerator ShowMessage(string text, GameObject imgTxtBack, GameObject txtMessage, bool isDelay = false)
    {
        if (isDelay)
        {
            yield return new WaitForSeconds(3);
        }     
        imgTxtBack.SetActive(true);
        txtMessage.GetComponent<Text>().text = text;
        StartCoroutine(CloseMessage(imgTxtBack));
    }
    /// <summary>
    /// 开启“关闭对话”的协程
    /// </summary>
    /// <param name="imgTxtBack"></param>
    public void StartShowMessage(string text, GameObject imgTxtBack,GameObject txtMessage, bool isDelay = false)
    {
        StartCoroutine(ShowMessage(text, imgTxtBack, txtMessage,isDelay));
    }
    //public UnityAction<string> MapClick;
    /// <summary>
    /// 地图场景 点击事件
    /// </summary>
    public void ClickMapScene()
    {
        var button = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        PlayVideo("Place");
        switch (button.name)
        {
            case "BtnPlayGround":
                imgBackground.sprite = Background[0];
                break;
            case "BtnBascket":
                imgBackground.sprite = Background[6];
                break;
            case "BtnRestaurant":
                imgBackground.sprite = Background[14];
                break;
            case "BtnDorm":
                imgBackground.sprite = Background[4];
                break;
            case "BtnGate":
                imgBackground.sprite = Background[3];
                break;
            case "BtnLibrary":
                imgBackground.sprite = Background[13];
                break;
            case "BtnGarden":
                imgBackground.sprite = Background[9];
                break;
            case "BtnClassroom":
                imgBackground.sprite = Background[8];
                break;
            case "BtnTeachingBuilding":
                imgBackground.sprite = Background[7];
                break;
            case "BtnMall":
                imgBackground.sprite = Background[5];
                break;
        }
        btnMapSecond.SetActive(false);
        activityFactory.Act(imgBackground.sprite.name, out btnMainText, out action, out btnMain2Text, out action2, btnActivitySecond);
        action += UIBtnEvent;
        action2 += UIBtnEvent;
        btnMain.transform.Find("Text").GetComponent<Text>().text = btnMainText;
        btnMain2.transform.Find("Text").GetComponent<Text>().text = btnMain2Text;
        if (imgBackground.sprite.name == "小商店2")
        {
            btnMain.GetComponent<Button>().onClick.RemoveAllListeners();
            btnMain.GetComponent<Button>().onClick.AddListener(UIBtnEvent);
            btnMain2.GetComponent<Button>().onClick.RemoveAllListeners();
            btnMain2.GetComponent<Button>().onClick.AddListener(UIBtnEvent);
        }
        else
        {
            btnMain.GetComponent<Button>().onClick.RemoveAllListeners();
            btnMain.GetComponent<Button>().onClick.AddListener(action);
        }
        btnActivitySecond.SetActive(false);

        MeetSb();

    }

    /// <summary>
    /// 行动 按钮点击事件
    /// </summary>
    public void AddUIBtnClickEvent()
    {
        GameObject uiBtn = GameObject.Find("UIButton");

        for (int i = 0; i < uiBtn.transform.childCount; ++i)
        {
            uiBtn.transform.GetChild(i).gameObject.GetComponent<Button>().onClick.AddListener(UIBtnEvent);
        }
    }
    /// <summary>
    /// 添加按钮事件。为行动按钮 添加事件
    /// </summary>
    public void AddMapSecondClickEvent()
    {
        //SecondMenu下按钮事件
        for (int i = 0; i < btnMapSecond.transform.childCount; ++i)
        {
            btnMapSecond.transform.GetChild(i).gameObject.GetComponent<Button>().onClick.AddListener(ClickMapScene);
        }
    }
    public void HideUIBtnChildren(GameObject clickBtn, GameObject[] secondMenus, out GameObject second)
    {
        second = null;
        string clickBtnName = clickBtn.name;
        string btnTxt = clickBtn.transform.Find("Text").GetComponent<Text>()?.text;
        string obName;
        foreach (GameObject ob in secondMenus)
        {
            obName = ob.GetComponentInParent<Transform>().gameObject.name;
            if (obName != clickBtnName)
            {
                if ((clickBtnName == "BtnBagReturn" || clickBtnName == "BtnBagpack") && ob.name == "BagPanel")
                {
                    second = ob;
                    continue;
                }
                else if ((clickBtnName == "BtnMain2" || clickBtnName == "BtnMain") && ob.name == "BagPanel" && (btnTxt == "购买" || btnTxt == "出售"))
                {
                    second = ob;
                    continue;
                }
                //else if ((name == "BtnMain" || name == "BtnShopReturn") && ob.name == "ShopPanel")
                //{
                //    second = ob;
                //    continue;
                //}
                ob.SetActive(false);
            }
            else
                second = ob;
            //Debug.LogWarning("孩子:"+ob.GetComponentInParent<Transform>().gameObject.name);
            //Debug.LogWarning("父："+name);
        }
    }
    public void UIBtnEvent()
    {
        GameObject btn = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        GameObject second;
        switch (btn.name)
        {
            case "BtnSave":
                Model model = Model.ModelInstance();
                model.SaveData();
                Debug.Log("存档成功");
                break;
            case "BtnMain":
                GoodsInitialize("shop");
                break;
            case "BtnMain2":
                GoodsInitialize("bag");
                break;
            case "BtnBagpack":
                GoodsInitialize("bag");
                break;
        }
        HideUIBtnChildren(btn, secondMenus, out second);
        //UIBtnEvent(btn);
        if (second == null)
        { }
        else if (!second.activeSelf)
            second.SetActive(true);
        else
            second.SetActive(false);

    }

    public void MeetSb()
    {
        if (Random.Range(0, 1f) < 0.8f)
        {
            return;
        }

        int index = Random.Range(1, 3);
        Person person = Relationship.People[index];
        ShowPerson(person, 0);

       
        ViewBase viewbase = new ViewBase();
        viewbase.StartShowMessage($"好巧，你是{person.Name}吧,");
    }

    /// <summary>
    /// 显示人物
    /// </summary>
    /// <param name="person">显示的人物</param>
    /// <param name="index">0是主界面，1是背包</param>
    public void ShowPerson(Person person,short index)
    {
        Sprite img = Resources.Load<Sprite>(person.ImgPath);
        //Instantiate(img, roles.transform, true);
        roles[index].GetComponent<Image>().sprite = img;
        roles[index].SetActive(true);
    }
    #region 背包、商店
    // 背包格子数量
    public int gridCount = 16;
    // 物品字典
    private Dictionary<int, BaseItem> mItemDic = null;
    /// <summary>
    /// 初始化背包、商店等物品存放系统
    /// </summary>
    /// <param name="container">背包or商店 文件名称</param>
    private void GoodsInitialize(string container)
    {

        //Clear();

        LoadItemCfg();

        //存档物品数据
        List<BagData> dataList = Load(container);
        // 初始化背包管理器
        BagManager.Instance.Init(gridCount, GetItem, dataList, container);

        BagManager.Instance.RefreshGridsUI();

    }
    private void AwakeGoodsInit()
    {

        LoadItemCfg();

        //存档物品数据
        List<BagData> dataList = Load("bag");
        // 初始化背包管理器
        BagManager.Instance.Init(gridCount, GetItem, dataList);

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
            string price = data["price"].ToString();
            BaseItem item = new BaseItem(id, name, capacity, icon, des, price);
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

    #endregion
}
class ActivityFactory
{
    Activity activity = new Activity();
    internal void Act(string backgroundName, out string actname, out UnityEngine.Events.UnityAction action, out string actname2, out UnityEngine.Events.UnityAction action2, GameObject btnActivitySecond)
    {
        actname = "";
        actname2 = null;
        action = null;
        action2 = null;// btnActivitySecond.SetActive(false);
        switch (backgroundName)
        {

            case "体育场":
                action += activity.Exercise;
                actname = "锻炼";
                break;
            case "小商店2":
                actname = "购买";
                actname2 = "出售";
                break;
            case "宿舍":
                action += activity.Sleep;
                actname = "睡觉";
                break;
            case "教室":
                action += activity.Study;
                actname = "学习";
                break;
            case "操场":
                action += activity.Exercise;
                actname = "打球";
                break;
            case "图书馆里":
                action += activity.Study;
                actname = "读书";

                break;
            case "食堂":
                action += activity.Eat;
                actname = "吃饭";
                action2 += activity.ParttimeJob;
                actname2 = "食堂兼职";
                break;
        }

    }

}
class ValueUI
{
    internal Text txtIQValue;
    internal Text txtEQValue;
    internal Text txtCharmValue;
    internal Text txtMoneyValue;
    internal Text txtDayValue;
    internal Text txtTimeValue;
    internal Slider sldEnergy;

    private ValueUI()
    {
        //playerValue = PlayerValue.PlayerInstance();
        txtIQValue = GameObject.Find("TxtIQValue").GetComponent<Text>();
        txtEQValue = GameObject.Find("TxtEQValue").GetComponent<Text>();
        txtCharmValue = GameObject.Find("TxtCharmValue").GetComponent<Text>();
        txtMoneyValue = GameObject.Find("TxtMoneyValue").GetComponent<Text>();
        txtDayValue = GameObject.Find("txtDayValue").GetComponent<Text>();
        sldEnergy = GameObject.Find("SldEnergy").GetComponent<UnityEngine.UI.Slider>();
    }
    static ValueUI playerValueUI = new ValueUI();
    public static ValueUI PlayerValueUIInstance()
    {
        return playerValueUI;
    }
}