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
    [ShowInInspector]
    public override string ContextID
    {
        get
        {
            return "PlayerInfoContext";
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

    public override void DataBinding()
    {
        var context = UIManager.Instance.GetContext<PlayerInfoContext>() as PlayerInfoContext;
        context.playLvProperty.SubscribeToText(playerLvLabel);
        context.playNameProperty.SubscribeToText(playerNameLabel);
    }

	// Update is called once per frame
	void Update () {
		
	}
}
