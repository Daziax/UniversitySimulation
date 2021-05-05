using UnityEngine;

class Activity
{
    //GameObject gameObject;
    PlayerValue playerValue = PlayerValue.PlayerInstance();
    EnvironmentValue envValue = EnvironmentValue.EnvInstance;
    ViewBase viewBase;
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
        //gameObject= GameObject.Find("UniversityScene").transform.Find("Canvas").gameObject;
        viewBase = new ViewBase();
        //viewBase = GameObject.Find("UIManagement").GetComponent<ViewBase>();
    }

    public void Study()//学习
    {
        playerValue.Energy += 2;
        if (IsEnergyEmpty())
        {
            viewBase.ShowTalk("您已经没有精力做这件事了");
            return;
        }
        playerValue.IQ += 1;
        viewBase.ShowTalk("经过接近于睡着的思维大碰撞后，您的脑力值增加了");
    }
    public void Exercise()//体育锻炼
    {

        playerValue.Energy += 2;
        // GameObject.Find("SldEnergy").GetComponent<UnityEngine.UI.Slider>().value += 2;

        if (IsEnergyEmpty())
        {
            viewBase.ShowTalk("您已经没有精力做这件事了");
            return;
        }
        playerValue.Charm += 1;
        viewBase.ShowTalk("在和朋友的激烈运动后，您的魅力值增加了");
    }
    public void Competition()//竞赛
    {
        playerValue.Energy += 2;
        if (IsEnergyEmpty())
        {
            viewBase.ShowTalk("您已经没有精力做这件事了");
            return;
        }
        playerValue.IQ += 1;
        if (playerValue.IQ > 400)
        {
            playerValue.Charm += (Random.Range(0, 10) > 6) ? 3 : 0;
            viewBase.ShowTalk("恭喜您在竞赛中取得了优异的成绩，您的魅力值大幅度增加了。");
        }
    }
    public void Certification()//证书
    {
        playerValue.Energy += 2;
        if (IsEnergyEmpty())
        {
            viewBase.ShowBlackMessage("您已经没有精力做这件事了");
            return;
        }
        playerValue.IQ += 1;
        int probability = Random.Range(0, 10);
        if (playerValue.IQ > 500 && probability > 6)
        {
            playerValue.Charm += 2;
            viewBase.ShowTalk("恭喜您获取了XX证书。您的魅力值增加了");
        }
    }
    public void GetMoney(float money)
    {
        playerValue.IncreaseFavorability("Money", money);
    }
    public void SpendMoney(float money)
    {
        playerValue.ReduceFavorability("Money", money);
    }
    /// <summary>
    /// 获得生活费
    /// </summary>
    public void GetlivingCosts()
    {
        int randomMoney = Random.Range(0, 300);
        randomMoney = randomMoney - randomMoney % 10;
        if (Random.Range(0, 2) == 0)
        {
            GetMoney(2000 + randomMoney);
            viewBase.ShowTalk($"您终于撑到了发生活费的时刻。妈妈这个月得到了奖金，多给你了{randomMoney}，恭喜您获得了{2000 + randomMoney}元");
        }
        else
        {
            GetMoney(2000 - randomMoney);
            viewBase.ShowTalk($"您终于撑到了发生活费的时刻。爸爸炒股亏了，你这月零花钱少了{randomMoney}，您获得了{2000 - randomMoney}元");
        }


    }
    public void ParttimeJob()//做兼职
    {
        playerValue.Energy -= 2;
        if (IsEnergyEmpty())
        {
            viewBase.ShowTalk("您已经没有精力做这件事了");
            return;
        }
        GetMoney(40);
        viewBase.ShowTalk("您辛苦的完成了2小时的兼职，获得了40元");
    }
    public void SellGoods(Goods good)
    {
        GoodList goodList = GoodList.GoodListInstance();
        viewBase.ShowTalk(string.Format("您成功卖出了物品{0},获得了{1}", good.name, good.price / 6));
        playerValue.Money += good.price / 6;
        Debug.LogWarning("此处尚未实现 物品减少");
    }//卖已有商品
    public void BuyGoods(Goods good)
    {
        GoodList goodList = GoodList.GoodListInstance();
        viewBase.ShowTalk($"您成功购买了物品{good.name},金币减少{good.price}");
        playerValue.Money -= good.price;
        Debug.LogWarning("此处尚未实现 背包增加物品");
    }
    public void Eat()//吃饭
    {
        if (playerValue.Money < 10)
        {
            viewBase.ShowTalk("bia~bia~bia~bia，您的钱不够了，只能吃空气了。");
            return;
        }
        playerValue.Energy -= 1;
        playerValue.Money -= 10;
        viewBase.ShowTalk("bia~bia~bia~bia，真好吃，您的精力提升了。");
    }
    public void Sleep()//睡觉
    {
        playerValue.Energy = 0;
        //viewBase.ShowTalk("hu~hu~hu，渐渐的，进入了梦乡。");
        viewBase.ShowBlackMessage("hu~hu~hu，渐渐的，进入了梦乡。");
        envValue.Day += 1;
        MainLine.Instance.NewDay();
    }
    public void Entertainment()//娱乐
    {
        playerValue.Energy += 1;
        if (IsEnergyEmpty())
        {
            viewBase.ShowTalk("您已经没有精力做这件事了");
            return;
        }
        playerValue.IQ += 1;
        viewBase.ShowTalk("您玩的很开心。");
    }

    public bool Exam()//考试
    {
        float score = Random.Range(0.01f, 1f);
        //TODO:考试动画
        if ((playerValue.IQ > 195 && score > 0.1f)||
            (playerValue.IQ > 130 && score > 0.13f)||
            (playerValue.IQ >= 65 && score > 0.2f)||
            (playerValue.IQ < 65 && score > 0.8f)
            )
        {
            viewBase.ShowTalk("采用了先进的AI改卷子，恭喜您通过了本学期的考试！");
            return true;
        }

        //else if (playerValue.IQ > 130 && score > 0.13f)
        //    return true;
        //else if (playerValue.IQ >= 65 && score > 0.2f)
        //    return true;
        //else if (playerValue.IQ < 65 && score > 0.8f)
        //    return true;
        viewBase.ShowTalk("采用了先进的AI改卷子，糟糕，您这学期挂科了！");
        return false;
    }
    public void DatingSb(string whom)
    {
        MainLine.Instance.IsDated = true;
        playerValue.EQ += 1;
        viewBase.ShowTalk(string.Format($"您的情商提升了并且和{whom}的亲密度提升了。"));
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
                     ((w.FriendValue >= 10 || w.FriendValue <= 30) && Random.Range(0, 1f) > 0.7f)||
                     (w.FriendValue >= 30 && Random.Range(0, 1f) > 0.05f)
                    )
                    AcceptDating(w);
                break;
            }
            RefuseDating(w);
        }

    }
    private void RefuseDating(Person person)
    {
        viewBase.ShowTalk($"对不起，{person}拒绝了你的邀请，原因不详...");
    }
    public void AcceptDating(Person person)
    {
        ++person.FriendValue;
        viewBase.ShowTalk($"{person}接受了你的邀请，");
    }
    bool IsEnergyEmpty()
    {
        if (playerValue.Energy >= 10)
            return true;
        return false;
    }
}

