// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransformRotationProvider.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Providers.Getters
{
    using UnityEngine;

    /// <summary>
    ///   Provides the rotation of the target transform.
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Getters/[DB] Transform Rotation Provider")]
    public class TransformRotationProvider : ComponentDataProvider<Transform, Quaternion>
    {
        private Quaternion currentRotation;

        /// <summary>
        ///   Register listener at target to be informed if its value changed.
        ///   The target is already checked for null reference.
        /// </summary>
        /// <param name="target">Target to add listener to.</param>
        protected override void AddListener(Transform target)
        {
        }

        /// <summary>
        ///   Derived classes should return the current value to set if this method is called.
        ///   The target is already checked for null reference.
        /// </summary>
        /// <param name="target">Target to get value from.</param>
        /// <returns>Current value to set.</returns>
        protected override Quaternion GetValue(Transform target)
        {
            return target.rotation;
        }

        /// <summary>
        ///   Remove listener from target which was previously added in AddListener.
        ///   The target is already checked for null reference.
        /// </summary>
        /// <param name="target">Target to remove listener from.</param>
        protected override void RemoveListener(Transform target)
        {
        }

        /// <summary>
        ///   Unity callback.
        /// </summary>
        protected void Update()
        {
            var target = this.Target;
            if (target == null)
            {
                return;
            }

            var newRotation = target.rotation;
            if (newRotation != this.currentRotation)
            {
                this.OnValueChanged(newRotation);
                this.currentRotation = newRotation;
            }
        }
    }
}