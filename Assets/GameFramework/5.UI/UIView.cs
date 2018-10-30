using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace GameFramework
{
    /// <summary>
    /// UI视图
    /// </summary>
    public class UIView : MonoBehaviour, IView
    {
        public virtual string TypeID
        {
            get { return "UIView"; }
        }

        public virtual string ContextID
        {
            get { return "UIContext"; }
        }

        public virtual void ShowView()
        {
            Debug.Log("Show View!!!");
        }

        public virtual void HideView()
        {
            Debug.Log("Hide View!!!");
        }
    }
}