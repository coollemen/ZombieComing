using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UniRx;
using UniRx.Triggers;
namespace GameFramework
{
    /// <summary>
    /// 视图
    /// </summary>
    public  interface IView
    {
        /// <summary>
        ///视图的名称
        /// </summary>
        string TypeID { get; }
        /// <summary>
        /// 绑定的Context的名称
        /// </summary>
//        string ContextID { get; }
        /// <summary>
        /// 视图模型
        /// </summary>
        UIContext Context { get; set; }
        /// <summary>
        /// 数据绑定
        /// </summary>
        void DataBinding();
    }
}
