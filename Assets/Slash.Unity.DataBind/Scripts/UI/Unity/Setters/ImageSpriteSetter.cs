// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ImageSpriteSetter.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.UI.Unity.Setters
{
    using Slash.Unity.DataBind.Foundation.Setters;

    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    ///   Setter which sets the sprite value of an image component.
    ///   <para>Input: <see cref="Sprite"/></para>
    /// </summary>
    [AddComponentMenu("Data Bind/UnityUI/Setters/[DB] Image Sprite Setter (Unity)")]
    public class ImageSpriteSetter : ComponentSingleSetter<Image, Sprite>
    {
        #region Methods

        /// <summary>
        ///   Called when the data binding value changed.
        /// </summary>
        /// <param name="newValue">New data value.</param>
        protected override void OnValueChanged(Sprite newValue)
        {
            this.Target.sprite = newValue;
        }

        #endregion
    }
}