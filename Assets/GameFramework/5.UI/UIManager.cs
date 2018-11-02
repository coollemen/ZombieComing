using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{
    public class UIManager : MonoSingleton<UIManager>
    {

        public Dictionary<string, UIContext> contexts = new Dictionary<string, UIContext>();
        public Dictionary<string, UIView> views = new Dictionary<string, UIView>();
        public List<UIView> normalViews = new List<UIView>();
        public Stack<UIView> popupViews = new Stack<UIView>();
        /// <summary>
        /// 注册视图
        /// </summary>
        /// <param name="view">视图</param>
        public void RegisterView(UIView view)
        {
            if (views.ContainsKey(view.TypeID))
            {
                views[view.TypeID] = view;
            }
            else
            {
                views.Add(view.TypeID, view);
            }
        }

        /// <summary>
        /// 注册视图模型
        /// </summary>
        /// <param name="context">视图模型</param>
        public void RegisterContext(UIContext context)
        {
            if (contexts.ContainsKey(context.TypeID))
            {
                contexts[context.TypeID] = context;
            }
            else
            {
                contexts.Add(context.TypeID, context);
            }
        }

        public UIView GetView<T>() where T : UIView
        {
            foreach (var v in views.Values)
            {
                if (v is T)
                {
                    return v;
                }
            }
            return null;
        }

        public UIView GetView(string viewID)
        {
            return views[viewID];
        }
        public UIContext GetContext<T>() where T : UIContext
        {
            foreach (var c in contexts.Values)
            {
                if (c is T)
                {
                    return c;
                }
            }
            return null;
        }

        public UIContext GetContext(string contextID)
        {
            return contexts[contextID];
        }
        public virtual void Awake()
        {
            this.LoadViews();
        }
        public virtual void Start()
        {
            
        }

        public virtual void LoadViews()
        {

        }
        public virtual void ShowView(string viewID)
        {

        }

        public virtual void HideView(string viewID)
        {

        }

        public virtual void CloseView(string viewID)
        {

        }
    }
}