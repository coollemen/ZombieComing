using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UniRx;
using UniRx.Triggers;
namespace GameFramework
{
    /// <summary>
    /// 视图
    /// </summary>
    public  class View:MonoBehaviour
    {
        /// <summary>
        /// id,用于view管理
        /// </summary>
        public int id;

        private readonly ReactiveProperty<Context> contextProperty = new ReactiveProperty<Context>();
        [ShowInInspector]
        public Context Context
        {
            get { return contextProperty.Value; }
            set
            {
                this.contextProperty.Value = value;

            }
        }

        public virtual void InitView()
        {
            if (Context==null)
            {
                this.Context = GetComponent<Context>();
            }
            contextProperty.Subscribe(ContextProperty_OnValueChanged);
        }
        public void Start()
        {

            InitView();
        }
        private void ContextProperty_OnValueChanged(Context c)
        {
            Debug.Log("Context Changed:" + c.name);
        }
    }
}
