using UnityEngine;
using UnityEngine.Events;
class Activity
{
    //GameObject gameObject;
    PlayerValue playerValue = PlayerValue.PlayerInstance();
    EnvironmentValue envValue = EnvironmentValue.EnvInstance;
    ViewBase viewBase;
    UnityAction<string> PlayVideo;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="gameObject">需要显式的物体</param>
    //public Activity(GameObject gameObject)
    //{
    //    this.gameObject = gameObject;
    //    viewBase = new ViewBase(gameObject);
    //    //viewBase = GameObject.Find("UIManagement").GetComponent<ViewBase>();
    //}
    public Activity()
    {
        PlayVideo = new UnityAction<string>(GameObject.Find("Canvas").transform.Find("Video Player").GetComponent<VideoControl>().Play);//注册Play时间
        //gameObject= GameObject.Find("UniversityScene").transform.Find("Canvas").gameObject;
        viewBase = new ViewBase();
        //viewBase = GameObject.Find("UIManagement").GetComponent<ViewBase>();
    }

    public void Study()//学习
    {
        if (IsEnergyEmpty())
        {
            viewBase.StartShowMessage("您已经没有精力做这件事了");
            return;
        }
        PlayVideo("Study");
        playerValue.Energy += 2;
        playerValue.IQ += 1;
        viewBase.StartShowMessage("经过接近于睡着的思维大碰撞后，您的脑力值增加了");
    }
    public void ReadBooks()//读书
    {
        if (IsEnergyEmpty())
        {
            viewBase.StartShowMessage("您已经没有精力做这件事了");
            return;
        }
        PlayVideo("ReadBooks");
        playerValue.Energy += 2;
        playerValue.IQ += 1;
        viewBase.StartShowMessage("呼噜噜~呼噜噜~，您在睡梦中习得了此书，您的脑力值增加了");
    }
    public void Exercise()//体育锻炼
    {
        if (IsEnergyEmpty())
        {
            viewBase.StartShowMessage("您已经没有精力做这件事了");
            return;
        }
        PlayVideo("Exercise");
        playerValue.Energy += 2;
        // GameObject.Find("SldEnergy").GetComponent<UnityEngine.UI.Slider>().value += 2;

       
        playerValue.Charm += 1;
        viewBase.StartShowMessage("在和朋友的激烈运动后，您的魅力值增加了");
    }
    public void Competition()//竞赛
    {
        playerValue.Energy += 2;
        if (IsEnergyEmpty())
        {
            viewBase.StartShowMessage("您已经没有精力做这件事了");
            return;
        }
        playerValue.IQ += 1;
        if (playerValue.IQ > 400)
        {
            playerValue.Charm += (Random.Range(0, 10) > 6) ? 3 : 0;
            viewBase.StartShowMessage("恭喜您在竞赛中取得了优异的成绩，您的魅力值大幅度增加了。");
        }
    }
    public void Certification()//证书
    {
        playerValue.Energy += 2;
        if (IsEnergyEmpty())
        {
            viewBase.StartShowMessage("您已经没有精力做这件事了");
            return;
        }
        playerValue.IQ += 1;
        int probability = Random.Range(0, 10);
        if (playerValue.IQ > 500 && probability > 6)
        {
            playerValue.Charm += 2;
            viewBase.StartShowMessage("恭喜您获取了XX证书。您的魅力值增加了");
        }
    }
    public void GetMoney(float money)
    {
        playerValue.IncreaseFavorability("Money", money);
    }
    public bool SpendMoney(float money)
    {
        //playerValue.ReduceFavorability("Money", money);
        if (playerValue.Money < money)
            return false;
        playerValue.Money -= money;
        return true;
    }
    /// <summary>
    /// 获得生活费
    /// </summary>
    public void GetlivingCosts()
    {
        int randomMoney = Random.Range(0, 300);
        randomMoney = randomMoney - randomMoney % 10;
        int money = 1500;
        if (Random.Range(0, 2) == 0)
        {
            GetMoney(money + randomMoney);
            viewBase.StartShowMessage($"您终于撑到了发生活费的时刻。妈妈这个月得到了奖金，多给你了{randomMoney}，恭喜您获得了{money + randomMoney}元");
        }
        else
        {
            GetMoney(money - randomMoney);
            viewBase.StartShowMessage($"您终于撑到了发生活费的时刻。爸爸炒股亏了，你这月零花钱少了{randomMoney}，您获得了{money - randomMoney}元");
        }


    }
    public void ParttimeJob()//做兼职
    {
        PlayVideo("ParttimeJob");
        playerValue.Energy -= 2;
        if (IsEnergyEmpty())
        {
            viewBase.StartShowMessage("您已经没有精力做这件事了");
            return;
        }
        GetMoney(40);
        viewBase.StartShowMessage("您辛苦的完成了2小时的兼职，获得了40元");
    }
    public void SellGoods(Goods good)
    {
        GoodList goodList = GoodList.GoodListInstance();
        viewBase.StartShowMessage(string.Format("您成功卖出了物品{0},获得了{1}", good.name, good.Price / 6));
        playerValue.Money += good.Price / 6;
        Debug.LogWarning("此处尚未实现 物品减少");
    }//卖已有商品
    public void BuyGoods(Goods good)
    {
        GoodList goodList = GoodList.GoodListInstance();
        viewBase.StartShowMessage($"您成功购买了物品{good.name},金币减少{good.Price}");
        playerValue.Money -= good.Price;
        Debug.LogWarning("此处尚未实现 背包增加物品");
    }
    public void Eat()//吃饭
    {
        PlayVideo("Eat");
        if (playerValue.Money < 10)
        {
            viewBase.StartShowMessage("bia~bia~bia~bia，您的钱不够了，只能吃空气了。");
            return;
        }
        if (playerValue.Energy<=10&& playerValue.Energy > 8)
        {
            playerValue.Buff = 1.1f;
        }
        //if()
        playerValue.Energy -= 1;
        playerValue.Money -= 10;
        viewBase.StartShowMessage("bia~bia~bia~bia，真好吃，您的精力提升了。");
    }
    public void Sleep()//睡觉
    {
        PlayVideo("Sleep");
        playerValue.Energy = 0;
        //viewBase.ShowTalk("hu~hu~hu，渐渐的，进入了梦乡。");
        //viewBase.StartShowMessage("hu~hu~hu，渐渐的，进入了梦乡。",true);
        envValue.Day += 1;
        MainLine.Instance.EndOfDay();
    }
    public void PlayGames()//玩游戏
    {
        Model.ModelInstance().SaveData();
        //PlayVideo("PlayGame");
        if (IsEnergyEmpty())
        {
            viewBase.StartShowMessage("您已经没有精力做这件事了");
            return;
        }
        playerValue.Energy += 1;
        playerValue.IQ += 1;
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("TankWar");
        
        
        
        //viewBase.StartShowMessage("您玩的很开心。");
    }
    public void HangOut()//娱乐
    {
        PlayVideo("HangOut");
        playerValue.Energy += 1;
        if (IsEnergyEmpty())
        {
            viewBase.StartShowMessage("您已经没有精力做这件事了");
            return;
        }
        playerValue.IQ += 1;
        viewBase.StartShowMessage("您玩的很开心。");
    }

