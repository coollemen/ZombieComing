using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace GameFramework
{
    public class UIView : View
    {
        public InputField usernameInput;
        public InputField passwordInput;
        public Button loginButton;
        public Button cancelButton;
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