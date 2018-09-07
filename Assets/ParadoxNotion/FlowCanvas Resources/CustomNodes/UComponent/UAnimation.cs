using ParadoxNotion.Design;
using UnityEngine;

namespace FlowCanvas.Nodes
{

    #region  Animation 

    [Name("getClip")]
    [Category("UnityEngine/Animation")]
    [Description("返回Animation组件默认的动画片段")]
    public class G_AnimationClip : PureFunctionNode<AnimationClip, Animation>
    {
        public override AnimationClip Invoke(Animation animation)
        {
            return animation.clip;
        }
    }

    [Name("setClip")]
    [Category("UnityEngine/Animation")]
    [Description("赋值Animation组件默认的动画片段")]
    public class S_AnimationClip : CallableFunctionNode<Animation, Animation, AnimationClip>
    {
        public override Animation Invoke(Animation animation, AnimationClip clip)
        {
            animation.clip = clip;
            return animation;
        }
    }

    [Name("isPlaying")]
    [Category("UnityEngine/Animation")]
    [Description("Animation是否处于播放状态")]
    public class G_AnimationPlaying : PureFunctionNode<bool, Animation>
    {
        public override bool Invoke(Animation animation)
        {
            return animation.isPlaying;
        }
    }

    [Name("isPlayingClip")]
    [Category("UnityEngine/Animation")]
    [Description("Animation的动画片段是否处于播放状态")]
    public class G_AnimationPlayingClip : PureFunctionNode<bool, Animation, string>
    {
        public override bool Invoke(Animation animation, string targetAnimClip)
        {
            return animation.IsPlaying(targetAnimClip);
        }
    }

    [Name("getClipCount")]
    [Category("UnityEngine/Animation")]
    [Description("返回Animation组件的动画片段的总量")]
    public class G_AnimationClipCount : PureFunctionNode<int, Animation>
    {
        public override int Invoke(Animation animation)
        {
            return animation.GetClipCount();
        }
    }

    [Name("getAnimClipState")]
    [Category("UnityEngine/Animation")]
    [Description("根据输入名称,获取Animation组件中的动画")]
    public class G_AnimationClipState : PureFunctionNode<AnimationState, Animation, string>
    {
        public override AnimationState Invoke(Animation animation, string animStateName)
        {
            return animation[animStateName];
        }
    }

    [Name("getAnimationWrapMode")]
    [Category("UnityEngine/Animation")]
    [Description("获取Animation动画的播放模式")]
    public class G_AnimationWrapMode : PureFunctionNode<WrapMode, Animation>
    {
        public override WrapMode Invoke(Animation animation)
        {
            return animation.wrapMode;
        }
    }

    [Name("setAnimationWrapMode")]
    [Category("UnityEngine/Animation")]
    [Description("设置Animation动画的播放模式")]
    public class S_AnimationWrapMode : CallableFunctionNode<Animation,Animation, WrapMode>
    {
        public override Animation Invoke(Animation animation, WrapMode wrapMode)
        {
            animation.wrapMode = wrapMode;
            return animation;
        }
    }

    [Name("play")]
    [Category("UnityEngine/Animation")]
    [Description("立即播放动画,没有过渡,如果无法播放目标动画则返回false")]
    public class PlayAnimation : CallableFunctionNode<bool, Animation, string, PlayMode>
    {
        public override bool Invoke(Animation animation, string newAnimName, PlayMode playMode = PlayMode.StopSameLayer)
        {
            return animation.Play(newAnimName, playMode);
        }
    }

    [Name("rewind")]
    [Category("UnityEngine/Animation")]
    [Description("重新播放动画")]
    public class RewindAnimation : CallableFunctionNode<Animation,Animation, string>
    {
        public override Animation Invoke(Animation animation, string newAnimName)
        {
            animation.Rewind(newAnimName);
            return animation;
        }
    }

