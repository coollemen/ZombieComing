namespace Slash.Unity.DataBind.Foundation.Providers.Lookups
{
    using System.Linq;

    using Slash.Unity.DataBind.Core.Data;
    using Slash.Unity.DataBind.Core.Presentation;

    using UnityEngine;

    /// <summary>
    ///   Returns a part of a given collection.
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Lookups/[DB] Collection Range Lookup")]
    public class CollectionRangeLookup : DataProvider
    {
        #region Fields

        /// <summary>
        ///   Collection to get the items from.
        /// </summary>
        [Tooltip("Collection to get item from.")]
        public DataBinding Collection;

        /// <summary>
        ///   Index of the first item to get from the collection.
        /// </summary>
        [Tooltip("Index of the first item to get from the collection.")]
        public DataBinding FirstIndex;

        /// <summary>
        ///   Index of the last item to get from the collection.
        /// </summary>
        [Tooltip("Index of the last item to get from the collection.")]
        public DataBinding LastIndex;

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
                if (this.dataCollection == null)
                {
                    return null;
                }

                // Select value range.
                var collection = new Collection<object>();

                var first = this.FirstIndex.GetValue<int>();
                var last = this.LastIndex.GetValue<int>();

                foreach (var item in this.dataCollection.Cast<object>().Skip(first).Take(last - first + 1))
                {
                    collection.Add(item);
                }

                return collection;
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
            this.AddBinding(this.FirstIndex);
            this.AddBinding(this.LastIndex);
            this.AddBinding(this.Collection);
        }

        /// <summary>
        ///   Unity callback.
        /// </summary>
        protected void OnDestroy()
        {
            this.RemoveBinding(this.FirstIndex);
            this.RemoveBinding(this.LastIndex);
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
            this.OnDataCollectionChanged(null);
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