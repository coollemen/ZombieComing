namespace Slash.Unity.DataBind.Foundation.Providers.Formatters
{
    using Slash.Unity.DataBind.Core.Presentation;

    using UnityEngine;

    /// <summary>
    ///   Formats a string by converting all letters to upper-case.
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Formatters/[DB] String ToUpper Formatter")]
    public class StringToUpperFormatter : DataProvider
    {
        #region Fields

        /// <summary>
        ///   String to convert.
        /// </summary>
        public DataBinding Argument;

        #endregion

        #region Properties

        /// <summary>
        ///   Current data value.
        /// </summary>
        public override object Value
        {
            get
            {
                if (this.Argument == null)
                {
                    return string.Empty;
                }

                var argument = this.Argument.GetValue<string>();
                return string.IsNullOrEmpty(argument) ? string.Empty : argument.ToUpper();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///   Unity callback.
        /// </summary>
        protected void Awake()
        {
            // Add bindings.
            this.AddBinding(this.Argument);
        }

        /// <summary>
        ///   Called when the value of the data provider should be updated.
        /// </summary>
        protected override void UpdateValue()
        {
            // TODO(co): Cache current value and check if really changed?
            this.OnValueChanged(this.Value);
        }

        #endregion
    }
}