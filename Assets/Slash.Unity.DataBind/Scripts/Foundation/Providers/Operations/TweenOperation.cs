// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TweenOperation.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Providers.Operations
{
    using System;

    using Slash.Unity.DataBind.Core.Presentation;

    using UnityEngine;

    /// <summary>
    ///   Provides a float value that smoothly changes over time till it reaches the target value.
    ///   <para>Input: Number</para>
    ///   <para>Output: Number</para>
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Operations/[DB] Tween Operation")]
    public class TweenOperation : DataProvider
    {
        #region Properties

        /// <summary>
        ///   Current data value.
        /// </summary>
        public override object Value
        {
            get
            {
                return this.currentValue;
            }
        }

        #endregion

        #region Fields

        /// <summary>
        ///   How much the value should change per second.
        /// </summary>
        [Tooltip("How much the value should change per second.")]
        public float ChangePerSecond = 1.0f;

        /// <summary>
        ///   Target value of tween.
        /// </summary>
        [Tooltip("Target value of tween.")]
        public DataBinding TargetValue;

        private float currentValue;

        private float targetValue;

        #endregion

        #region Methods

        /// <summary>
        ///   Unity callback.
        /// </summary>
        protected void Awake()
        {
            this.AddBinding(this.TargetValue);
        }

        /// <summary>
        ///   Unity callback.
        /// </summary>
        protected void OnDestroy()
        {
            this.RemoveBinding(this.TargetValue);
        }

        /// <summary>
        ///   Unity callback.
        /// </summary>
        protected void Update()
        {
            var difference = this.targetValue - this.currentValue;
            if (Math.Abs(difference) > 0.001f)
            {
                var maxDifference = Time.deltaTime * this.ChangePerSecond;
                var appliedChange = Mathf.Clamp(Mathf.Abs(difference), 0, maxDifference);

                this.currentValue += Mathf.Sign(difference) * appliedChange;
                this.OnValueChanged(this.currentValue);
            }
        }

        /// <summary>
        ///   Called when the value of the data provider should be updated.
        /// </summary>
        protected override void UpdateValue()
        {
            this.targetValue = this.TargetValue.GetValue<float>();
        }

        #endregion
    }
}