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
    public  abstract class View:MonoBehaviour
    {
        /// <summary>
        ///视图的名称
        /// </summary>
        public abstract string ViewName { get; }
        /// <summary>
        /// 绑定的Context的名称
        /// </summary>
        public  string ContextName { get; set; }
        /// <summary>
        /// 上下文属性，支持变化检测（基于UniRx）
        /// </summary>
        protected readonly ReactiveProperty<Context> contextProperty = new ReactiveProperty<Context>();
        /// <summary>
        /// 上下文
        /// </summary>
        [ShowInInspector]
        public Context Context
        {
            get { return contextProperty.Value; }
            set
            {
                this.contextProperty.Value = value;

            }
        }
        public virtual void Start()
        {

            if (Context == null)
            {
                Debug.Log("Contxt is null!!!");
                Context = MVPManager.Instance.GetContext(ContextName);
            }
            //订阅数值变化事件
            contextProperty.Subscribe(OnContextValueChanged);
        }
        public virtual void OnContextValueChanged(Context c)
        {
            Debug.Log("Context Changed:" + c.Name);
        }
    }
}
