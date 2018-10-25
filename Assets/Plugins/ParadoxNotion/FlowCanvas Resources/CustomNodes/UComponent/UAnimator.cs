using ParadoxNotion.Design;
using UnityEngine;

namespace FlowCanvas.Nodes
{

    #region Animator

    [Name("getCurrentAnimatorStateInfo")]
    [Category("UnityEngine/Animator")]
    [Description("获取Animator当前动画片段")]
    public class G_CurrentAnimatorStateInfo : PureFunctionNode<AnimatorStateInfo, Animator, int>
    {
        public override AnimatorStateInfo Invoke(Animator animator, int layerIndex = 0)
        {
            return animator.GetCurrentAnimatorStateInfo(layerIndex);
        }
    }

    [Name("StringToHash")]
    [Category("UnityEngine/Animator")]
    [Description("Animator的动画名称转换成int数值,为更高效率的匹配寻找动画")]
    public class G_StringToHash : PureFunctionNode<int, string>
    {
        public override int Invoke(string name)
        {
            return Animator.StringToHash(name);
        }
    }

    [Name("isAnimatorPlaying")]
    [Category("UnityEngine/Animator")]
    [Description("Animator是否在播放所输入名称的动画,Base Layer 中间有空格")]
    public class G_IsAnimatorPlaying : PureFunctionNode<bool, Animator, string, string>
    {
        public override bool Invoke(Animator animator, string animStateName, string layerName)
        {
            //animator.
            int layerIndex = animator.GetLayerIndex(layerName);
            return (animator.GetCurrentAnimatorStateInfo(layerIndex).fullPathHash ==
                    Animator.StringToHash(layerName + "." + animStateName) && !animator.IsInTransition(layerIndex))
                ? true
                : false;
        }
    }


    [Name("isInTransition")]
    [Category("UnityEngine/Animator")]
    [Description("Animator是否在切换动画片段的过渡状态中")]
    public class G_IsTransition : PureFunctionNode<bool, Animator, int>
    {
        public override bool Invoke(Animator animator, int layerIndex)
        {
            return animator.IsInTransition(layerIndex) ? true : false;
        }
    }

    [Name("hasState")]
    [Category("UnityEngine/Animator")]
    [Description("Animator组件中是否存在该名称的动画")]
    public class G_HasState : PureFunctionNode<bool, Animator, string, int>
    {
        public override bool Invoke(Animator animator, string AnimStateName, int layerIndex = 0)
        {
            return animator.HasState(layerIndex, Animator.StringToHash(AnimStateName)) ? true : false;
        }
    }

    [Name("getBool")]
    [Category("UnityEngine/Animator")]
    [Description("获取Animator组件中的Bool值")]
    public class G_AnimatorBool : PureFunctionNode<bool, Animator, string>
    {
        public override bool Invoke(Animator animator, string boolParaName)
        {
            return animator.GetBool(Animator.StringToHash(boolParaName));
        }
    }

    [Name("setBool")]
    [Category("UnityEngine/Animator")]
    [Description("设置Animator组件中的Bool值")]
    public class S_AnimatorBool : CallableFunctionNode<Animator, Animator, string, bool>
    {
        public override Animator Invoke(Animator animator, string boolParaName, bool value)
        {
            animator.SetBool(Animator.StringToHash(boolParaName), value);
            return animator;
        }
    }

    [Name("crossFade(Animator)")]
    [Category("UnityEngine/Animator")]
    [Description(
         "过渡播放,transitionDuration是原先动画过渡到新的动画花费的时间百分比;同理normalizedTime是目标动画开始时间的百分比,如果小于零则无视开始时间,isfixTime是以fixtime计算过渡时间;stateName已做优化处理"
     )]
    public class S_CrossFade : CallableFunctionNode<Animator, Animator, string, float, int, float, bool>
    {
        public override Animator Invoke(Animator animator, string stateName, float transitionDuration, int layer,
            float normalizedTime, bool isFixTimeMode = false)
        {
            float targetStateStartTime = 0;
            if (normalizedTime < 0)
            {
                targetStateStartTime = float.NegativeInfinity;
            }
            else
            {
                targetStateStartTime = normalizedTime;
            }

            if (!isFixTimeMode)
                animator.CrossFade(Animator.StringToHash(stateName), transitionDuration, layer, targetStateStartTime);
            else
            {
                animator.CrossFadeInFixedTime(Animator.StringToHash(stateName), transitionDuration, layer,
                    targetStateStartTime);
            }
            return animator;
        }
    }

