// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColorObject.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Providers.Objects
{
    using Slash.Unity.DataBind.Core.Presentation;

    using UnityEngine;

    /// <summary>
    ///   Provides a plain color object.
    ///   <para>Output: Color.</para>
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Objects/[DB] Color Object")]
    public class ColorObject : DataProvider
    {
        /// <summary>
        ///   Color this provider holds.
        /// </summary>
        [Tooltip("Color this provider holds.")]
        public Color Color;

        /// <summary>
        ///   Current data value.
        /// </summary>
        public override object Value
        {
            get
            {
                return this.Color;
            }
        }

        /// <summary>
        ///   Called when the value of the data provider should be updated.
        /// </summary>
        protected override void UpdateValue()
        {
            this.OnValueChanged(this.Color);
        }
    }
}