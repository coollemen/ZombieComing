namespace Slash.Unity.DataBind.Foundation.Providers.Lookups
{
    using Slash.Unity.DataBind.Core.Data;
    using Slash.Unity.DataBind.Core.Presentation;

    using UnityEngine;

    /// <summary>
    ///   Looks up an item with a specific index from a given collection.
    /// </summary>
    public class CollectionLookup : DataProvider
    {
        #region Fields

        /// <summary>
        ///   Collection to get item from.
        /// </summary>
        [Tooltip("Collection to get item from.")]
        public DataBinding Collection;

        /// <summary>
        ///   Default value if index wasn't found in collection.
        /// </summary>
        [Tooltip("Default value if index wasn't found in collection.")]
        public string DefaultValue;

        /// <summary>
        ///   Index of item to get from collection.
        /// </summary>
        [Tooltip("Index of item to get from collection.")]
        public DataBinding Index;

        private Collection dataCollection;

        #endregion

        #region Properties

        /// <summary>
        ///   Current data value.
        /// </summary>
        public override object Value
        {
            get
            {
                object value = string.IsNullOrEmpty(this.DefaultValue) ? null : this.DefaultValue;
                if (this.DataCollection != null)
                {
                    var index = this.Index.GetValue<int>();
                    foreach (var dataValue in this.DataCollection)
                    {
                        if (index == 0)
                        {
                            value = dataValue;
                            break;
                        }
                        --index;
                    }
                }

                return value;
            }
        }

        private Collection DataCollection
        {
            get
            {
                return this.dataCollection;
            }
            set
            {
                if (value == this.dataCollection)
                {
                    return;
                }

                if (this.dataCollection != null)
                {
                    this.dataCollection.ItemAdded -= this.OnCollectionItemAdded;
                    this.dataCollection.ItemRemoved -= this.OnCollectionItemRemoved;
                    this.dataCollection.Cleared -= this.OnCollectionCleared;
                }

                this.dataCollection = value;

                if (this.dataCollection != null)
                {
                    this.dataCollection.ItemAdded += this.OnCollectionItemAdded;
                    this.dataCollection.ItemRemoved += this.OnCollectionItemRemoved;
                    this.dataCollection.Cleared += this.OnCollectionCleared;
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
            this.AddBinding(this.Index);
            this.AddBinding(this.Collection);
        }

        /// <summary>
        ///   Unity callback.
        /// </summary>
        protected void OnDestroy()
        {
            this.RemoveBinding(this.Index);
            this.RemoveBinding(this.Collection);
        }

        /// <summary>
        ///   Unity callback.
        /// </summary>
        protected override void OnDisable()
        {
            base.OnDisable();
            this.Collection.ValueChanged -= this.OnDataCollectionChanged;
        }

        /// <summary>
        ///   Unity callback.
        /// </summary>
        protected override void OnEnable()
        {
            base.OnEnable();

            this.Collection.ValueChanged += this.OnDataCollectionChanged;
            this.DataCollection = this.Collection.GetValue<Collection>();
        }

        /// <summary>
        ///   Called when the value of the data provider should be updated.
        /// </summary>
        protected override void UpdateValue()
        {
            this.OnValueChanged(this.Value);
        }

        private void OnCollectionCleared()
        {
            this.UpdateValue();
        }

        private void OnCollectionItemAdded(object item)
        {
            this.UpdateValue();
        }

        private void OnCollectionItemRemoved(object item)
        {
            this.UpdateValue();
        }

        private void OnDataCollectionChanged(object newValue)
        {
            this.DataCollection = this.Collection.GetValue<Collection>();
        }

        #endregion
    }
}