    [Name("getFloat")]
    [Category("UnityEngine/Animator")]
    [Description("获取Animator组件中的Float值")]
    public class G_AnimatorFloat : PureFunctionNode<float, Animator, string>
    {
        public override float Invoke(Animator animator, string floatParaName)
        {
            return animator.GetFloat(Animator.StringToHash(floatParaName));
        }
    }

    [Name("getLayerIndex")]
    [Category("UnityEngine/Animator")]
    [Description("获取Animator组件中Layer名称对应的int值")]
    public class G_AnimatorGetLayerIndex : PureFunctionNode<int, Animator, string>
    {
        public override int Invoke(Animator animator, string layerName)
        {
            return animator.GetLayerIndex(layerName);
        }
    }

    [Name("getLayerName")]
    [Category("UnityEngine/Animator")]
    [Description("获取Animator组件中Layer指数对应的层名")]
    public class G_AnimatorGetLayerName : PureFunctionNode<string, Animator, int>
    {
        public override string Invoke(Animator animator, int layerIndex)
        {
            return animator.GetLayerName(layerIndex);
        }
    }

    [Name("getLayerWeight")]
    [Category("UnityEngine/Animator")]
    [Description("获取Animator组件中Layer权重值")]
    public class G_AnimatorGetLayerWeight : PureFunctionNode<float, Animator, int>
    {
        public override float Invoke(Animator animator, int layerIndex)
        {
            return animator.GetLayerWeight(layerIndex);
        }
    }

    [Name("setLayerWeight")]
    [Category("UnityEngine/Animator")]
    [Description("设置Animator组件中layer的权重值")]
    public class S_AnimatorLayerWeight : CallableFunctionNode<Animator, Animator, int, float>
    {
        public override Animator Invoke(Animator animator, int layerIndex, float value)
        {
            animator.SetLayerWeight(layerIndex, value);
            return animator;
        }
    }

    [Name("setFloat")]
    [Category("UnityEngine/Animator")]
    [Description("设置Animator组件中的Float值")]
    public class S_AnimatorFloat : CallableFunctionNode<Animator, Animator, string, float>
    {
        public override Animator Invoke(Animator animator, string floatParaName, float value)
        {
            animator.SetFloat(Animator.StringToHash(floatParaName), value);
            return animator;
        }
    }

    [Name("getInteger")]
    [Category("UnityEngine/Animator")]
    [Description("获取Animator组件中的Integer值")]
    public class G_AnimatorInteger : PureFunctionNode<int, Animator, string>
    {
        public override int Invoke(Animator animator, string intParaName)
        {
            return animator.GetInteger(Animator.StringToHash(intParaName));
        }
    }

    [Name("setInteger")]
    [Category("UnityEngine/Animator")]
    [Description("设置Animator组件中的Integer值")]
    public class S_AnimatorInteger : CallableFunctionNode<Animator, Animator, string, int>
    {
        public override Animator Invoke(Animator animator, string intParaName, int value)
        {
            animator.SetInteger(Animator.StringToHash(intParaName), value);
            return animator;
        }
    }

    [Name("SetTrigger")]
    [Category("UnityEngine/Animator")]
    [Description("触发Animator组件中的Trigger")]
    public class S_AnimatorTrigger : CallableFunctionNode<Animator,Animator, string>
    {
        public override Animator Invoke(Animator animator, string triggerParaName)
        {
            animator.SetTrigger(Animator.StringToHash(triggerParaName));
            return animator;
        }
    }

    [Name("ResetTrigger")]
    [Category("UnityEngine/Animator")]
    [Description("重新触发Animator组件中的Trigger")]
    public class S_AnimatorResetTrigger : CallableFunctionNode<Animator,Animator, string>
    {
        public override Animator Invoke(Animator animator, string triggerParaName)
        {
            animator.ResetTrigger(Animator.StringToHash(triggerParaName));
            return animator;
        }
    }

    [Name("play(Animator)")]
    [Category("UnityEngine/Animator")]
    [Description("播放Animator组件中的特定动画,可控制动画的初始播放进程")]
    public class S_AnimatorPlay : CallableFunctionNode<Animator, Animator, string, int, float, bool>
    {
        public override Animator Invoke(Animator animator, string AnimStateName, int layerIndex = -1,
            float normalizedTime = float.NegativeInfinity, bool isFixedTime = false)
        {
            if (!isFixedTime)
            {
                animator.Play(Animator.StringToHash(AnimStateName), layerIndex, normalizedTime);
            }
            else
            {
                animator.PlayInFixedTime(Animator.StringToHash(AnimStateName), layerIndex, normalizedTime);
            }
            return animator;
        }
    }
    

    #endregion
}