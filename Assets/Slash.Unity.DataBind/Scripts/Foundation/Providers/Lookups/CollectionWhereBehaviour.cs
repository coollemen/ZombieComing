// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CollectionWhereBehaviour.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Providers.Lookups
{
    using Slash.Unity.DataBind.Core.Data;
    using Slash.Unity.DataBind.Core.Presentation;
    using Slash.Unity.DataBind.Core.Utils;

    /// <summary>
    ///   Returns the item of the collection that has a specific value at a specific path.
    /// </summary>
    public class CollectionWhereBehaviour : DataProvider
    {
        #region Fields

        /// <summary>
        ///   Collection to check items.
        /// </summary>
        public DataBinding Collection;

        /// <summary>
        ///   Value to compare item value with.
        /// </summary>
        public DataBinding ComparisonValue;

        /// <summary>
        ///   Path to value within item to compare to comparison value.
        /// </summary>
        [ContextPath(PathDisplayName = "Item Path")]
        public string ItemPath;

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
                var collection = this.Collection.GetValue<Collection>();
                if (collection == null)
                {
                    return null;
                }

                var comparisonValue = this.ComparisonValue.Value;
                foreach (var item in collection)
                {
                    var itemValue = item;
                    var itemContext = item as Context;
                    if (itemContext != null && !string.IsNullOrEmpty(this.ItemPath))
                    {
                        itemValue = itemContext.GetValue(this.ItemPath);
                    }

                    if (ComparisonUtils.CheckValuesForEquality(itemValue, comparisonValue))
                    {
                        return item;
                    }
                }
                return null;
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
            this.AddBinding(this.Collection);
            this.AddBinding(this.ComparisonValue);
        }

        /// <summary>
        ///   Unity callback.
        /// </summary>
        protected void OnDestroy()
        {
            this.RemoveBinding(this.Collection);
            this.RemoveBinding(this.ComparisonValue);
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