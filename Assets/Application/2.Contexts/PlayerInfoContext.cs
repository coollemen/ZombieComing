using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Sirenix.OdinInspector;
using GameFramework;
public class PlayerInfoContext : UIContext
{

    public override string TypeID
    {
        get
        {
            return "PlayerInfoContext";
        }
    }
    public ReactiveProperty<int> playLvProperty = new ReactiveProperty<int>();

    /// <summary>
    /// 玩家等级
    /// </summary>
    [ShowInInspector]
    public int playerLv
    {
        get { return this.playLvProperty.Value; }
        set { this.playLvProperty.Value = value; }
    }


    public ReactiveProperty<string> playNameProperty = new ReactiveProperty<string>();

    /// <summary>
    /// 玩家名字
    /// </summary>
    [ShowInInspector]
    public string playerName
    {
        get { return playNameProperty.Value; }
        set { playNameProperty.Value = value; }
    }



    public ReactiveProperty<Texture2D> playAvatarProperty = new ReactiveProperty<Texture2D>();

    /// <summary>
    /// 玩家头像
    /// </summary>
    [ShowInInspector]
    public Texture2D playerAvatar
    {
        get { return playAvatarProperty.Value; }
        set { playAvatarProperty.Value = value; }
    }
    public override void Awake()
    {
        base.Awake();
        this.GetData();
    }
    public override void GetData()
    {
        base.GetData();
        this.playerLv = 1;
        this.playerName = "小喵喵";
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

 
}
