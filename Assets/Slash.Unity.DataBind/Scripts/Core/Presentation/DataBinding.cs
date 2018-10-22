// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataBinding.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Core.Presentation
{
    using System;

    using Slash.Unity.DataBind.Core.Utils;

    using UnityEngine;

    using Object = UnityEngine.Object;

    /// <summary>
    ///   Structure which hold the information to which data to bind.
    /// </summary>
    [Serializable]
    public sealed class DataBinding
    {
        #region Fields

        /// <summary>
        ///   Constant value.
        /// </summary>
        public string Constant;

        /// <summary>
        ///   Path to value in data context.
        /// </summary>
        [ContextPath(Filter = ~ContextMemberFilter.Methods)]
        public string Path;

        /// <summary>
        ///   Referenced data provider.
        /// </summary>
        public DataProvider Provider;

        /// <summary>
        ///   Unity object reference.
        /// </summary>
        public Object Reference;

        /// <summary>
        ///   Type of data binding.
        /// </summary>
        public DataBindingType Type;

        /// <summary>
        ///   Node to get the data from a context.
        /// </summary>
        private ContextNode contextNode;

        /// <summary>
        ///   Current data value.
        /// </summary>
        private object value;

        #endregion

        #region Delegates

        /// <summary>
        ///   Delegate for ValueChanged event.
        /// </summary>
        /// <param name="newValue">New data value.</param>
        public delegate void ValueChangedDelegate(object newValue);

        #endregion

        #region Events

        /// <summary>
        ///   Called when the data value changed.
        /// </summary>
        public event ValueChangedDelegate ValueChanged;

        #endregion

        #region Properties

        /// <summary>
        ///   Indicates if the data binding already holds a valid value.
        /// </summary>
        public bool IsInitialized { get; private set; }

        /// <summary>
        ///   Current data value.
        /// </summary>
        public object Value
        {
            get
            {
                return this.value;
            }
            private set
            {
                if (Equals(value, this.value))
                {
                    return;
                }

                this.value = value;
                this.OnValueChanged(this.value);
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Deinitializes the data binding, e.g. unregistering from events.
        /// </summary>
        public void Deinit()
        {
            if (this.Provider != null)
            {
                this.Provider.ValueChanged -= this.OnTargetValueChanged;
            }
            if (this.contextNode != null)
            {
                this.contextNode.SetValueListener(null);
                this.contextNode = null;
            }
        }

        /// <summary>
        ///   Returns the current data value, converted to the specified type.
        /// </summary>
        /// <typeparam name="T">Desired type of data.</typeparam>
        /// <returns>Current data value, converted to the specified type.</returns>
        /// <exception cref="System.InvalidCastException">Thrown if the data value can't be cast to the specified type.</exception>
        public T GetValue<T>()
        {
            try
            {
                return (T)this.GetValue(typeof(T));
            }
            catch (InvalidCastException)
            {
                throw new InvalidCastException(
                    string.Format(
                        "Can't cast value '{0}' of binding '{1}' to type '{2}'",
                        this.Value,
                        this.Path,
                        typeof(T)));
            }
        }

        /// <summary>
        ///   Returns the current data value, converted to the specified type.
        /// </summary>
        /// <param name="type">Desired type of data.</param>
        /// <returns>Current data value, converted to the specified type.</returns>
        /// <exception cref="System.InvalidCastException">Thrown if the data value can't be cast to the specified type.</exception>
        public object GetValue(Type type)
        {
            var rawValue = this.Value;
            if (rawValue == null)
            {
                return type.IsValueType ? Activator.CreateInstance(type) : null;
            }

            object convertedValue;
            return ReflectionUtils.TryConvertValue(rawValue, type, out convertedValue) ? convertedValue : rawValue;
        }

        /// <summary>
        ///   Initializes the data binding, depending on the type of data binding.
        /// </summary>
        /// <param name="gameObject">Game object this data binding belongs to.</param>
        public void Init(GameObject gameObject)
        {
            switch (this.Type)
            {
                case DataBindingType.Context:
                {
                    this.contextNode = new ContextNode(gameObject, this.Path);
                    var initialValue = this.contextNode.SetValueListener(this.OnTargetValueChanged);
                    this.OnTargetValueChanged(initialValue);
                }
                    break;
                case DataBindingType.Provider:
                {
                    if (this.Provider != null)
                    {
                        this.Provider.ValueChanged += this.OnTargetValueChanged;
                        if (this.Provider.IsInitialized)
                        {
                            this.OnTargetValueChanged(this.Provider.Value);
                        }
                    }
                    else
                    {
                        this.OnTargetValueChanged(null);
                    }
                }
                    break;
                case DataBindingType.Constant:
                {
                    this.OnTargetValueChanged(this.Constant);
                }
                    break;
                case DataBindingType.Reference:
                {
                    this.OnTargetValueChanged(this.Reference == null ? null : this.Reference);
                }
                    break;
            }
        }

        /// <summary>
        ///   Has to be called when an anchestor context changed as the data value may change.
        /// </summary>
        public void OnContextChanged()
        {
            if (this.contextNode != null)
            {
                this.contextNode.OnHierarchyChanged();
            }
        }

        #endregion

        #region Methods

        private void OnTargetValueChanged(object newValue)
        {
            if (this.IsInitialized)
            {
                // Only trigger callback if value changed.
                this.Value = newValue;
            }
            else
            {
                this.IsInitialized = true;

                // On initialization make sure the callback is triggered.
                this.value = newValue;
                this.OnValueChanged(this.value);
            }
        }

        private void OnValueChanged(object newValue)
        {
            var handler = this.ValueChanged;
            if (handler != null)
            {
                handler(newValue);
            }
        }

        #endregion
    }
}