using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{
    public class UIManager : MonoSingleton<UIManager>
    {
        public UIConfig config;
        public Canvas canvas;
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
            this.ShowViews();
        }

        public virtual void LoadViews()
        {
            if (config == null)
            {
                Debug.LogError("UIManager 缺少配置文件！");
            }
            foreach (var p in config.paths)
            {
                var prefab = Resources.Load(p.prefabPath) as GameObject;
                var go = Instantiate(prefab);
                if (canvas != null)
                {
                    go.transform.SetParent(canvas.transform);
                    go.transform.localPosition = Vector3.zero;
                }
            }

        }

        public virtual void ShowViews()
        {
            foreach (var v in views.Values)
            {
                v.Show();
            }
        }
        public virtual void ShowView(string viewID)
        {
            views[viewID].Show();

        }

        public virtual void HideView(string viewID)
        {
            views[viewID].Hide();
        }

        public virtual void CloseView(string viewID)
        {

        }
    }
}