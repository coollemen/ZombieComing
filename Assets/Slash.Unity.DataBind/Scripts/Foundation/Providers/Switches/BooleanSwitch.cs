// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BooleanSwitch.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Providers.Switches
{
    using Slash.Unity.DataBind.Core.Presentation;

    using UnityEngine;

    /// <summary>
    ///   Data provider which chooses one of two options depending on a provided boolean value.
    ///   <para>Input: Boolean (Switch).</para>
    ///   <para>Output: Object (Chosen data).</para>
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Switches/[DB] Boolean Switch")]
    public class BooleanSwitch : DataProvider
    {
        #region Properties

        /// <summary>
        ///   Current data value.
        /// </summary>
        public override object Value
        {
            get
            {
                // Get value of switch.
                var switchValue = this.Switch.GetValue<bool>();
                return switchValue ? this.OptionTrue.Value : this.OptionFalse.Value;
            }
        }

        #endregion

        #region Fields

        /// <summary>
        ///   Data to use if switch is false.
        /// </summary>
        [Tooltip("Data to use if switch is false.")]
        public DataBinding OptionFalse;

        /// <summary>
        ///   Data to use if switch is true.
        /// </summary>
        [Tooltip("Data to use if switch is true.")]
        public DataBinding OptionTrue;

        /// <summary>
        ///   Switch to decide which option to use.
        /// </summary>
        [Tooltip("Switch to decide which option to use.")]
        public DataBinding Switch;

        #endregion

        #region Methods

        /// <summary>
        ///   Unity callback.
        /// </summary>
        protected void Awake()
        {
            this.AddBinding(this.Switch);
            this.AddBinding(this.OptionTrue);
            this.AddBinding(this.OptionFalse);
        }

        /// <summary>
        ///   Unity callback.
        /// </summary>
        protected void OnDestroy()
        {
            this.RemoveBinding(this.Switch);
            this.RemoveBinding(this.OptionTrue);
            this.RemoveBinding(this.OptionFalse);
        }

        /// <summary>
        ///   Called when the value of the data provider should be updated.
        /// </summary>
        protected override void UpdateValue()
        {
            this.OnValueChanged(this.Value);
        }

        #endregion
    }
}