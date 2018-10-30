using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDesigner;
namespace GameFramework
{
    /// <summary>
    /// UI视图模型
    /// </summary>
    public class UIContext :MonoBehaviour, IContext
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


    }
}
