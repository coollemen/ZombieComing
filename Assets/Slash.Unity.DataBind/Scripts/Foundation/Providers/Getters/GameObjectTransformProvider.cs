// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameObjectTransformProvider.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Providers.Getters
{
    using Slash.Unity.DataBind.Core.Presentation;

    using UnityEngine;

    /// <summary>
    ///   Provides the transform of the target game object.
    /// </summary>
    public class GameObjectTransformProvider : DataProvider
    {
        /// <summary>
        ///   Game object to get transform from.
        /// </summary>
        public DataBinding Target;

        /// <summary>
        ///   Current data value.
        /// </summary>
        public override object Value
        {
            get
            {
                var targetGameObject = this.Target.GetValue<GameObject>();
                return targetGameObject != null ? targetGameObject.transform : null;
            }
        }

        /// <summary>
        ///   Unity callback.
        /// </summary>
        protected void Awake()
        {
            this.AddBinding(this.Target);
        }

        /// <summary>
        ///   Unity callback.
        /// </summary>
        protected void OnDestroy()
        {
            this.RemoveBinding(this.Target);
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