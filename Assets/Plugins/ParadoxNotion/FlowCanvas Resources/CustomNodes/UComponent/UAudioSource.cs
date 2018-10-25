using ParadoxNotion.Design;
using UnityEngine;


namespace FlowCanvas.Nodes
{

    #region AudioSource

    [Name("getClip(AudioSource)")]
    [Category("UnityEngine/AudioSource")]
    [Description("获取AudioSource的默认音频")]
    public class G_AudioClip : PureFunctionNode<AudioClip, AudioSource>
    {
        public override AudioClip Invoke(AudioSource audioSource)
        {
            return audioSource.clip;
        }
    }

    [Name("getIsPlaying(AudioSource)")]
    [Category("UnityEngine/AudioSource")]
    [Description("获取AudioSource的播放音频状态")]
    public class G_IsAudioPlaying : PureFunctionNode<bool, AudioSource>
    {
        public override bool Invoke(AudioSource audioSource)
        {
            return audioSource.isPlaying;
        }
    }

    [Name("getloop(AudioSource)")]
    [Category("UnityEngine/AudioSource")]
    [Description("获取AudioSource的循环播放状态")]
    public class G_IsAudioLoop : PureFunctionNode<bool, AudioSource>
    {
        public override bool Invoke(AudioSource audioSource)
        {
            return audioSource.loop;
        }
    }

    [Name("setloop(AudioSource)")]
    [Category("UnityEngine/AudioSource")]
    [Description("获取AudioSource的循环播放状态")]
    public class S_AudioLoop : CallableFunctionNode<AudioSource,AudioSource, bool>
    {
        public override AudioSource Invoke(AudioSource audioSource, bool value)
        {
            audioSource.loop = value;
            return audioSource;
        }
    }

    [Name("getMute(AudioSource)")]
    [Category("UnityEngine/AudioSource")]
    [Description("获取AudioSource的静音状态")]
    public class G_AudioMute : PureFunctionNode<bool, AudioSource>
    {
        public override bool Invoke(AudioSource audioSource)
        {
            return audioSource.mute;
        }
    }

    [Name("setMute(AudioSource)")]
    [Category("UnityEngine/AudioSource")]
    [Description("设置AudioSource的静音状态")]
    public class S_AudioMute : CallableFunctionNode<AudioSource,AudioSource, bool>
    {
        public override AudioSource Invoke(AudioSource audioSource, bool value)
        {
            audioSource.mute = value;
            return audioSource;
        }
    }

    [Name("getVolume")]
    [Category("UnityEngine/AudioSource")]
    [Description("获取AudioSource的音量")]
    public class G_Volume : PureFunctionNode<float, AudioSource>
    {
        public override float Invoke(AudioSource audioSource)
        {
            return audioSource.volume;
        }
    }

    [Name("setVolume")]
    [Category("UnityEngine/AudioSource")]
    [Description("设置AudioSource的音量")]
    public class S_AudioVolume : CallableFunctionNode<AudioSource,AudioSource, float>
    {
        public override AudioSource Invoke(AudioSource audioSource, float value)
        {
            audioSource.volume = value;
            return audioSource;
        }
    }

    [Name("getTime")]
    [Category("UnityEngine/AudioSource")]
    [Description("获取AudioSource的音量")]
    public class G_AudioTime : PureFunctionNode<float, AudioSource>
    {
        public override float Invoke(AudioSource audioSource)
        {
            return audioSource.time;
        }
    }

    [Name("setVolume")]
    [Category("UnityEngine/AudioSource")]
    [Description("设置AudioSource的静音状态")]
    public class S_AudioTime : CallableFunctionNode<AudioSource,AudioSource, float>
    {
        public override AudioSource Invoke(AudioSource audioSource, float value)
        {
            audioSource.time = value;
            return audioSource;
        }
    }

    [Name("play(AudioSource)")]
    [Category("UnityEngine/AudioSource")]
    [Description("播放音频")]
    public class PlayAudioClip : CallableFunctionNode<AudioSource, AudioSource>
    {
        public override AudioSource Invoke(AudioSource audioSource)
        {
            audioSource.Play();
            return audioSource;
        }
    }

    [Name("plaClipAtPoint(AudioSource)")]
    [Category("UnityEngine/AudioSource")]
    [Description("在某处播放音频")]
    public class PlayAudioClipAtPoint : CallableFunctionNode<AudioClip,AudioClip, Vector3, float>
    {
        public override AudioClip Invoke(AudioClip audioClip, Vector3 postion, float volume = 1f)
        {
            AudioSource.PlayClipAtPoint(audioClip, postion, volume);
            return audioClip;
        }
    }

    [Name("playOneShot(AudioSource)")]
    [Category("UnityEngine/AudioSource")]
    [Description("播放一次音频,可控制音量")]
    public class PlayAudioClipOneShot : CallableFunctionNode<AudioSource,AudioSource, AudioClip, float>
    {
        public override AudioSource Invoke(AudioSource audioSource, AudioClip audioClip, float volume)
        {
            audioSource.PlayOneShot(audioClip, volume);
            return audioSource;
        }
    }

    [Name("playScheduled(AudioSource)")]
    [Category("UnityEngine/AudioSource")]
    [Description("经过多久后开始播放音频")]
    public class PlayAudioClipScheduled : CallableFunctionNode<AudioSource,AudioSource, double>
    {
        public override AudioSource Invoke(AudioSource audioSource, double time)
        {
            audioSource.PlayScheduled(time);
            return audioSource;
        }
    }

    [Name("stop(AudioSource)")]
    [Category("UnityEngine/AudioSource")]
    [Description("停止播放音频")]
    public class StopAudioClip : CallableFunctionNode<AudioSource,AudioSource>
    {
        public override AudioSource Invoke(AudioSource audioSource)
        {
            audioSource.Stop();
            return audioSource;
        }
    }

    [Name("unpause(AudioSource)")]
    [Category("UnityEngine/AudioSource")]
    [Description("继续播放音频")]
    public class UnPauseAudioClip : CallableFunctionNode<AudioSource, AudioSource>
    {
        public override AudioSource Invoke(AudioSource audioSource)
        {
            audioSource.UnPause();
            return audioSource;
        }
    }

    [Name("pause(AudioSource)")]
    [Category("UnityEngine/AudioSource")]
    [Description("暂停播放音频")]
    public class PauseAudioClip : CallableFunctionNode<AudioSource, AudioSource>
    {
        public override AudioSource Invoke(AudioSource audioSource)
        {
            audioSource.Pause();
            return audioSource;
        }
    }

    #endregion

}