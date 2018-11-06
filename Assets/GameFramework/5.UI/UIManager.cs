using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace GameFramework
{
    public class UIManager : MonoSingleton<UIManager>
    {
        public UIConfig config;
        public Canvas canvas;
        //游戏Hud视图锚点
        public Transform hudViewAnchor;
        //固定视图锚点
        public Transform fixedViewAnchor;
        //普通视图锚点
        public Transform normalViewAnchor;
        //置顶视图锚点
        public Transform topViewAnchor;
        //弹出视图锚点
        public Transform popupViewAnchor;
        //遮罩物体
        public GameObject maskObject;
        //Prefab视图缓存
        public Dictionary<string, UIView> viewPrefabs = new Dictionary<string, UIView>();
        //视图缓存
        public Dictionary<string, UIView> viewCaches = new Dictionary<string, UIView>();
        //所有视图
        public Dictionary<string, UIView> currViews = new Dictionary<string, UIView>();
        //普通视图列表
        public Dictionary<string, UIView> normalViews = new Dictionary<string, UIView>();
        //固定视图列表
        public Dictionary<string, UIView> fixedViews = new Dictionary<string, UIView>();
        //弹出视图列表
        public Stack<UIView> popupViews = new Stack<UIView>();
        public virtual void Awake()
        {
            this.LoadViewsPrefab();
        }
        public virtual void Start()
        {
        }
        #region View Prefab
        /// <summary>
        /// 注册视图Prefab
        /// </summary>
        /// <param name="view">视图</param>
        protected void RegisterViewPrefab(UIView view)
        {
            if (viewPrefabs.ContainsKey(view.TypeID))
            {
                viewPrefabs[view.TypeID] = view;
            }
            else
            {
                viewPrefabs.Add(view.TypeID, view);
            }

        }

        /// <summary>
        /// 获取视图Prefab
        /// </summary>
        /// <typeparam name="T">UIViewType</typeparam>
        /// <returns>UIView</returns>
        protected UIView GetViewPrefab<T>() where T : UIView
        {
            foreach (var v in viewPrefabs.Values)
            {
                if (v is T)
                {
                    return v;
                }
            }
            return null;
        }
        /// <summary>
        /// 读取View的预制体
        /// </summary>
        /// <param name="path"></param>
        private void LoadViewPrefab(string path)
        {
            //读取prefab
            var prefab = Resources.Load(path) as GameObject;
            //注册视图类型
            var viewPrefab = prefab.GetComponent<UIView>();
            this.RegisterViewPrefab(viewPrefab);
        }
        /// <summary>
        /// 读取视图prefab
        /// </summary>
        public virtual void LoadViewsPrefab()
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
        #endregion
        #region views
        /// <summary>
        /// 创建视图
        /// </summary>
        /// <param name="name">视图名</param>
        /// <param name="view">UIView</param>
        public void CreateView<T>(string name) where T : UIView
        {
            var viewPrefab = GetViewPrefab<T>();
            if (viewPrefab != null)
            {
                //实例化
                var go = Instantiate(viewPrefab.gameObject);
                //设置父节点
                if (canvas != null)
                {
                    var view = go.GetComponent<UIView>();
                    if (view != null)
                    {
                        //获取视图并注册
                        if (currViews.ContainsKey(name))
                        {
                            currViews[name] = view;
                        }
                        else
                        {
                            currViews.Add(name, view);
                        }
                        //根据不同视图类型，添加到不同的锚点
                        if (view.type == UIViewType.Hud)
                        {
                            go.transform.SetParent(hudViewAnchor);
                            go.transform.localPosition = Vector3.zero;
                            SortViewsByOrderID(hudViewAnchor);
                        }
                        else if (view.type == UIViewType.Fixed)
                        {
                            go.transform.SetParent(fixedViewAnchor);
                            go.transform.localPosition = Vector3.zero;
                            SortViewsByOrderID(fixedViewAnchor);

                        }
                        else if (view.type == UIViewType.Normal)
                        {
                            go.transform.SetParent(normalViewAnchor);
                            go.transform.localPosition = Vector3.zero;
                            SortViewsByOrderID(normalViewAnchor);
                        }
                        else if (view.type == UIViewType.Top)
                        {
                            go.transform.SetParent(topViewAnchor);
                            go.transform.localPosition = Vector3.zero;
                            SortViewsByOrderID(topViewAnchor);
                        }
                        else if (view.type == UIViewType.Popup)
                        {
                            go.transform.SetParent(popupViewAnchor);
                            go.transform.localPosition = Vector3.zero;
                            SortViewsByOrderID(popupViewAnchor);
                        }
                        //隐藏
                        go.GetComponent<CanvasGroup>().alpha = 0;
                    }
                }
                else
                {
                    Debug.LogError("未找到相应的Prefab，注册视图失败！");
                }
            
            }
        }
        /// <summary>
        /// 根据视图的OrderID进行升序排序
        /// </summary>
        /// <param name="anchor"></param>
        public void SortViewsByOrderID(Transform anchor)
        {
            UIView[] views = anchor.GetComponentsInChildren<UIView>();
            List<UIView> viewList = new List<UIView>(views);
            viewList=viewList.OrderBy(v => v.orderID).ToList();
            for (int i = 0; i < viewList.Count; i++)
            {
                viewList[i].transform.SetSiblingIndex(i);
            }
        }

        public UIView GetView<T>()
        {
            var name = typeof(T).Name;
            return this.GetView(name);
        }

        public UIView GetView(string name)
        {
            if (currViews.ContainsKey(name))
            {
                return currViews[name];
            }
            else
            {
                return null;
            }
        }
        public virtual void ShowView<T>() where T:UIView
        {
            var name = typeof(T).Name;
            this.ShowView<T>(name);
        }
        public virtual void ShowView<T>(string name) where T : UIView
        {
            //如果视图不在缓存中，那么创建视图
            if (!currViews.ContainsKey(name))
            {
                this.CreateView<T>(name);
            }
            this.ShowView(name);
        }

        public virtual void ShowView(string name)
        {
            var view = currViews[name];
            if (view.type == UIViewType.Popup)
            {
                //如果是弹出窗口
                int index = view.transform.GetSiblingIndex();
                maskObject.transform.SetParent(view.transform.parent);
                maskObject.transform.SetSiblingIndex(index);
                maskObject.SetActive(true);
            }
            view.Show();
        }

        public virtual void HideView<T>() where T : UIView
        {
            var name = typeof(T).Name;
            Debug.Log("Hide View " + name + "!");
            this.HideView(name);
        }
        public virtual void HideView(string name)
        {
            var view = currViews[name];

            if (view.type == UIViewType.Popup)
            {
                //如果是弹出窗口
                maskObject.transform.SetParent(canvas.transform);
                maskObject.SetActive(false);
            }
            view.Hide();
        }
        public virtual void CloseView<T>() where T : UIView
        {
            var name = typeof(T).Name;
            Debug.Log("Close View " + name + "!");
            this.CloseView(name);
        }
        public virtual void CloseView(string name)
        {
            currViews.Remove(name);
        }
        #endregion
    }
}