
public class PlayerValue : IChangeValue
{
    private static PlayerValue player = new PlayerValue();
    ValueUI valueUI = ValueUI.PlayerValueUIInstance();
    //UnityEngine.UI.Slider sldEnergy;
    /// <summary>
    /// 返回Player实例
    /// </summary>
    /// <returns></returns>
    public static PlayerValue PlayerInstance()
    {
        return player;
    }
    private PlayerValue()
    {
        //sldEnergy = GameObject.Find("SldEnergy").GetComponent<UnityEngine.UI.Slider>();
    }
    int iq;
    /// <summary>
    /// 智商,max:500
    /// </summary>
    public int IQ
    {
        get => iq;
        set
        {
            valueUI.txtIQValue.text = value.ToString();
            iq = value;
        }

    }
    
    int charm;
    /// <summary>
    /// 魅力值,max
    /// </summary>

    public int Charm
    {
        get => charm;
        set
        {
            charm = value;
            valueUI.txtCharmValue.text = value.ToString();
        }
    }
    private int eq;
    /// <summary>
    /// 情商
    /// </summary>
    public int EQ
    {
        get => eq;
        set
        {
            valueUI.txtEQValue.text = value.ToString();
            eq = value;
        }
    }
    /// <summary>
    /// 虚荣
    /// </summary>
    public int Vanity
    {
        get; set;
    }

    private float energy;
    /// <summary>
    /// 精力值,max:10
    /// </summary>
    public float Energy
    {
        get { return energy; }
        set
        {
            valueUI.sldEnergy.value = value;
            energy = value;
        }
    }
    /// <summary>
    /// 金钱
    /// </summary>

    private float money;
    public float Money
    {
        get => money;
        set { valueUI.txtMoneyValue.text = value.ToString(); money = value; }
    }
    public void IncreaseFavorability(string name, float score)
    {
        System.Reflection.PropertyInfo propertyInfo = GetType().GetProperty(name);
        propertyInfo.SetValue(this, (float)propertyInfo.GetValue(this) + score);
    }
    public void ReduceFavorability(string name, float score)
    {
        System.Reflection.PropertyInfo propertyInfo = GetType().GetProperty(name);
        propertyInfo.SetValue(this, score - (float)propertyInfo.GetValue(this));
    }
}

