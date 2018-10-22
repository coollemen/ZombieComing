// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IsSetCheck.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Providers.Checks
{
    using Slash.Unity.DataBind.Core.Presentation;

    using UnityEngine;

    /// <summary>
    ///   Indicates if the data value is set, i.e. not null.
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Checks/[DB] Is Set Check")]
    public class IsSetCheck : DataProvider
    {
        #region Fields

        /// <summary>
        ///   Data to bind to.
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
                return this.Data.Value != null;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///   Unity callback.
        /// </summary>
        protected void Awake()
        {
            this.AddBinding(this.Data);
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