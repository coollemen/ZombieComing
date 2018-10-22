// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataConverter.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Providers.Converters
{
    using Slash.Unity.DataBind.Core.Presentation;

    /// <summary>
    ///   Converter which converts its provided value to a target type. 
    /// </summary>
    /// <typeparam name="TFrom">Expected type of data value.</typeparam>
    /// <typeparam name="TTo">Type to convert data value to.</typeparam>
    public abstract class DataConverter<TFrom, TTo> : DataProvider
    {
        #region Fields

        /// <summary>
        ///   Data value to use.
        /// </summary>
        public DataBinding Data;

        #endregion

        #region Properties

        /// <summary>
        ///   Current data value.
        /// </summary>
        public override object Value
        {
            get
            {
                var value = this.Data.GetValue<TFrom>();
                return this.Convert(value);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///   Unity callback.
        /// </summary>
        protected virtual void Awake()
        {
            this.AddBinding(this.Data);
        }

        /// <summary>
        ///   Called when the specified value should be converted.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <returns>Converted value.</returns>
        protected abstract TTo Convert(TFrom value);

        /// <summary>
        ///   Called when the value of the data provider should be updated.
        /// </summary>
        protected override void UpdateValue()
        {
            this.OnValueChanged(this.Value);
        }

        #endregion
    }
}