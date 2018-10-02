using ParadoxNotion.Design;
using UnityEngine;

namespace FlowCanvas.Nodes
{

    #region Mathf

    [Name("getInfinity")]
    [Category("UnityEngine/Mathf")]
    [Description("获取float正无穷大")]
    public class G_Infinity : PureFunctionNode<float>
    {
        public override float Invoke()
        {
            return Mathf.Infinity;
        }
    }

    [Name("inRange")]
    [Category("UnityEngine/Mathf")]
    [Description("获取float是否在两个值的范围内")]
    public class G_InRange : PureFunctionNode<bool, float, float, float>
    {
        public override bool Invoke(float value, float min, float max)
        {
            if (value < min || value > max)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    [Name("getNegativeInfinity")]
    [Category("UnityEngine/Mathf")]
    [Description("获取float负无穷大")]
    public class G_NegativeInfinity : PureFunctionNode<float>
    {
        public override float Invoke()
        {
            return Mathf.NegativeInfinity;
        }
    }

    [Name("rad2Deg")]
    [Category("UnityEngine/Mathf")]
    [Description("将弧度转换成角度")]
    public class G_Rad2Deg : PureFunctionNode<float, float>
    {
        public override float Invoke(float radians)
        {
            return Mathf.Rad2Deg*radians;
        }
    }

    [Name("deg2Rad")]
    [Category("UnityEngine/Mathf")]
    [Description("将角度转换成弧度")]
    public class G_Deg2Rad : PureFunctionNode<float, float>
    {
        public override float Invoke(float degress)
        {
            return Mathf.Deg2Rad*degress;
        }
    }

    [Name("Abs(float)")]
    [Category("UnityEngine/Mathf")]
    [Description("float绝对值")]
    public class G_floatAbs : PureFunctionNode<float, float>
    {
        public override float Invoke(float value)
        {
            return Mathf.Abs(value);
        }
    }

    [Name("Abs(int)")]
    [Category("UnityEngine/Mathf")]
    [Description("int绝对值")]
    public class G_intAbs : PureFunctionNode<int, int>
    {
        public override int Invoke(int value)
        {
            return Mathf.Abs(value);
        }
    }

    [Name("ceilToInt(int)")]
    [Category("UnityEngine/Mathf")]
    [Description("返回天花板值,如10.1=>11,-10.1=>-10")]
    public class G_CeilToInt : PureFunctionNode<int, float>
    {
        public override int Invoke(float value)
        {
            return Mathf.CeilToInt(value);
        }
    }

    [Name("clamp(float)")]
    [Category("UnityEngine/Mathf")]
    [Description("float限定在范围内")]
    public class G_Clampfloat : PureFunctionNode<float, float, float, float>
    {
        public override float Invoke(float value, float min, float max)
        {   
            return Mathf.Clamp(value, min, max);
        }
    }

    [Name("clamp(int)")]
    [Category("UnityEngine/Mathf")]
    [Description("int限定在范围内")]
    public class G_Clampint : PureFunctionNode<int, int, int, int>
    {
        public override int Invoke(int value, int min, int max)
        {
            return Mathf.Clamp(value, min, max);
        }
    }

    [Name("clamp01(float)")]
    [Category("UnityEngine/Mathf")]
    [Description("float限定在0-1内")]
    public class G_Clampfloat01 : PureFunctionNode<float, float>
    {
        public override float Invoke(float value)
        {
            return Mathf.Clamp01(value);
        }
    }

    [Name("deltaAngle(float)")]
    [Category("UnityEngine/Mathf")]
    [Description("返回两个角度间最小夹角值,如450,75°的夹角")]
    public class G_DeltaAngle : PureFunctionNode<float, float, float>
    {
        public override float Invoke(float currentAngle, float targetAngle)
        {
            return Mathf.DeltaAngle(currentAngle, targetAngle);
        }
    }

    [Name("floorToInt(int)")]
    [Category("UnityEngine/Mathf")]
    [Description("返回地板值,10.1=>10,-10.1=>-11")]
    public class G_FloorToInt : PureFunctionNode<int, float>
    {
        public override int Invoke(float value)
        {
            return Mathf.FloorToInt(value);
        }
    }

    [Name("roundToInt(int)")]
    [Category("UnityEngine/Mathf")]
    [Description("四舍五入")]
    public class G_RoundToInt : PureFunctionNode<int, float>
    {
        public override int Invoke(float value)
        {
            return Mathf.RoundToInt(value);
        }
    }

    [Name("inverseLerp(float)")]
    [Category("UnityEngine/Mathf")]
    [Description("占据比例,如 from:5,to:10,value:8,则占据3/5")]
    public class G_InverseLerp : PureFunctionNode<float, float, float, float>
    {
        public override float Invoke(float from, float to, float value)
        {
            return Mathf.InverseLerp(from, to, value);
        }
    }

    [Name("lerp(float)")]
    [Category("UnityEngine/Mathf")]
    [Description("获取差值,percent是两float值间的比例")]
    public class G_Lerp : PureFunctionNode<float, float, float, float>
    {
        public override float Invoke(float from, float to, float percent)
        {
            return Mathf.Lerp(from, to, percent);
        }
    }

    [Name("moveTowards(float)")]
    [Category("UnityEngine/Mathf")]
    [Description("获取差值,增量")]
    public class G_MoveTowards : PureFunctionNode<float, float, float, float>
    {
        public override float Invoke(float from, float to, float delta)
        {
            return Mathf.MoveTowards(from, to, delta);
        }
    }

    [Name("moveTowardsAngle(float)")]
    [Category("UnityEngine/Mathf")]
    [Description("获取差值,deltaAngle是两角度值间增量,")]
    public class G_MoveTowardsAngle : PureFunctionNode<float, float, float, float>
    {
        public override float Invoke(float from, float to, float deltaAngle)
        {
            return Mathf.MoveTowardsAngle(from, to, deltaAngle);
        }
    }

    [Name("pingPong(float)")]
    [Category("UnityEngine/Mathf")]
    [Description("乒乓值,在0和length值间来回变化,适合输入Time.time")]
    public class G_PingPong : PureFunctionNode<float, float, float>
    {
        public override float Invoke(float value, float length)
        {
            return Mathf.PingPong(value, length);
        }
    }

    [Name("repeat(float)")]
    [Category("UnityEngine/Mathf")]
    [Description("重复值,在0和length值间重新变化,适合输入Time.time")]
    public class G_Repeat : PureFunctionNode<float, float, float>
    {
        public override float Invoke(float value, float length)
        {
            return Mathf.Repeat(value, length);
        }
    }

    [Name("sign(float)")]
    [Category("UnityEngine/Mathf")]
    [Description("大于等于0,=1,小于0=-1")]
    public class G_Sign : PureFunctionNode<float, float>
    {
        public override float Invoke(float value)
        {
            return Mathf.Sign(value);
        }
    }

    [Name("pow(float)")]
    [Category("UnityEngine/Mathf")]
    [Description("value 的 power次方")]
    public class G_Pow : PureFunctionNode<float, float, float>
    {
        public override float Invoke(float value, float pow)
        {
            return Mathf.Pow(value, pow);
        }
    }

    [Name("PI(float)")]
    [Category("UnityEngine/Mathf")]
    [Description("π")]
    public class G_PI : PureFunctionNode<float>
    {
        public override float Invoke()
        {
            return Mathf.PI;
        }
    }

    [Name("lerpAngle(float)")]
    [Category("UnityEngine/Mathf")]
    [Description("获取差值,percent是两角度值间的比例,")]
    public class G_LerpAngle : PureFunctionNode<float, float, float, float>
    {
        public override float Invoke(float from, float to, float percent)
        {
            return Mathf.LerpAngle(from, to, percent);
        }
    }

    [Name("max(float)")]
    [Category("UnityEngine/Mathf")]
    [Description("获取两者间最大值")]
    public class G_Max : PureFunctionNode<float, float, float>
    {
        public override float Invoke(float value1, float value2)
        {
            return Mathf.Max(value1, value2);
        }
    }

    [Name("mix(float)")]
    [Category("UnityEngine/Mathf")]
    [Description("获取两者间最小值")]
    public class G_Mix : PureFunctionNode<float, float, float>
    {
        public override float Invoke(float value1, float value2)
        {
            return Mathf.Min(value1, value2);
        }
    }

    [Name("max(int)")]
    [Category("UnityEngine/Mathf")]
    [Description("获取两者间最大值")]
    public class G_MaxInt : PureFunctionNode<int, int, int>
    {
        public override int Invoke(int value1, int value2)
        {
            return Mathf.Max(value1, value2);
        }
    }

    [Name("mix(int)")]
    [Category("UnityEngine/Mathf")]
    [Description("获取两者间最小值")]
    public class G_MixInt : PureFunctionNode<int, int, int>
    {
        public override int Invoke(int value1, int value2)
        {
            return Mathf.Min(value1, value2);
        }
    }

    #endregion
}