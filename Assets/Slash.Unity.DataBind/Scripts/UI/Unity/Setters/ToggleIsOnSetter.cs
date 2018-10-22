// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ToggleIsOnSetter.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.UI.Unity.Setters
{
    using Slash.Unity.DataBind.Foundation.Setters;

    using UnityEngine.UI;

    /// <summary>
    ///   Enables/Disables a toggle control depending on a boolean data value.
    ///   <para>
    ///     Input: <see cref="bool" />
    ///   </para>
    /// </summary>
    public class ToggleIsOnSetter : ComponentSingleSetter<Toggle, bool>
    {
        #region Methods

        /// <summary>
        ///   Called when the data binding value changed.
        /// </summary>
        /// <param name="newValue">New data value.</param>
        protected override void OnValueChanged(bool newValue)
        {
            this.Target.isOn = newValue;
        }

        #endregion
    }
}