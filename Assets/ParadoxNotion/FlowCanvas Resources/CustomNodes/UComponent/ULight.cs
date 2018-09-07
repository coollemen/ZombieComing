using ParadoxNotion.Design;
using UnityEngine;


namespace FlowCanvas.Nodes
{

    #region Light

    [Name("getIntensity")]
    [Category("UnityEngine/Light")]
    [Description("获取灯光强度")]
    public class G_LightIntensity : PureFunctionNode<float, Light>
    {
        public override float Invoke(Light light)
        {
            return light.intensity;
        }
    }

    [Name("setIntensity")]
    [Category("UnityEngine/Light")]
    [Description("设置灯光强度")]
    public class S_LightIntensity : CallableFunctionNode<Light,Light, float>
    {
        public override Light Invoke(Light light, float value)
        {
            light.intensity = value;
            return light;
        }
    }

    [Name("getRange")]
    [Category("UnityEngine/Light")]
    [Description("获取灯光范围")]
    public class G_LightRange : PureFunctionNode<float, Light>
    {
        public override float Invoke(Light light)
        {
            return light.range;
        }
    }

    [Name("setRange")]
    [Category("UnityEngine/Light")]
    [Description("设置灯光范围")]
    public class S_LightRange : CallableFunctionNode<Light,Light, float>
    {
        public override Light Invoke(Light light, float value)
        {
            light.range = value;
            return light;
        }
    }

    [Name("getColor")]
    [Category("UnityEngine/Light")]
    [Description("获取灯光强度")]
    public class G_LightColor : PureFunctionNode<Color, Light>
    {
        public override Color Invoke(Light light)
        {
            return light.color;
        }
    }

    [Name("setColor")]
    [Category("UnityEngine/Light")]
    [Description("设置灯光强度")]
    public class S_LightColor : CallableFunctionNode<Light,Light, Color>
    {
        public override Light Invoke(Light light, Color color)
        {
            light.color = color;
            return light;
        }
    }

    #endregion
}