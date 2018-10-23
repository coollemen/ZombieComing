using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{
    public class UIView : View
    {
        public override string ViewName
        {
            get { return "UIView"; }
        }

        // Use this for initialization
        public override void Start()
        {
            base.Start();
            this.ContextName = "UIContext";

        }
        // Update is called once per frame
        void Update()
        {

        }

        public virtual void ShowView()
        {
        }

        public virtual void HideView()
        {
        }


    }
}