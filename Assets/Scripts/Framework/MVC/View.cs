using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class View : MonoBehaviour
{
    public abstract string Name { get; }

    /// <summary>
    /// 事件列表
    /// </summary>
    public List<string> attationEvents = new List<string>();

    /// <summary>
    /// 处理事件
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="data"></param>
    public abstract void HandleEvent(string eventName, object data);

    protected Model GetModel<T>() where T : Model
    {
        return MVC.GetModel<T>();
    }

    /// <summary>
    /// 发送事件
    /// </summary>
    /// <param name="eventName">事件名</param>
    /// <param name="data">数据</param>
    protected void SendEvent(string eventName, object data = null)
    {
        MVC.SendEvent(eventName, data);
    }
}