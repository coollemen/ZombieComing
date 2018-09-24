using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : Singleton<Sound> {

    public string ResourceDir = "";

    public AudioSource m_music;
    public AudioSource m_effect;

    public float MusicVolume
    {
        get { return m_music.volume; }
        set { m_music.volume = value; }
    }

    public float EffectVolume
    {
        get { return m_effect.volume; }
        set { m_effect.volume = value; }
    }
    protected override void Awake()
    {
        base.Awake();
        m_music = this.gameObject.AddComponent<AudioSource>();
        m_music.playOnAwake = false;
        m_music.loop = true;

        m_effect = this.gameObject.AddComponent<AudioSource>();

    }
    /// <summary>
    /// 播放背景音乐
    /// </summary>
    /// <param name="audioName">音频文件名</param>
    public void PlayMusic(string audioName)
    {
        string oldName;
        if (m_music.clip == null)
        {
            oldName = "";
        }
        else
        {
            oldName = m_music.clip.name;
        }

        if (oldName != audioName)
        {
            //音乐文件路径
            string path = "";
            if (string.IsNullOrEmpty(ResourceDir))
            {
                path = name;
            }
            else
            {
                path = ResourceDir + "/" + audioName;
            }
            //加载音乐
            AudioClip clip = Resources.Load<AudioClip>(path);
            //播放
            if (clip != null)
            {
                m_music.clip = clip;
                m_music.Play();
            }
        }
       
    }
    /// <summary>
    /// 停止音乐
    /// </summary>
    public void StopMusic()
    {
        m_music.Stop();
        m_music.clip = null;
    }
    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="audioName">音频文件名</param>
    public void PlayEffect(string audioName)
    {
        //音乐文件路径
        string path = "";
        if (string.IsNullOrEmpty(ResourceDir))
        {
            path = name;
        }
        else
        {
            path = ResourceDir + "/" + audioName;
        }
        //加载音乐
        AudioClip clip = Resources.Load<AudioClip>(path);
        //播放
        m_effect.PlayOneShot(clip);
    }
}
