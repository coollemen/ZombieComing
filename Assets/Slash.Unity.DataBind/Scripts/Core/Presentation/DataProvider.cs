// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataProvider.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Core.Presentation
{
    /// <summary>
    ///   Base class for a data provider.
    /// </summary>
    public abstract class DataProvider : DataBindingOperator
    {
        #region Delegates

        /// <summary>
        ///   Delegate for ValueChanged event.
        /// </summary>
        /// <param name="newValue">New data value.</param>
        public delegate void ValueChangedDelegate(object newValue);

        #endregion

        #region Events

        /// <summary>
        ///   Triggered when the value changed.
        /// </summary>
        public event ValueChangedDelegate ValueChanged;

        #endregion

        #region Properties

        /// <summary>
        ///   Current data value.
        /// </summary>
        public abstract object Value { get; }

        #endregion

        #region Methods

        /// <summary>
        ///   Called when a value of the bindings of this operator changed.
        /// </summary>
        protected override void OnBindingValuesChanged()
        {
            this.UpdateValue();
        }

        /// <summary>
        ///   Should be called by a derived class if the value of the data provider changed.
        /// </summary>
        /// <param name="newValue">New value of this data provider.</param>
        protected void OnValueChanged(object newValue)
        {
            var handler = this.ValueChanged;
            if (handler != null)
            {
                handler(newValue);
            }
        }

        /// <summary>
        ///   Called when the value of the data provider should be updated.
        /// </summary>
        protected abstract void UpdateValue();

        #endregion
    }
}