    public bool Exam()//考试
    {
        playerValue.Energy += 8;
        PlayVideo("Exam");
        float score = Random.Range(0.01f, 1f);
        //TODO:考试动画
        if ((playerValue.IQ > 195 && score > 0.1f)||
            (playerValue.IQ > 130 && score > 0.13f)||
            (playerValue.IQ >= 65 && score > 0.2f)||
            (playerValue.IQ < 65 && score > 0.8f)
            )
        {
            viewBase.StartShowMessage("采用了先进的AI改卷子，恭喜您通过了考试！");
            return true;
        }

        //else if (playerValue.IQ > 130 && score > 0.13f)
        //    return true;
        //else if (playerValue.IQ >= 65 && score > 0.2f)
        //    return true;
        //else if (playerValue.IQ < 65 && score > 0.8f)
        //    return true;
        viewBase.StartShowMessage("采用了先进的AI改卷子，糟糕，您挂科了！");
        return false;
    }
    public void Exam2()//方便ActivityFactory调用
    {
        MainLine.Instance.IsReExam = Exam() ? false : true;
        
    }
    public void Ending()
    {
        PlayVideo("Ending2");
        viewBase.StartShowMessage("天下没有不散的宴席，转眼间，大学四年的美丽年代就快到了要结束的时候。\n回首这四年，有过欢乐兴奋、也有痛苦悲伤，付出过汗水泪水，也有收获与成就。\n有人说过，忘记过去就意味着背叛，所以，我们回味过去，为的是未来。\n再见学校，再见教室，再见老师，再见同学，再见我暗恋的女孩。", true);
       // viewBase.StartShowMessage("再见学校，再见教室，再见老师，再见同学，再见我暗恋的女孩。",true);
        //foreach(Person person in Relationship.People)
        //{
        //    if (person.FriendValue >= 50 && Random.Range(0, 1f) > 0.3)
        //    {
        //        PlayVideo("Ending1");
        //    }
        //}
    }
    public void DatingSb(string whom)//约人
    {
        MainLine.Instance.IsDated = true;
        playerValue.EQ += 1;
        viewBase.StartShowMessage(string.Format($"您的情商提升了并且和{whom}的亲密度提升了。"));
        //Relationship.RelationshipInstance().IncreaseFavorability(whom, 1);
        Person w;
        for (int i = 0; i < Relationship.People.Count; ++i)
        {
            w = Relationship.People[i];
            if (w.Name != whom)
                continue;
            
            if (!w.IsFriend && (Random.Range(0, 1f) > 0.9f|| Random.Range(0, 1f) < playerValue.Charm / 100f))
                w.IsFriend = true;
            else if(w.IsFriend)
            {
                if ((w.FriendValue < 10 && Random.Range(0, 1f) > 0.9f)||
                     ((w.FriendValue >= 10 ||w.FriendValue <= 30)&& Random.Range(0, 1f) > 0.7f)||
                     (w.FriendValue >= 30 && Random.Range(0, 1f) > 0.05f)
                    )
                    AcceptDating(w);
                break;
            }
            RefuseDating(w);
        }

    }
    private void RefuseDating(Person person)//拒绝邀请
    {
        viewBase.StartShowMessage($"对不起，{person}拒绝了你的邀请，原因不详...");
    }
    public void AcceptDating(Person person)
    {
        ++person.FriendValue;
        viewBase.StartShowMessage($"{person}接受了你的邀请，");
    }
    bool IsEnergyEmpty()
    {
        if (playerValue.Energy >= 10)
            return true;
        return false;
    }
}

