// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PositionSetter.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Setters
{
    using UnityEngine;

    /// <summary>
    ///   Sets the position of a transform depending on a Vector3 data value.
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Setters/[DB] Transform Position Setter")]
    public class TransformPositionSetter : ComponentSingleSetter<Transform, Vector3>
    {
        /// <summary>
        ///   Called when the data binding value changed.
        /// </summary>
        /// <param name="newValue">New data value.</param>
        protected override void OnValueChanged(Vector3 newValue)
        {
            this.Target.position = newValue;
        }
    }
}