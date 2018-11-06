using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDesigner;
namespace GameFramework
{
    /// <summary>
    /// UI视图模型
    /// </summary>
    public abstract class UIContext: IContext
    {
//        public virtual string TypeID
//        {
//            get { return "UIContext"; }
//        }


        public virtual void GetData()
        {
            
        }

        public virtual void SetData()
        {
            
        }

        public virtual void Init()
        {
            //从模型获取数据
            this.GetData();
        }

    }
}
