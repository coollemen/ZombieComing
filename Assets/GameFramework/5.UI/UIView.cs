using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace GameFramework
{
    /// <summary>
    /// UI视图
    /// </summary>
    public abstract class  UIView : MonoBehaviour, IView
    {
        public UIViewType type = UIViewType.Normal;
        public UIViewShowMode showMode = UIViewShowMode.Normal;
        public virtual string TypeID
        {
            get { return "UIView"; }
        }

        public virtual string ContextID
        {
            get { return "UIContext"; }
        }

        public virtual void Awake()
        {
            UIManager.Instance.RegisterView(this);
        }
        public virtual void Start()
        {

        }
        /// <summary>
        /// 显示状态
        /// </summary>
        public virtual void Show()
        {
            
        }
        /// <summary>
        /// 隐藏状态，窗口不显示时处于隐藏状态，而不是删除
        /// </summary>
        public virtual void Hide()
        {
            
        }
        /// <summary>
        /// 冻结状态，在模式窗口显示时，底下的窗口处于冻结状态
        /// </summary>
        public virtual void Freeze()
        {
            
        }
        /// <summary>
        /// 关闭视图，调用Destroy函数
        /// </summary>
        public virtual void Close()
        {
            
        }
    }
}