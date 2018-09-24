using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MVC
{
    /// <summary>
    /// 模型
    /// </summary>
    public static Dictionary<string, Model> models = new Dictionary<string, Model>();
    /// <summary>
    /// 视图
    /// </summary>
    public static Dictionary<string, View> views = new Dictionary<string, View>();
    /// <summary>
    /// 事件
    /// </summary>
    public static Dictionary<string, Type> commandMap= new Dictionary<string, Type>();
    /// <summary>
    /// 注册模型
    /// </summary>
    /// <param name="model"></param>
    public static void RegisterModel(Model model)
    {
        models[model.Name] = model;
    }
    /// <summary>
    /// 注册视图
    /// </summary>
    /// <param name="view"></param>
    public static void RegisterView(View view)
    {
        views[view.Name] = view;
    }
    /// <summary>
    /// 注册控制器
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="controllerType"></param>
    public static void RegisterController(string eventName,Type controllerType)
    {
        commandMap[eventName] = controllerType;
    }

    public static Model GetModel<T>() where T : Model
    {
        foreach (Model m in models.Values)
        {
            if (m is T)
                return m;
        }
        return null;
    }

    public static View GetView<T>() where T : View
    {
        foreach (View v in views.Values)
        {
            if (v is T)
                return v;
        }
        return null;
    }

    public static void SendEvent(string eventName, object data = null)
    {
        //控制器响应事件
        if (commandMap.ContainsKey(eventName))
        {
            Type t = commandMap[eventName];
            Controller c = Activator.CreateInstance(t) as Controller;
            c.Execute(data);
        }
        //视图响应事件
        foreach (View v in views.Values)
        {
            if (v.attationEvents.Contains(eventName))
            {
                v.HandleEvent(eventName, data);
            }
        }
    }
}
