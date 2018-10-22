// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComparisonCheck.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Providers.Checks
{
    using System;

    using Slash.Unity.DataBind.Core.Presentation;

    using UnityEngine;

    /// <summary>
    ///   Check to compare two comparable data values.
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Checks/[DB] Comparison Check")]
    public class ComparisonCheck : DataProvider
    {
        /// <summary>
        ///   How to compare the data values.
        /// </summary>
        public enum ComparisonType
        {
            /// <summary>
            ///   Checks if the first value is less than the second one.
            /// </summary>
            LessThan,

            /// <summary>
            ///   Checks if the first value is equal to the second one.
            /// </summary>
            Equal,

            /// <summary>
            ///   Checks if the first value is greater than the second one.
            /// </summary>
            GreaterThan
        }

        #region Fields

        /// <summary>
        ///   How to compare the data values.
        /// </summary>
        public ComparisonType Comparison;

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
                // Assume first type is known, e.g. context property.
                var first = (IComparable)this.First.Value;
                if (first == null)
                {
                    return false;
                }

                // Convert second argument to type of first.
                var second = this.Second.GetValue(first.GetType());

                // Compare values.
                var newValue = false;
                switch (this.Comparison)
                {
                    case ComparisonType.Equal:
                        newValue = first.CompareTo(second) == 0;
                        break;

                    case ComparisonType.GreaterThan:
                        newValue = first.CompareTo(second) > 0;
                        break;

                    case ComparisonType.LessThan:
                        newValue = first.CompareTo(second) < 0;
                        break;
                }

                return newValue;
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