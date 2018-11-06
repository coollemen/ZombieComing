using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameFramework;
using Sirenix.OdinInspector;
using UniRx;
public class PlayerInfoView : UIView {
    [ShowInInspector]
    public override string TypeID
    {
        get
        {
            return "PlayerInfoView";
        }
    }
    [Title("Controls")]
    public Text playerLvLabel;
    public Text playerNameLabel;
    public Image playerAvatarImage;
    // Use this for initialization
    public override void Awake()
    {
        base.Awake();
    }
    public override void Start()
    {
        base.Start();
    }
    public override void Init()
    {
        base.Init();
        this.Context = new PlayerInfoContext();
        this.Context.Init();
    }
    public override void DataBinding()
    {
        if (Context == null)
        {
            Debug.LogError("Context is Null!");
        }
        //转换到匹配类型
        var c = Context as PlayerInfoContext;
        //绑定到控件
        c.playLvProperty.SubscribeToText(playerLvLabel);
        c.playNameProperty.SubscribeToText(playerNameLabel);
    }

	// Update is called once per frame
	void Update () {
		
	}
}
