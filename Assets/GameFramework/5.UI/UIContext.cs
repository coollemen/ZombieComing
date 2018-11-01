using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDesigner;
namespace GameFramework
{
    /// <summary>
    /// UI视图模型
    /// </summary>
    public abstract class UIContext :MonoBehaviour, IContext
    {
        public virtual string TypeID
        {
            get { return "UIContext"; }
        }


        public virtual void GetData()
        {
            
        }

        public virtual void SetData()
        {
            
        }

        public virtual void Awake()
        {
            //注册视图模型
            UIManager.Instance.RegisterContext(this);
            //从模型获取数据
            this.GetData();
        }
    }
}
