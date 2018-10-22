// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnityEventTrigger.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Triggers
{
    using System;

    using Slash.Unity.DataBind.Core.Data;
    using Slash.Unity.DataBind.Core.Presentation;
    using Slash.Unity.DataBind.Core.Utils;

    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    ///   Raises Unity events whenever a specified context trigger is invoked.
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Triggers/[DB] Unity Event Trigger")]
    public class UnityEventTrigger : MonoBehaviour, IContextOperator
    {
        /// <summary>
        ///   Path of trigger that initiates the unity event.
        /// </summary>
        [ContextPath(Filter = ContextMemberFilter.Triggers | ContextMemberFilter.Recursive)]
        public string Path;

        /// <summary>
        ///   Unity event fired when trigger is invoked.
        /// </summary>
        public TriggerInvokedEvent TriggerInvoked;

        private ContextNode node;

        private DataTrigger trigger;

        private DataTrigger Trigger
        {
            set
            {
                if (value == this.trigger)
                {
                    return;
                }

                if (this.trigger != null)
                {
                    this.trigger.Invoked -= this.OnTriggerInvoked;
                }

                this.trigger = value;

                if (this.trigger != null)
                {
                    this.trigger.Invoked += this.OnTriggerInvoked;
                }
            }
        }

        /// <summary>
        ///   Has to be called when an anchestor context changed as the data value may change.
        /// </summary>
        public void OnContextChanged()
        {
            if (this.node != null)
            {
                this.node.OnHierarchyChanged();
            }
        }

        /// <summary>
        ///   Unity callback.
        /// </summary>
        protected virtual void Awake()
        {
            this.node = new ContextNode(this.gameObject, this.Path);
        }

        /// <summary>
        ///   Unity callback.
        /// </summary>
        protected void OnDestroy()
        {
            this.Trigger = null;

            this.node.SetValueListener(null);
            this.node = null;
        }

        /// <summary>
        ///   Unity callback.
        /// </summary>
        protected virtual void OnDisable()
        {
            this.node.SetValueListener(null);
        }

        /// <summary>
        ///   Unity callback.
        /// </summary>
        protected virtual void OnEnable()
        {
            var value = this.node.SetValueListener(this.OnObjectValueChanged);
            if (this.node.IsInitialized)
            {
                this.OnObjectValueChanged(value);
            }
        }

        /// <summary>
        ///   Called when the data binding value changed.
        /// </summary>
        /// <param name="newValue">New data value.</param>
        protected virtual void OnObjectValueChanged(object newValue)
        {
            this.Trigger = newValue as DataTrigger;
        }

        private void OnTriggerInvoked()
        {
            this.TriggerInvoked.Invoke(this.node.Context);
        }

        /// <summary>
        ///   Event to fire when trigger was invoked.
        /// </summary>
        [Serializable]
        public class TriggerInvokedEvent : UnityEvent<object>
        {
        }
    }
}