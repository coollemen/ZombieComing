// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnimatorTriggerSetter.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Setters
{
    using UnityEngine;

    /// <summary>
    ///   Sets the animator paramater of a game object to the boolean data value.
    ///   <para>Input: Boolean</para>
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Setters/[DB] Animator Trigger Setter")]
    public class AnimatorTriggerSetter : ComponentSingleSetter<Animator, bool>
    {
        /// <summary>
        ///   TypeID of the animator parameter.
        /// </summary>
        [Tooltip("TypeID of an animator parameter.")]
        public string AnimatorParameterName;

        /// <summary>
        ///   Called when the data binding value changed.
        /// </summary>
        /// <param name="newValue">New data value.</param>
        protected override void OnValueChanged(bool newValue)
        {
            if (!this.Target.isInitialized)
            {
                // Ignore trigger if animator is not ready yet.
                return;
            }

            if (newValue)
            {
                this.Target.SetTrigger(this.AnimatorParameterName);
            }
            else
            {
                this.Target.ResetTrigger(this.AnimatorParameterName);
            }
        }
    }
}