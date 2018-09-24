using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public abstract class View : MonoBehaviour
{
    //视图标识
    public abstract string Name { get; }

    //关心的事件列表
    public List<string> AttentionEvents = new List<string>();

    //事件处理函数
    public abstract void HandleEvent(string eventName, object data);

    //获取模型
    protected Model GetModel<T>()
        where T:Model
    {
        return MVC.GetModel<T>();
    }

    //发送消息
    protected void SendEvent(string eventName,object data=null)
    {
        MVC.SendEvent(eventName, data);
    }
}