using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{
    /// <summary>
    /// UI控制器
    /// </summary>
    public class UIController : MonoBehaviour, IController
    {
        public virtual string TypeID
        {
            get { return "UIController"; }
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