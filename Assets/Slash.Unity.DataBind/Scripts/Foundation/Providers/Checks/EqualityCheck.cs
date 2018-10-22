// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EqualityCheck.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Providers.Checks
{
    using Slash.Unity.DataBind.Core.Presentation;
    using Slash.Unity.DataBind.Core.Utils;

    using UnityEngine;

    /// <summary>
    ///   Checks for equality of two data values.
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Checks/[DB] Equality Check")]
    public class EqualityCheck : DataProvider
    {
        #region Fields

        /// <summary>
        ///   First data value.
        /// </summary>
        public DataBinding First;

        /// <summary>
        ///   Second data value.
        /// </summary>
        public DataBinding Second;

        #endregion

        #region Properties

        /// <summary>
        ///   Current data value.
        /// </summary>
        public override object Value
        {
            get
            {
                var firstValue = this.First.Value;
                var secondValue = this.Second.Value;
                return ComparisonUtils.CheckValuesForEquality(firstValue, secondValue);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///   Unity callback.
        /// </summary>
        protected void Awake()
        {
            // Add bindings.
            this.AddBinding(this.First);
            this.AddBinding(this.Second);
        }

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