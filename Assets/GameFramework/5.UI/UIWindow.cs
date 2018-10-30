using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace GameFramework
{
    /// <summary>
    /// 这里的窗口是狭义的窗口，
    /// </summary>
    public class UIWindow : UIView
    {
        public Text titleLabel;
        public Button closeButton;

        public override string TypeID
        {
            get
            {
                return "UIWindow";
            }
        }
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}