    [Name("stop")]
    [Category("UnityEngine/Animation")]
    [Description("停止播放动画,可以指定动画或全部动画")]
    public class StopAnimation : CallableFunctionNode<Animation,Animation, string, bool>
    {
        public override Animation Invoke(Animation animation, string newAnimName, bool stopAll = false)
        {
            if (!stopAll)
                animation.Stop(newAnimName);
            else animation.Stop();

            return animation;
        }
    }

    [Name("playQueued")]
    [Category("UnityEngine/Animation")]
    [Description("立即播放动画,没有过渡到新动画,排队方式等待先前的动画完成,或立即叠加播放一个新的动画")]
    public class playQueuedAnimation : CallableFunctionNode<bool, Animation, string, QueueMode, PlayMode>
    {
        public override bool Invoke(Animation animation, string newAnimName,
            QueueMode queueMode = QueueMode.CompleteOthers, PlayMode playMode = PlayMode.StopAll)
        {
            return animation.PlayQueued(newAnimName, queueMode, playMode);
        }
    }

    [Name("blend")]
    [Category("UnityEngine/Animation")]
    [Description("混合播放两个动画")]
    public class BlendAnimation : CallableFunctionNode<Animation,Animation, string, float, float>
    {
        public override Animation Invoke(Animation animation, string newAnimName, float targetWeight, float fadeTime)
        {
            animation.Blend(newAnimName, targetWeight, fadeTime);
            return animation;
        }
    }

    [Name("crossFade")]
    [Category("UnityEngine/Animation")]
    [Description("过渡到新动画,播放模式:停止该层的动画,或停止所有动画,然后播放新动画")]
    public class CrossFadeAnimation : CallableFunctionNode<Animation, Animation, string, float, PlayMode>
    {
        public override Animation Invoke(Animation animation, string newAnimName, float fadeTime,
            PlayMode playMode = PlayMode.StopSameLayer)
        {
            animation.CrossFade(newAnimName, fadeTime, playMode);
            return animation;
        }
    }

    [Name("crossFadeQueued")]
    [Category("UnityEngine/Animation")]
    [Description("排队过渡到新动画,排队方式等待先前的动画完成,或立即叠加播放一个新的动画")]
    public class CrossFadeQueuedAnimation : CallableFunctionNode<Animation,Animation, string, float, QueueMode, PlayMode>
    {
        public override Animation Invoke(Animation animation, string newAnimName, float fadeTime,
            QueueMode queueMode = QueueMode.CompleteOthers, PlayMode playMode = PlayMode.StopAll)
        {
            animation.CrossFadeQueued(newAnimName, fadeTime, queueMode, playMode);
            return animation;
        }
    }

    #endregion

    #region AnimationClip
    [Name("getAnimationClipWrapMode")]
    [Category("UnityEngine/AnimationClip")]
    [Description("获取AnimationClip动画的播放模式")]
    public class G_AnimationClipWrapMode : PureFunctionNode<WrapMode, AnimationClip>
    {
        public override WrapMode Invoke(AnimationClip animationClip)
        {
            return animationClip.wrapMode;
        }
    }

    [Name("setAnimationClipWrapMode")]
    [Category("UnityEngine/AnimationClip")]
    [Description("获取AnimationClip动画的播放模式")]
    public class S_AnimationClipWrapMode : CallableFunctionNode<AnimationClip,AnimationClip, WrapMode>
    {
        public override AnimationClip Invoke(AnimationClip animationClip, WrapMode wrapMpde)
        {
            animationClip.wrapMode = wrapMpde;
            return animationClip;
        }
    }

    [Name("getlength")]
    [Category("UnityEngine/AnimationClip")]
    [Description("获取AnimationClip动画的播放长度")]
    public class G_AnimationClipLength : PureFunctionNode<float, AnimationClip>
    {
        public override float Invoke(AnimationClip animationClip)
        {
            return animationClip.length;
        }
    }

    #endregion
}
