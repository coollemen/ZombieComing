// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DictionaryLookup.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Providers.Lookups
{
    using Slash.Unity.DataBind.Core.Data;
    using Slash.Unity.DataBind.Core.Presentation;
    using Slash.Unity.DataBind.Core.Utils;

    /// <summary>
    ///   Looks up a value from a data dictionary by its key.
    /// </summary>
    public class DictionaryLookup : DataProvider
    {
        #region Fields

        /// <summary>
        ///   Default value if key wasn't found in dictionary.
        /// </summary>
        public string DefaultValue;

        /// <summary>
        ///   Dictionary to get value from.
        /// </summary>
        public DataBinding Dictionary;

        /// <summary>
        ///   Key to get value for.
        /// </summary>
        public DataBinding Key;

        private DataDictionary dataDictionary;

        #endregion

        #region Properties

        /// <summary>
        ///   Current data value.
        /// </summary>
        public override object Value
        {
            get
            {
                if (this.DataDictionary == null)
                {
                    return string.IsNullOrEmpty(this.DefaultValue) ? null : this.DefaultValue;
                }

                var key = this.Key.GetValue(this.DataDictionary.KeyType);

                object value;
                if (!this.DataDictionary.TryGetValue(key, out value))
                {
                    ReflectionUtils.TryConvertValue(this.DefaultValue, this.DataDictionary.ValueType, out value);
                }

                return value;
            }
        }

        private DataDictionary DataDictionary
        {
            get
            {
                return this.dataDictionary;
            }
            set
            {
                if (value == this.dataDictionary)
                {
                    return;
                }

                if (this.dataDictionary != null)
                {
                    this.dataDictionary.CollectionChanged -= this.OnDataDictionaryChanged;
                }

                this.dataDictionary = value;

                if (this.dataDictionary != null)
                {
                    this.dataDictionary.CollectionChanged += this.OnDataDictionaryChanged;
                }

                this.UpdateValue();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///   Unity callback.
        /// </summary>
        protected void Awake()
        {
            this.AddBinding(this.Key);
            this.AddBinding(this.Dictionary);
        }

        /// <summary>
        ///   Unity callback.
        /// </summary>
        protected void OnDestroy()
        {
            this.RemoveBinding(this.Key);
            this.RemoveBinding(this.Dictionary);
        }

        /// <summary>
        ///   Unity callback.
        /// </summary>
        protected override void OnDisable()
        {
            base.OnDisable();
            this.Dictionary.ValueChanged -= this.OnDictionaryChanged;
        }

        /// <summary>
        ///   Unity callback.
        /// </summary>
        protected override void OnEnable()
        {
            base.OnEnable();
            this.Dictionary.ValueChanged += this.OnDictionaryChanged;
            this.DataDictionary = this.Dictionary.GetValue<DataDictionary>();
        }

        /// <summary>
        ///   Called when the value of the data provider should be updated.
        /// </summary>
        protected override void UpdateValue()
        {
            this.OnValueChanged(this.Value);
        }

        private void OnDataDictionaryChanged()
        {
            this.UpdateValue();
        }

        private void OnDictionaryChanged(object newValue)
        {
            this.DataDictionary = this.Dictionary.GetValue<DataDictionary>();
        }

        #endregion
    }
}