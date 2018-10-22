// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MaterialInstanceFormatter.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.UI.Unity.Providers
{
    using Slash.Unity.DataBind.Core.Presentation;

    using UnityEngine;

    /// <summary>
    ///   Creates a new material instance from the specified material.
    /// </summary>
    [AddComponentMenu("Data Bind/Unity/Formatters/[DB] Material Instance Formatter")]
    public class MaterialInstanceFormatter : DataProvider
    {
        /// <summary>
        ///   Material to instantiate.
        /// </summary>
        public DataBinding Material;

        /// <summary>
        ///   Current data value.
        /// </summary>
        public override object Value
        {
            get
            {
                var material = this.Material.GetValue<Material>();
                return material != null ? new Material(material) : null;
            }
        }
        
        /// <summary>
        ///   Unity callback.
        /// </summary>
        protected void Awake()
        {
            // Add bindings.
            this.AddBinding(this.Material);
        }

        /// <summary>
        ///   Called when the value of the data provider should be updated.
        /// </summary>
        protected override void UpdateValue()
        {
            this.OnValueChanged(this.Value);
        }
    }
}