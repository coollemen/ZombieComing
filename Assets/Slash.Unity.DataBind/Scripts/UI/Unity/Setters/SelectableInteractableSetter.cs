// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SelectableInteractableSetter.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.UI.Unity.Setters
{
    using Slash.Unity.DataBind.Foundation.Setters;

    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    ///   Sets if a selectable is interactable depending on a boolean data value.
    ///   <para>Input: Boolean</para>
    /// </summary>
    [AddComponentMenu("Data Bind/UnityUI/Setters/[DB] Selectable Interactable Setter (Unity)")]
    public class SelectableInteractableSetter : ComponentSingleSetter<Selectable, bool>
    {
        #region Methods

        /// <summary>
        ///   Called when the data binding value changed.
        /// </summary>
        /// <param name="newValue">New data value.</param>
        protected override void OnValueChanged(bool newValue)
        {
            this.Target.interactable = newValue;
        }

        #endregion
    }
}