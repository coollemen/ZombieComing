namespace Slash.Unity.DataBind.Foundation.Providers.Formatters
{
    using Slash.Unity.DataBind.Core.Presentation;

    using UnityEngine;

    /// <summary>
    ///   Provides a fallback value if the specified data value is not set.
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Formatters/[DB] Fallback Value Formatter")]
    public class FallbackValueFormatter : DataProvider
    {
        #region Fields

        /// <summary>
        ///   Binding to get data from.
        /// </summary>
        public DataBinding Data;

        /// <summary>
        ///   Binding to get data value from if other binding doesn't provide a value.
        /// </summary>
        public DataBinding Fallback;

        #endregion

        #region Properties

        /// <summary>
        ///   Current data value.
        /// </summary>
        public override object Value
        {
            get
            {
                return this.Data.Value ?? this.Fallback.Value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///   Unity callback.
        /// </summary>
        protected void Awake()
        {
            this.AddBinding(this.Data);
            this.AddBinding(this.Fallback);
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