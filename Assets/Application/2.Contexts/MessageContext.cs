using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using Sirenix.OdinInspector;
using UniRx;
public class MessageContext : UIContext {

//    public override string TypeID
//    {
//        get
//        {
//            return "MessageContext";
//        }
//    }

    public ReactiveProperty<string> msgProperty = new ReactiveProperty<string>();

    /// <summary>
    /// 玩家名字
    /// </summary>
    [ShowInInspector]
    public string message
    {
        get { return msgProperty.Value; }
        set { msgProperty.Value = value; }
    }
    public override void GetData()
    {
        base.GetData();
        this.message = "Test UI Popup Window!";
    }
    public void OnOK()
    {
        Debug.Log("OK Clicked!");
        UIManager.Instance.HideView<MessageView>();
    }

    public void OnCancel()
    {
        Debug.Log("Cancel Clicked!");
        UIManager.Instance.HideView<MessageView>();
    }
}
