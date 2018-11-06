using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameFramework;
using UniRx;

public class MessageView : UIView {
    public override string TypeID
    {
        get
        {
            return "MessageView";
        }
    }
    public Text msgLabel;
    public Button okButton;
    public Button cancelButton;
    public override void Init()
    {
        base.Init();
        this.Context = new MessageContext();
        this.Context.Init();
    }
    public override void DataBinding()
    {
        base.DataBinding();
        var c = this.Context as MessageContext;
        c.msgProperty.SubscribeToText(msgLabel);
        okButton.OnClickAsObservable().Subscribe(_ => c.OnOK());
        cancelButton.OnClickAsObservable().Subscribe(_ => c.OnCancel());
    }

}
