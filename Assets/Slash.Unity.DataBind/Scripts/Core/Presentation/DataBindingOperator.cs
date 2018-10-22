// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataBindingOperator.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Core.Presentation
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using UnityEngine;

    /// <summary>
    ///   Base class of a behaviour that uses on one or more data bindings.
    /// </summary>
    public abstract class DataBindingOperator : MonoBehaviour, IContextOperator
    {
        #region Fields

        private readonly List<DataBinding> bindings = new List<DataBinding>();

        /// <summary>
        ///   Indicates if the provider listens to value changes of its bindings.
        /// </summary>
        private bool isMonitoringBindings;

        #endregion

        #region Properties

        /// <summary>
        ///   Indicates if the data provider already holds a valid value.
        /// </summary>
        public bool IsInitialized
        {
            get
            {
                return this.bindings == null || this.bindings.All(binding => binding.IsInitialized);
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Has to be called when an anchestor context changed as the data value may change.
        /// </summary>
        public virtual void OnContextChanged()
        {
            foreach (var binding in this.bindings)
            {
                binding.OnContextChanged();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///   Adds and initializes the specified binding.
        /// </summary>
        /// <param name="binding">Binding to add.</param>
        protected void AddBinding(DataBinding binding)
        {
            // Init.
            binding.Init(this.gameObject);

            if (this.isMonitoringBindings)
            {
                binding.ValueChanged += this.OnBindingValueChanged;
            }

            this.bindings.Add(binding);
        }

        /// <summary>
        ///   Adds and initializes the specified bindings.
        /// </summary>
        /// <param name="newBindings">Bindings to add.</param>
        protected void AddBindings(IEnumerable<DataBinding> newBindings)
        {
            foreach (var binding in newBindings)
            {
                this.AddBinding(binding);
            }
        }

        /// <summary>
        ///   Called when a value of the bindings of this operator changed.
        /// </summary>
        protected virtual void OnBindingValuesChanged()
        {
        }

        /// <summary>
        ///   Unity callback.
        /// </summary>
        [SuppressMessage("ReSharper", "UnusedMemberHiearchy.Global")]
        protected virtual void OnDisable()
        {
            this.UnregisterFromValueChanges();
        }

        /// <summary>
        ///   Unity callback.
        /// </summary>
        [SuppressMessage("ReSharper", "UnusedMemberHiearchy.Global")]
        protected virtual void OnEnable()
        {
            this.RegisterForValueChanges();
            var bindingsInitialized = this.bindings.All(binding => binding.IsInitialized);
            if (bindingsInitialized)
            {
                this.OnBindingValuesChanged();
            }
        }

        /// <summary>
        ///   Removes and deinitializes the specified binding.
        /// </summary>
        /// <param name="binding">Binding to remove.</param>
        protected void RemoveBinding(DataBinding binding)
        {
            if (this.isMonitoringBindings)
            {
                binding.ValueChanged -= this.OnBindingValueChanged;
            }

            // Deinit.
            binding.Deinit();

            this.bindings.Remove(binding);
        }

        /// <summary>
        ///   Unity callback.
        /// </summary>
        [SuppressMessage("ReSharper", "UnusedMemberHiearchy.Global")]
        protected virtual void Start()
        {
            var bindingsInitialized = this.bindings.All(binding => binding.IsInitialized);
            if (bindingsInitialized)
            {
                this.OnBindingValuesChanged();
            }
        }

        private void OnBindingValueChanged(object newValue)
        {
            this.OnBindingValuesChanged();
        }

        private void RegisterForValueChanges()
        {
            foreach (var binding in this.bindings)
            {
                binding.ValueChanged += this.OnBindingValueChanged;
            }
            this.isMonitoringBindings = true;
        }

        private void UnregisterFromValueChanges()
        {
            foreach (var binding in this.bindings)
            {
                binding.ValueChanged -= this.OnBindingValueChanged;
            }
            this.isMonitoringBindings = false;
        }

        #endregion
    }
}