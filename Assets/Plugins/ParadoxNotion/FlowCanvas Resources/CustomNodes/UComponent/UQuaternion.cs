using ParadoxNotion.Design;
using UnityEngine;


namespace FlowCanvas.Nodes
{

    #region Quaternion

    [Name("identity")]
    [Category("UnityEngine/Quaternion")]
    [Description("返回Quaternion默认初始值")]
    public class G_QuaterIdentity : PureFunctionNode<Quaternion>
    {
        public override Quaternion Invoke()
        {
            return Quaternion.identity;
        }
    }

    [Name("angleAxis")]
    [Category("UnityEngine/Quaternion")]
    [Description("沿指定轴向(不考虑向量长度),在旋转一定角度对应的Quaternion数值")]
    public class G_QuaterAngleAxis : PureFunctionNode<Quaternion, Vector3, float>
    {
        public override Quaternion Invoke(Vector3 axis, float angle)
        {
            return Quaternion.AngleAxis(angle, axis);
        }
    }

    [Name("euler")]
    [Category("UnityEngine/Quaternion")]
    [Description("根据角度换算对应Quaternion")]
    public class G_QuaterEuler : PureFunctionNode<Quaternion, Vector3>
    {
        public override Quaternion Invoke(Vector3 eulerAngle)
        {
            return Quaternion.Euler(eulerAngle);
        }
    }

    [Name("get_EulerAngle")]
    [Category("UnityEngine/Quaternion")]
    [Description("获得Quaternion对应的换算角度")]
    public class G_EulerAngle : PureFunctionNode<Vector3, Quaternion>
    {
        public override Vector3 Invoke(Quaternion quaternion)
        {
            return quaternion.eulerAngles;
        }
    }

    [Name("set_EulerAngle")]
    [Category("UnityEngine/Quaternion")]
    [Description("设置Quaternion对应的换算角度")]
    public class S_EulerAngle : PureFunctionNode<Quaternion, Quaternion, Vector3>
    {
        public override Quaternion Invoke(Quaternion quaternion, Vector3 targetEulerAngle)
        {
            quaternion.eulerAngles = targetEulerAngle;
            return quaternion;
        }
    }

    [Name("fromToRotation")]
    [Category("UnityEngine/Quaternion")]
    [Description("从一个向量转向另一个向量转过的Quaternion数值")]
    public class G_QuaterFromToRotation : PureFunctionNode<Quaternion, Vector3, Vector3>
    {
        public override Quaternion Invoke(Vector3 fromDirection, Vector3 toDirection)
        {
            return Quaternion.FromToRotation(fromDirection, toDirection);
        }
    }

    [Name("inverse")]
    [Category("UnityEngine/Quaternion")]
    [Description("反转Quaternion数值")]
    public class G_QuaterInverse : PureFunctionNode<Quaternion, Quaternion>
    {
        public override Quaternion Invoke(Quaternion target)
        {
            return Quaternion.Inverse(target);
        }
    }

    [Name("lerp")]
    [Category("UnityEngine/Quaternion")]
    [Description("在两个Quaternion数值间线性过渡,由一个0-1的float值控制过渡比值")]
    public class G_QuaterLerp : PureFunctionNode<Quaternion, Quaternion, Quaternion, float>
    {
        public override Quaternion Invoke(Quaternion from, Quaternion to, float percent)
        {
            return Quaternion.Lerp(from, to, percent);
        }
    }

    [Name("slerp")]
    [Category("UnityEngine/Quaternion")]
    [Description("在两个Quaternion数值间弧形过渡,由一个0-1的float值控制过渡比值")]
    public class G_QuaterSLerp : PureFunctionNode<Quaternion, Quaternion, Quaternion, float>
    {
        public override Quaternion Invoke(Quaternion from, Quaternion to, float percent)
        {
            return Quaternion.Slerp(from, to, percent);
        }
    }

    [Name("slerpUnclamped")]
    [Category("UnityEngine/Quaternion")]
    [Description("在两个Quaternion数值间弧形过渡,由一个0-1的float值控制过渡比值,并且超出0-1范围Quaternion过渡值不会限定在两者间")]
    public class G_QuaterSLerpUnclamped : PureFunctionNode<Quaternion, Quaternion, Quaternion, float>
    {
        public override Quaternion Invoke(Quaternion from, Quaternion to, float percent)
        {
            return Quaternion.SlerpUnclamped(from, to, percent);
        }
    }

    [Name("lerpUnclamped")]
    [Category("UnityEngine/Quaternion")]
    [Description("在两个Quaternion数值间线性过渡,由一个0-1的float值控制过渡比值,并且超出0-1范围Quaternion过渡值不会限定在两者间")]
    public class G_QuaterLerpUnclamped : PureFunctionNode<Quaternion, Quaternion, Quaternion, float>
    {
        public override Quaternion Invoke(Quaternion from, Quaternion to, float percent)
        {
            return Quaternion.LerpUnclamped(from, to, percent);
        }
    }

    [Name("lookRotation")]
    [Category("UnityEngine/Quaternion")]
    [Description("获得注视目标的quaternion角度,输入注视的方向.可以定义向上方向(即已哪个轴为基准调整朝向)")]
    public class G_QuaterLookRotation : PureFunctionNode<Quaternion, Vector3, Vector3>
    {
        public override Quaternion Invoke(Vector3 lookDirection, Vector3 UpVector)
        {
            return Quaternion.LookRotation(lookDirection, UpVector);
        }
    }

    [Name("rotateTowards")]
    [Category("UnityEngine/Quaternion")]
    [Description("两个quaternion间过渡,过渡方式不是以0-1的百分比来控制进度,而是每帧角度的增值")]
    public class G_QuaterRotateTowards : PureFunctionNode<Quaternion, Quaternion, Quaternion, float>
    {
        public override Quaternion Invoke(Quaternion from, Quaternion to, float deltaAngle)
        {
            return Quaternion.RotateTowards(from, to, deltaAngle);
        }
    }

    [Name("angle(Quaternion)")]
    [Category("UnityEngine/Quaternion")]
    [Description("返回两个quaternion的夹角")]
    public class G_QuaterAngle : PureFunctionNode<float, Quaternion, Quaternion>
    {
        public override float Invoke(Quaternion from, Quaternion to)
        {
            return Quaternion.Angle(from, to);
        }
    }

    [Name("dot(Quaternion)")]
    [Category("UnityEngine/Quaternion")]
    [Description("返回两个quaternion角度的相似度,0表示相垂直,1表示相平行")]
    public class G_QuaterDot : PureFunctionNode<float, Quaternion, Quaternion>
    {
        public override float Invoke(Quaternion from, Quaternion to)
        {
            return Quaternion.Dot(from, to);
        }
    }

    #endregion
}