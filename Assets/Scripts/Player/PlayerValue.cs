
public class PlayerValue : IChangeValue
{
    private static PlayerValue player = new PlayerValue();
    ValueUI valueUI;
    public float Buff { get; set; } = 0.5f;
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
        valueUI = ValueUI.PlayerValueUIInstance();
        Buff = 1f;
        //sldEnergy = GameObject.Find("SldEnergy").GetComponent<UnityEngine.UI.Slider>();
    }
    float iq;
    /// <summary>
    /// 智商,max:500
    /// </summary>
    public float IQ
    {
        get => iq;
        set
        {
            iq = value * Buff;
            valueUI.txtIQValue.text = iq .ToString();
        }

    }
    
    float charm;
    /// <summary>
    /// 魅力值,max
    /// </summary>

    public float Charm
    {
        get => charm;
        set
        {
            charm = value * Buff;
            valueUI.txtCharmValue.text = charm.ToString();
        }
    }
    private float eq;
    /// <summary>
    /// 情商
    /// </summary>
    public float EQ
    {
        get => eq;
        set
        {
            eq = value * Buff;
            valueUI.txtEQValue.text = eq.ToString();
            
        }
    }
    /// <summary>
    /// 虚荣
    /// </summary>
    public float Vanity
    {
        get; set;
    }

    public bool isBreakfast = false, isLunch = false, isDinner = false;
    private float energy;
    /// <summary>
    /// 精力值,max:10
    /// </summary>
    public float Energy
    {
        get { return energy; }
        set
        {
            energy = value;
            valueUI.sldEnergy.value = value;
            //if(value == 0)
            //{
            //    Buff = 0.5f;
            //    isBreakfast = false;
            //    isLunch = false;
            //    isDinner = false;
            //}
            //else if (value < 8 && value > 4 && !isLunch)
            //{
            //    Buff = 0.5f;
            //    //isLunch = false;
            //}
            //else if (value <= 8 && value >= 6 && !isDinner)
            //{
            //    Buff = 0.5f;
            //    //isDinner = false;
            //}


        }
    }
    /// <summary>
    /// 金钱
    /// </summary>

    private float money;
    public float Money
    {
        get => money;
        set { money = value; valueUI.txtMoneyValue.text = value.ToString();  }
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

