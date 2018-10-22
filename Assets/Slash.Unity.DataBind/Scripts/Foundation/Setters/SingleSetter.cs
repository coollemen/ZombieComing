// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SingleSetter.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Setters
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using Slash.Unity.DataBind.Core.Presentation;

    using UnityEngine;

    /// <summary>
    ///   Base class for a setter of a single data value.
    /// </summary>
    public abstract class SingleSetter : DataBindingOperator
    {
        #region Fields

        /// <summary>
        ///   Data to bind to.
        /// </summary>
        public DataBinding Data;

        #endregion

        #region Methods

        /// <summary>
        ///   Unity callback.
        /// </summary>
        [SuppressMessage("ReSharper", "UnusedMemberHiearchy.Global")]
        protected virtual void Awake()
        {
            this.AddBinding(this.Data);
        }

        /// <summary>
        ///   Unity callback.
        /// </summary>
        [SuppressMessage("ReSharper", "UnusedMemberHiearchy.Global")]
        protected virtual void OnDestroy()
        {
            this.RemoveBinding(this.Data);
        }

        /// <summary>
        ///   Unity callback.
        /// </summary>
        [SuppressMessage("ReSharper", "VirtualMemberNeverOverriden.Global")]
        protected override void OnDisable()
        {
            base.OnDisable();

            this.Data.ValueChanged -= this.OnObjectValueChanged;
        }

        /// <summary>
        ///   Unity callback.
        /// </summary>
        [SuppressMessage("ReSharper", "VirtualMemberNeverOverriden.Global")]
        protected override void OnEnable()
        {
            base.OnEnable();

            this.Data.ValueChanged += this.OnObjectValueChanged;
            if (this.Data.IsInitialized)
            {
                this.OnObjectValueChanged(this.Data.Value);
            }
        }

        /// <summary>
        ///   Called when the data binding value changed.
        /// </summary>
        /// <param name="newValue">New data value.</param>
        protected virtual void OnObjectValueChanged(object newValue)
        {
        }

        #endregion
    }

    /// <summary>
    ///   Generic base class for a single data setter of a specific type.
    /// </summary>
    /// <typeparam name="T">Type of data to set.</typeparam>
    public abstract class SingleSetter<T> : SingleSetter
    {
        #region Methods

        /// <summary>
        ///   Called when the data binding value changed.
        /// </summary>
        /// <param name="newValue">New data value.</param>
        protected override void OnObjectValueChanged(object newValue)
        {
            T value;
            if (newValue is T)
            {
                value = (T)newValue;
            }
            else
            {
                try
                {
                    value = this.Data.GetValue<T>();
                }
                catch (Exception e)
                {
                    Debug.LogWarning(
                        string.Format(
                            "Couldn't convert new value '{0}' to type '{1}', using default value: {2}",
                            newValue,
                            typeof(T),
                            e.Message),
                        this);
                    value = default(T);
                }
            }
            this.OnValueChanged(value);
        }

        /// <summary>
        ///   Called when the data binding value changed.
        /// </summary>
        /// <param name="newValue">New data value.</param>
        protected abstract void OnValueChanged(T newValue);

        #endregion
    }
}