using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
namespace GameFramework
{
    public  class View:MonoBehaviour
    {
        /// <summary>
        /// id,用于view管理
        /// </summary>
        public int id;

        private readonly BindableProperty<Context> contextProperty = new BindableProperty<Context>();
        [ShowInInspector]
        public Context Context
        {
            get { return contextProperty.Value; }
            set { this.contextProperty.Value = value; }
        }

        public virtual void InitView()
        {
            this.contextProperty.OnValueChanged += ContextProperty_OnValueChanged;
        }

        private void ContextProperty_OnValueChanged()
        {
            throw new System.NotImplementedException();
        }
    }
}
