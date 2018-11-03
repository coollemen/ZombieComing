using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{
    public class UIManager : MonoSingleton<UIManager>
    {
        public UIConfig config;
        public Canvas canvas;
        //普通视图锚点
        public Transform normalViewAnchor;
        //固定视图锚点
        public Transform fixedViewAnchor;
        //弹出视图锚点
        public Transform popupViewAnchor;
        //所有视图模型
        public Dictionary<string, UIContext> contexts = new Dictionary<string, UIContext>();
        //所有视图
        public Dictionary<string, UIView> views = new Dictionary<string, UIView>();
        //普通视图列表
        public List<UIView> normalViews = new List<UIView>();
        //固定视图列表
        public List<UIView> fixedViews = new List<UIView>();
        //弹出视图列表
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
                LoadViewPrefab(p.prefabPath);
            }

        }
        /// <summary>
        /// 读取View的预制体
        /// </summary>
        /// <param name="path"></param>
        private void LoadViewPrefab(string path)
        {
            //读取
            var prefab = Resources.Load(path) as GameObject;
            //实例化
            var go = Instantiate(prefab);
            //设置父节点
            if (canvas != null)
            {
                var view = go.GetComponent<UIView>();
                if (view.type == UIViewType.Normal)
                {
                    go.transform.SetParent(normalViewAnchor);
                    go.transform.localPosition = Vector3.zero;
                }
                else if (view.type == UIViewType.Fixed)
                {
                    go.transform.SetParent(fixedViewAnchor);
                    go.transform.localPosition = Vector3.zero;
                }
                else if (view.type == UIViewType.Popup)
                {
                    go.transform.SetParent(popupViewAnchor);
                    go.transform.localPosition = Vector3.zero;
                    go.SetActive(false);
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