using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller
{
    /// <summary>
    /// 获取模型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    protected Model GetModel<T>() where T : Model
    {
        return MVC.GetModel<T>();
    }

    /// <summary>
    /// 获取视图
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    protected View GeView<T>() where T : View
    {
        return MVC.GetView<T>();
    }

    /// <summary>
    /// 注册模型
    /// </summary>
    /// <param name="model"></param>
    protected void RegisterModel(Model model)
    {
        MVC.RegisterModel(model);
    }

    /// <summary>
    /// 注册视图
    /// </summary>
    /// <param name="view"></param>
    protected void RegisterView(View view)
    {
        MVC.RegisterView(view);
    }

    /// <summary>
    /// 注册控制器
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="controllerType"></param>
    protected void RegisterConteroller(string eventName, Type controllerType)
    {
        MVC.RegisterController(eventName, controllerType);
    }

    /// <summary>
    /// 处理系统消息
    /// </summary>
    /// <param name="data">数据</param>
    public abstract void Execute(object data);
}