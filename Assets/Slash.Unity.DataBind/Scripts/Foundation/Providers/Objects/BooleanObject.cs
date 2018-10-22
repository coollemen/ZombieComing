// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BooleanObject.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Providers.Objects
{
    using Slash.Unity.DataBind.Core.Presentation;

    using UnityEngine;

    /// <summary>
    ///   Provides a plain boolean object.
    ///   <para>Output: Boolean.</para>
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Objects/[DB] Boolean Object")]
    public class BooleanObject : DataProvider
    {
        #region Properties

        /// <summary>
        ///   Current data value.
        /// </summary>
        public override object Value
        {
            get
            {
                return this.Boolean;
            }
        }

        #endregion

        #region Fields

        /// <summary>
        ///   Boolean this provider holds.
        /// </summary>
        [Tooltip("Boolean this provider holds.")]
        public bool Boolean;

        private bool currentBoolean;

        #endregion

        #region Methods

        /// <summary>
        ///   Unity callback.
        /// </summary>
        protected void Update()
        {
            if (this.Boolean != this.currentBoolean)
            {
                this.UpdateValue();
            }
        }

        /// <summary>
        ///   Called when the value of the data provider should be updated.
        /// </summary>
        protected override void UpdateValue()
        {
            this.currentBoolean = this.Boolean;
            this.OnValueChanged(this.currentBoolean);
        }

        #endregion
    }
}