using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using GameFramework;
public class PlayerInfoContext : UIContext
{

//    public override string TypeID
//    {
//        get
//        {
//            return "PlayerInfoContext";
//        }
//    }
    public ReactiveProperty<int> playLvProperty = new ReactiveProperty<int>();

    /// <summary>
    /// 玩家等级
    /// </summary>
    public int playerLv
    {
        get { return this.playLvProperty.Value; }
        set { this.playLvProperty.Value = value; }
    }


    public ReactiveProperty<string> playNameProperty = new ReactiveProperty<string>();

    /// <summary>
    /// 玩家名字
    /// </summary>
    public string playerName
    {
        get { return playNameProperty.Value; }
        set { playNameProperty.Value = value; }
    }

    public ReactiveProperty<Texture2D> playAvatarProperty = new ReactiveProperty<Texture2D>();
    /// <summary>
    /// 玩家头像
    /// </summary>
    public Texture2D playerAvatar
    {
        get { return playAvatarProperty.Value; }
        set { playAvatarProperty.Value = value; }
    }
    public override void GetData()
    {
        base.GetData();
        this.playerLv = 1;
        this.playerName = "小喵喵";
    }
}
