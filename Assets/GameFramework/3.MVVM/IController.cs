using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{
    /// <summary>
    /// MVVM中虽然没有Controller，但为了与FlowCanvas插件配合，实现高效和可视化控制
    /// 更方便的与Unity的组件方式配合
    /// </summary>
    public  interface IController 
    {
        /// <summary>
        ///控制器的名称
        /// </summary>
        string TypeID { get; }
    }
}
