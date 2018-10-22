// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ActiveSetter.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Setters
{
    using UnityEngine;

    /// <summary>
    ///   Setter which activates/deactivates a game object depending on the boolean data value.
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Setters/[DB] Active Setter")]
    public class ActiveSetter : GameObjectSingleSetter<bool>
    {
        #region Methods

        /// <summary>
        ///   Called when the data binding value changed.
        /// </summary>
        /// <param name="newValue">New data value.</param>
        protected override void OnValueChanged(bool newValue)
        {
            this.Target.SetActive(newValue);
        }

        #endregion
    }
}