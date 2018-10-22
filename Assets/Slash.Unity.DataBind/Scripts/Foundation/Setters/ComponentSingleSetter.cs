// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComponentSingleSetter.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Setters
{
    using System.Diagnostics.CodeAnalysis;

    using Slash.Unity.DataBind.Core.Presentation;

    using UnityEngine;
    using UnityEngine.Serialization;

    /// <summary>
    ///   Base class for a setter for a component.
    /// </summary>
    /// <typeparam name="TComponent">Type of component.</typeparam>
    /// <typeparam name="TData">Type of data to set.</typeparam>
    public abstract class ComponentSingleSetter<TComponent, TData> : SingleSetter<TData>, ISerializationCallbackReceiver
        where TComponent : Component
    {
        /// <summary>
        ///   Target to get the data from.
        /// </summary>
        public DataBinding TargetBinding;

        /// <summary>
        ///   For backward compatibility.
        /// </summary>
        [FormerlySerializedAs("Target")]
        [SerializeField]
        [HideInInspector]
        private TComponent constantTarget;

        /// <summary>
        ///   Current target to get data from.
        /// </summary>
        protected TComponent Target
        {
            get
            {
                return this.TargetBinding.GetValue<TComponent>();
            }
        }

        /// <summary>
        ///   <para>
        ///     Implement this method to receive a callback after Unity deserializes your object.
        ///   </para>
        /// </summary>
        public void OnAfterDeserialize()
        {
            if (!this.IsTargetBindingSet())
            {
                this.TargetBinding = new DataBinding
                {
                    Type = DataBindingType.Reference,
                    Reference = this.constantTarget
                };
                this.constantTarget = null;
            }
        }

        /// <summary>
        ///   <para>
        ///     Implement this method to receive a callback before Unity serializes your object.
        ///   </para>
        /// </summary>
        public void OnBeforeSerialize()
        {
        }

        /// <summary>
        ///   Unity callback.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            this.AddBinding(this.TargetBinding);
        }

        /// <summary>
        ///   Unity callback.
        /// </summary>
        protected override void OnDestroy()
        {
            base.OnDestroy();
            this.RemoveBinding(this.TargetBinding);
        }

        /// <summary>
        ///   Called when the data binding value changed.
        /// </summary>
        /// <param name="newValue">New data value.</param>
        protected override void OnObjectValueChanged(object newValue)
        {
            if (this.Target != null)
            {
                base.OnObjectValueChanged(newValue);
            }
        }

        /// <summary>
        ///   Unity callback.
        /// </summary>
        [SuppressMessage("ReSharper", "VirtualMemberNeverOverriden.Global")]
        protected virtual void Reset()
        {
            if (!this.IsTargetBindingSet())
            {
                this.TargetBinding = new DataBinding
                {
                    Reference = this.constantTarget ?? this.GetComponent<TComponent>()
                };
            }
        }

        private bool IsTargetBindingSet()
        {
            return
                !(this.TargetBinding == null
                  || this.TargetBinding.Type == DataBindingType.Context && string.IsNullOrEmpty(this.TargetBinding.Path));
        }
    }
}