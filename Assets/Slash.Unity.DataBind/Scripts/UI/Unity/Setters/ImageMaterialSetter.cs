// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ImageMaterialSetter.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.UI.Unity.Setters
{
    using Slash.Unity.DataBind.Foundation.Setters;

    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    ///   Sets the material for an image.
    ///   <para>Input: Material</para>
    /// </summary>
    [AddComponentMenu("Data Bind/UnityUI/Setters/[DB] Image Material Setter (Unity)")]
    public class ImageMaterialSetter : ComponentSingleSetter<Image, Material>
    {
        #region Methods

        /// <summary>
        ///   Called when the data binding value changed.
        /// </summary>
        /// <param name="newValue">New data value.</param>
        protected override void OnValueChanged(Material newValue)
        {
            this.Target.material = newValue;
        }

        #endregion
    }
}