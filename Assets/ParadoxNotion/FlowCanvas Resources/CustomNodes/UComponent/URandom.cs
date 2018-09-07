using ParadoxNotion.Design;
using UnityEngine;

namespace FlowCanvas.Nodes
{

    #region  Random

    [Name("floatRandom")]
    [Category("UnityEngine/Random")]
    [Description("返回一个0-1的随机float值")]
    public class G_RandomFloat : PureFunctionNode<float>
    {
        public override float Invoke()
        {
            return UnityEngine.Random.value;
        }
    }

    [Name("vector2Random")]
    [Category("UnityEngine/Random")]
    [Description("返回一个半径为1平面圆内的随机vector2位置值")]
    public class G_RandomVector2 : PureFunctionNode<Vector2>
    {
        public override Vector2 Invoke()
        {
            return UnityEngine.Random.insideUnitCircle;
        }
    }

    [Name("vector3Random")]
    [Category("UnityEngine/Random")]
    [Description("返回一个半径为1球形内的随机vector3位置值")]
    public class G_RandomVector3 : PureFunctionNode<Vector3>
    {
        public override Vector3 Invoke()
        {
            return UnityEngine.Random.insideUnitSphere;
        }
    }

    [Name("vector3NomalLizedRandom")]
    [Category("UnityEngine/Random")]
    [Description("返回一个半径为1球形表面(即长度为1)的随机vector3位置值")]
    public class G_RandomVector3NomalLized : PureFunctionNode<Vector3>
    {
        public override Vector3 Invoke()
        {
            return UnityEngine.Random.onUnitSphere;
        }
    }

    [Name("quaternionRandom")]
    [Category("UnityEngine/Random")]
    [Description("返回一个随机quternion角度值")]
    public class G_RandomQuaternion : PureFunctionNode<Quaternion>
    {
        public override Quaternion Invoke()
        {
            return UnityEngine.Random.rotation;
        }
    }

    [Name("initState")]
    [Category("UnityEngine/Random")]
    [Description("初始化设置随机种子")]
    public class G_RandomInitState : CallableActionNode<int>
    {
        public override void Invoke(int seed)
        {
            UnityEngine.Random.InitState(seed);
        }
    }

    [Name("floatRandomRange")]
    [Category("UnityEngine/Random")]
    [Description("在输入范围内随机float数值,数值包含极限值,需要配合InitState随机化种子")]
    public class G_RandomFloatRange : PureFunctionNode<float, float, float>
    {
        public override float Invoke(float min, float max)
        {
            return UnityEngine.Random.Range(min, max);
        }
    }

    [Name("intRandomRange")]
    [Category("UnityEngine/Random")]
    [Description("在输入范围内随机int数值,排除最大值,需要配合InitState随机化种子")]
    public class G_RandomIntRange : PureFunctionNode<int, int, int>
    {
        public override int Invoke(int min, int max)
        {
            return UnityEngine.Random.Range(min, max);
        }
    }

    #endregion
}