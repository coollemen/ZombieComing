// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComponentSingleGetter.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Slash.Unity.DataBind.Core.Presentation;
using Slash.Unity.DataBind.Core.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace Slash.Unity.DataBind.Foundation.Providers.Getters
{
    /// <summary>
    ///     Base class for a getter for a component which modifies a single data value.
    /// </summary>
    /// <typeparam name="TComponent">Type of component to get value from.</typeparam>
    /// <typeparam name="TData">Type of data which is modified.</typeparam>
    public abstract class ComponentSingleGetter<TComponent, TData> : DataProvider, ISerializationCallbackReceiver
        where TComponent : Component
    {
        #region Properties

        /// <summary>
        ///     Current data value.
        /// </summary>
        public override object Value
        {
            get
            {
                return this.Target != null ? this.GetValue(this.Target) : default(TData);
            }
        }

        #endregion

        /// <summary>
        ///   Unity callback.
        /// </summary>
        public void OnAfterDeserialize()
        {
            if (!this.IsTargetBindingSet())
            {
                this.TargetBinding = new DataBinding {Type = DataBindingType.Reference, Reference = this.constantTarget};
            }
        }

        /// <summary>
        ///   Unity callback.
        /// </summary>
        public void OnBeforeSerialize()
        {
        }

        #region Public Methods and Operators

        /// <summary>
        ///     Has to be called when an anchestor context changed as the data value may change.
        /// </summary>
        public override void OnContextChanged()
        {
            base.OnContextChanged();

            if (this.node == null)
            {
                return;
            }

            this.node.OnHierarchyChanged();

            // Update value.
            this.UpdateDataValue();
        }

        #endregion

        private bool IsTargetBindingSet()
        {
            return !(this.TargetBinding == null ||
                     this.TargetBinding.Type == DataBindingType.Context && string.IsNullOrEmpty(this.TargetBinding.Path));
        }

        #region Fields

        /// <summary>
        ///     Path to value in data context.
        /// </summary>
        [ContextPath(Filter = ~ContextMemberFilter.Methods | ~ContextMemberFilter.Contexts)]
        public string Path;

        /// <summary>
        ///     Target component.
        /// </summary>
        [FormerlySerializedAs("Target")]
        [SerializeField]
        private TComponent constantTarget;

        /// <summary>
        ///   Binding that provides the target component.
        /// </summary>
        public DataBinding TargetBinding;
        
        /// <summary>
        ///   Current target component to get data value from.
        /// </summary>
        public TComponent Target
        {
            get
            {
                return this.TargetBinding.GetValue<TComponent>();
            }
        }

        /// <summary>
        ///     Cache for contexts and master paths.
        /// </summary>
        private ContextNode node;

        #endregion

        #region Methods

        /// <summary>
        ///     Register listener at target to be informed if its value changed.
        ///     The target is already checked for null reference.
        /// </summary>
        /// <param name="target">Target to add listener to.</param>
        protected abstract void AddListener(TComponent target);

        /// <summary>
        ///     Unity callback.
        /// </summary>
        [SuppressMessage("ReSharper", "VirtualMemberNeverOverriden.Global")]
        protected virtual void Awake()
        {
            this.node = new ContextNode(this.gameObject, this.Path);
            this.AddBinding(this.TargetBinding);
        }

        /// <summary>
        ///     Unity callback.
        /// </summary>
        protected virtual void OnDestroy()
        {
            this.RemoveBinding(this.TargetBinding);
        }

        /// <summary>
        ///     Derived classes should return the current value to set if this method is called.
        ///     The target is already checked for null reference.
        /// </summary>
        /// <param name="target">Target to get value from.</param>
        /// <returns>Current value to set.</returns>
        protected abstract TData GetValue(TComponent target);

        /// <summary>
        ///     Unity callback.
        /// </summary>
        protected override void OnDisable()
        {
            base.OnDisable();

            if (this.Target != null)
            {
                this.RemoveListener(this.Target);
            }
        }

        /// <summary>
        ///     Unity callback.
        /// </summary>
        protected override void OnEnable()
        {
            base.OnEnable();

            if (this.Target != null)
            {
                this.AddListener(this.Target);
            }
        }

        /// <summary>
        ///     Has to be called by derived classes when the value may have changed.
        /// </summary>
        protected void OnTargetValueChanged()
        {
            this.UpdateDataValue();
            this.OnValueChanged(this.Value);
        }

        /// <summary>
        ///     Remove listener from target which was previously added in AddListener.
        ///     The target is already checked for null reference.
        /// </summary>
        /// <param name="target">Target to remove listener from.</param>
        protected abstract void RemoveListener(TComponent target);

        /// <summary>
        ///     Unity callback.
        /// </summary>
        [SuppressMessage("ReSharper", "VirtualMemberNeverOverriden.Global")]
        protected virtual void Reset()
        {
            if (!this.IsTargetBindingSet())
            {
                this.TargetBinding = new DataBinding
                {
                    Type = DataBindingType.Reference,
                    Reference = this.GetComponent<TComponent>()
                };
            }
        }

        /// <summary>
        ///     Unity callback.
        /// </summary>
        protected override void Start()
        {
            base.Start();

            // Initial update of data value.
            this.UpdateDataValue();

            // Inform that value changed.
            this.OnValueChanged(this.Value);
        }

        /// <summary>
        ///     Called when the value of the data provider should be updated.
        /// </summary>
        protected override void UpdateValue()
        {
            // Not required, data comes from presentation side.
        }

        private void UpdateDataValue()
        {
            this.node.SetValue(this.Value);
        }

        #endregion
    }
}