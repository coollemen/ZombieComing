// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LongSmoothener.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Providers.Smootheners
{
    using System;

    using Slash.Unity.DataBind.Core.Presentation;

    using UnityEngine;

    /// <summary>
    ///   Formats arguments by a specified format string to create a new string value.
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Smootheners/[DB] Long Smoothener")]
    public class LongSmoothener : DataProvider
    {
        /// <summary>
        ///   Binding to get target value from.
        /// </summary>
        public DataBinding Data;

        /// <summary>
        ///   If set to smaller as 0, it is ignored. If set to 0 values are instant updated.
        /// </summary>
        [Tooltip("If set to smaller as 0, it is ignored. If set to 0 values are instant updated.")]
        public float MaxUpdateTime = -1f;

        /// <summary>
        ///   Minimal step to take per update of the value.
        /// </summary>
        [Tooltip("Minimal step to take per update of the value.")]
        public long MinStep = 1;

        private long actualValue;

        private long targetValue;

        private long updateIntervalValue;

        /// <summary>
        ///   Current data value.
        /// </summary>
        public override object Value
        {
            get
            {
                return this.actualValue;
            }
        }

        /// <summary>
        ///   Unity callback.
        /// </summary>
        protected void Awake()
        {
            // Add bindings.
            this.AddBinding(this.Data);
            if (this.Data.Value is long)
            {
                this.actualValue = this.targetValue = (long)this.Data.Value;
            }
            this.Data.ValueChanged += this.OnDataValueChanged;
        }

        /// <summary>
        ///   Unity callback.
        /// </summary>
        protected void Update()
        {
            if (this.actualValue != this.targetValue)
            {
                if (this.updateIntervalValue > Math.Abs(this.actualValue - this.targetValue))
                {
                    this.updateIntervalValue = Math.Abs(this.actualValue - this.targetValue);
                }

                this.actualValue = this.actualValue < this.targetValue
                    ? this.actualValue + this.updateIntervalValue
                    : this.actualValue - this.updateIntervalValue;

                this.OnValueChanged(this.actualValue);
            }
        }

        /// <summary>
        ///   Called when the value of the data provider should be updated.
        /// </summary>
        protected override void UpdateValue()
        {
            // TODO(co): Cache current value and check if really changed?
            this.OnValueChanged(this.Value);
        }

        private void OnDataValueChanged(object newValue)
        {
            // TODO: check for different types like int, float
            if (newValue is long)
            {
                this.targetValue = (long)newValue;

                if (this.MaxUpdateTime == 0)
                {
                    this.actualValue = this.targetValue;
                }
                else if (this.MaxUpdateTime < 0)
                {
                    this.updateIntervalValue = this.MinStep;
                }
                else
                {
                    float updateCirclesInMaxUpdateTime = this.MaxUpdateTime / Time.deltaTime;
                    long difference = Math.Abs(this.actualValue - this.targetValue);

                    this.updateIntervalValue = updateCirclesInMaxUpdateTime * this.MinStep >= difference
                        ? this.MinStep
                        : (long)Mathf.Ceil(difference / updateCirclesInMaxUpdateTime);
                }
            }
        }
    }
}