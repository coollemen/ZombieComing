// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextTextSetter.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.UI.Unity.Setters
{
    using Slash.Unity.DataBind.Foundation.Setters;

    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    ///   Set the text of a Text depending on the string data value.
    /// </summary>
    [AddComponentMenu("Data Bind/UnityUI/Setters/[DB] Text Text Setter (Unity)")]
    public class TextTextSetter : ComponentSingleSetter<Text, string>
    {
        #region Methods

        /// <summary>
        ///   Called when the data binding value changed.
        /// </summary>
        /// <param name="newValue">New data value.</param>
        protected override void OnValueChanged(string newValue)
        {
            if (this.Target != null)
            {
                this.Target.text = newValue;
            }
        }

        #endregion
    }
}