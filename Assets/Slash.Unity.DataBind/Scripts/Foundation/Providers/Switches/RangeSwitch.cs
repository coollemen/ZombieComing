namespace Slash.Unity.DataBind.Foundation.Providers.Switches
{
    using Slash.Unity.DataBind.Core.Presentation;

    /// <summary>
    ///   Base class for data providers which return different values depending on specified value ranges.
    /// </summary>
    /// <typeparam name="T">Type of ranges to check.</typeparam>
    public abstract class RangeSwitch<T> : DataProvider
        where T : SwitchOption
    {
        #region Fields

        /// <summary>
        ///   Data value to use as switch.
        /// </summary>
        public DataBinding Switch;

        /// <summary>
        ///   Default data value to use if no option is valid.
        /// </summary>
        public DataBinding Default;

        #endregion

        #region Properties

        /// <summary>
        ///   Current data value.
        /// </summary>
        public override object Value
        {
            get
            {
                var value = this.Switch.Value;
                var option = this.SelectOption(value);
                if (option == null)
                {
                    return this.Default.Value;
                }
                
                // Init if not done.
                if (!option.Value.IsInitialized)
                {
                    option.Value.Init(this.gameObject);
                }

                return option.Value.Value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///   Unity callback.
        /// </summary>
        protected void Awake()
        {
            this.AddBinding(this.Switch);
            this.AddBinding(this.Default);
        }

        /// <summary>
        ///   Unity callback.
        /// </summary>
        protected void OnDestroy()
        {
            this.RemoveBinding(this.Switch);
            this.RemoveBinding(this.Default);
        }

        /// <summary>
        ///   Selects the option to use for the specified value.
        /// </summary>
        /// <param name="value">Value to get option for.</param>
        /// <returns>Option to use for the specified value.</returns>
        protected abstract SwitchOption SelectOption(object value);

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