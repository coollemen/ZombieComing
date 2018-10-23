using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{
    public class MVPManager : MonoSingleton<MVPManager>
    {
        public Dictionary<string, Context> contexts = new Dictionary<string, Context>();
        public Dictionary<string, View> views = new Dictionary<string, View>();

        /// <summary>
        /// 注册视图
        /// </summary>
        /// <param name="view">视图</param>
        public void RegisterView(View view)
        {
            if (views.ContainsKey(view.ViewName))
            {
                views[view.ViewName] = view;
            }
            else
            {
                views.Add(view.ViewName, view);
            }
        }

        /// <summary>
        /// 注册视图模型
        /// </summary>
        /// <param name="context">视图模型</param>
        public void RegisterContext(Context context)
        {
            if (contexts.ContainsKey(context.Name))
            {
                contexts[context.Name] = context;
            }
            else
            {
                contexts.Add(context.Name, context);
            }
        }

        public View GetView<T>() where T : View
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

        public View GetView(string viewName)
        {
            return views[viewName];
        }
        public Context GetContext<T>() where T : Context
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

        public Context GetContext(string contextName)
        {
            return contexts[contextName];
        }

    }
}
