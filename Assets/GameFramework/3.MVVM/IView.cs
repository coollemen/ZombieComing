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
        string ContextID { get; }
//        /// <summary>
//        /// 上下文属性，支持变化检测（基于UniRx）
//        /// </summary>
//        protected readonly ReactiveProperty<Context> contextProperty = new ReactiveProperty<Context>();
//        /// <summary>
//        /// 上下文
//        /// </summary>
//        [ShowInInspector]
//        public Context Context
//        {
//            get { return contextProperty.Value; }
//            set
//            {
//                this.contextProperty.Value = value;
//
//            }
//        }
//        public virtual void Start()
//        {
//
//            if (Context == null)
//            {
//                Debug.Log("Contxt is null!!!");
//                Context = MVVM.Instance.GetContext(ContextID);
//            }
//            //订阅数值变化事件
//            contextProperty.Subscribe(OnContextValueChanged);
//        }
//        public virtual void OnContextValueChanged(Context c)
//        {
//            Debug.Log("Context Changed:" + c.TypeID);
//        }
    }
}
