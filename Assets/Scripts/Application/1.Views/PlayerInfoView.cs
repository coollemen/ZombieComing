using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameFramework;
using Sirenix.OdinInspector;
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
    void Start () {
		
	}

    public void BindingData()
    {
        var context = UIManager.Instance.GetContext<PlayerInfoContext>() as PlayerInfoContext;
        context.playLvProperty.Subscribe(lv =>playerLvLabel.text=lv);
    }

    public void OnLvChanged(string newLv)
    {

    }
	// Update is called once per frame
	void Update () {
		
	}
}
