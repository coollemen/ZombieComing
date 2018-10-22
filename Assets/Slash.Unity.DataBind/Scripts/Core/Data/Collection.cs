// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Collection.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Core.Data
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    ///   Collection with events to monitor if an item was added/removed.
    /// </summary>
    public abstract class Collection : IEnumerable, IDataProvider
    {
        #region Fields

        private readonly Property<int> countProperty = new Property<int>();

        #endregion

        #region Delegates

        /// <summary>
        ///   Delegate for Cleared event.
        /// </summary>
        public delegate void ClearedDelegate();

        /// <summary>
        ///   Delegate for ClearedItems event.
        /// </summary>
        /// <param name="items">Removed items.</param>
        public delegate void ClearedItemsDelegate(IEnumerable<object> items);

        /// <summary>
        ///   Delegate for ItemAdded event.
        /// </summary>
        /// <param name="item">Item which was added.</param>
        public delegate void ItemAddedDelegate(object item);

        /// <summary>
        ///   Delegate for ItemRemoved event.
        /// </summary>
        /// <param name="item">Item which was removed.</param>
        public delegate void ItemRemovedDelegate(object item);

        #endregion

        #region Events

        /// <summary>
        ///   Called when an item was added.
        /// </summary>
        public event ItemAddedDelegate ItemAdded;

        /// <summary>
        ///   Called when an item was removed.
        /// </summary>
        public event ItemRemovedDelegate ItemRemoved;

        /// <summary>
        ///   Called when the collection was cleared.
        /// </summary>
        public event ClearedDelegate Cleared;

        /// <summary>
        ///   Called when the collection was cleared.
        /// </summary>
        public event ClearedItemsDelegate ClearedItems;

        /// <summary>
        ///   Called when the collection changed.
        /// </summary>
        public event ValueChangedDelegate ValueChanged;

        #endregion

        #region Properties

        /// <summary>
        ///   Number of items in the collection.
        /// </summary>
        public int Count
        {
            get
            {
                return this.countProperty.Value;
            }
            protected set
            {
                this.countProperty.Value = value;
            }
        }

        /// <summary>
        ///   Current data value.
        /// </summary>
        public abstract object Value { get; }

        /// <summary>
        ///   Type of items in this collection.
        /// </summary>
        public abstract Type ItemType { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Adds a new item to the collection.
        /// </summary>
        /// <returns>New item.</returns>
        public abstract object AddNewItem();

        /// <summary>
        ///   Returns the enumerator of the collection.
        /// </summary>
        /// <returns>Enumerator of the collection.</returns>
        public abstract IEnumerator GetEnumerator();

        /// <summary>
        ///   Removes the specified item from the collection.
        /// </summary>
        /// <param name="item">Item to remove.</param>
        /// <returns>True if the item was removed; false, if it didn't exist in the collection.</returns>
        public abstract bool Remove(object item);

        #endregion

        #region Methods

        /// <summary>
        ///   Called when the collection was cleared.
        /// </summary>
        /// <param name="items">Removed items.</param>
        protected virtual void OnClearedItems(IEnumerable<object> items)
        {
            var handler = this.ClearedItems;
            if (handler != null)
            {
                handler(items);
            }
            this.OnCleared();
        }

        /// <summary>
        ///   Called when an item was added.
        /// </summary>
        /// <param name="item">Item which was added.</param>
        protected void OnItemAdded(object item)
        {
            var handler = this.ItemAdded;
            if (handler != null)
            {
                handler(item);
            }
            this.OnValueChanged();
        }

        /// <summary>
        ///   Called when an item was removed.
        /// </summary>
        /// <param name="item">Item which was removed.</param>
        protected void OnItemRemoved(object item)
        {
            var handler = this.ItemRemoved;
            if (handler != null)
            {
                handler(item);
            }
            this.OnValueChanged();
        }

        /// <summary>
        ///   Called when the collection changed.
        /// </summary>
        protected virtual void OnValueChanged()
        {
            var handler = this.ValueChanged;
            if (handler != null)
            {
                handler();
            }
        }

        /// <summary>
        ///   Called when the collection was cleared.
        /// </summary>
        private void OnCleared()
        {
            var handler = this.Cleared;
            if (handler != null)
            {
                handler();
            }
            this.OnValueChanged();
        }

        #endregion
    }

    /// <summary>
    ///   Generic collection with events to monitor when an item was added/removed.
    /// </summary>
    /// <typeparam name="T">Type of items in the collection.</typeparam>
    public sealed class Collection<T> : Collection, ICollection<T>
    {
        #region Fields

        private readonly List<object> items = new List<object>();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Constructor.
        /// </summary>
        /// <param name="items">Items to initialize the collection with.</param>
        public Collection(IEnumerable<T> items)
        {
            this.items = new List<object>(items.Cast<object>());
            this.UpdateCount();
        }

        /// <summary>
        ///   Constructor.
        /// </summary>
        public Collection()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Indicates if the collection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        ///   Returns the item of the collection at the specified index.
        /// </summary>
        /// <param name="index">Index of item to return.</param>
        /// <returns>Item at specified index.</returns>
        public T this[int index]
        {
            get
            {
                return (T)this.items[index];
            }
        }

        /// <summary>
        ///   Current data value.
        /// </summary>
        public override object Value
        {
            get
            {
                return this.items;
            }
        }

        /// <summary>
        ///   Type of items in this collection.
        /// </summary>
        public override Type ItemType
        {
            get
            {
                return typeof(T);
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Adds the specified item to the collection.
        /// </summary>
        /// <param name="item">Item to add.</param>
        public void Add(T item)
        {
            this.items.Add(item);
            this.OnItemAdded(item);
            this.UpdateCount();
        }

        /// <summary>
        ///   Adds a new item to the collection.
        /// </summary>
        /// <returns>New item.</returns>
        public override object AddNewItem()
        {
            var newItem = Activator.CreateInstance<T>();
            this.Add(newItem);
            return newItem;
        }

        /// <summary>
        ///   Clears the collection.
        /// </summary>
        public void Clear()
        {
            if (this.items.Count == 0)
            {
                return;
            }
            var removedItems = new List<object>(this.items);
            this.items.Clear();
            this.OnClearedItems(removedItems);
            this.UpdateCount();
        }

        /// <summary>
        ///   Indicates if the collection contains the specified item.
        /// </summary>
        /// <param name="item">Item to check.</param>
        /// <returns>True if the specified item exists in the collection; otherwise, false.</returns>
        public bool Contains(T item)
        {
            return this.items.Contains(item);
        }

        /// <summary>
        ///   Copies the collection to the specified index in the specified array.
        /// </summary>
        /// <param name="array">Array to copy this collection to.</param>
        /// <param name="arrayIndex">Array index to start to put copies in the array.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            foreach (var item in this.items)
            {
                array[arrayIndex++] = (T)item;
            }
        }

        /// <summary>
        ///   Returns the enumerator of the collection.
        /// </summary>
        /// <returns>Enumerator of the collection.</returns>
        public override IEnumerator GetEnumerator()
        {
            return this.items.GetEnumerator();
        }

        /// <summary>
        ///   Removes the specified item from the collection.
        /// </summary>
        /// <param name="item">Item to remove.</param>
        /// <returns>True if the item was removed; false, if it didn't exist in the collection.</returns>
        public override bool Remove(object item)
        {
            if (!(item is T))
            {
                return false;
            }
            return this.Remove((T)item);
        }

        /// <summary>
        ///   Removes the specified item from the collection.
        /// </summary>
        /// <param name="item">Item to remove.</param>
        /// <returns>True if the item was removed; false, if it didn't exist in the collection.</returns>
        public bool Remove(T item)
        {
            if (!this.items.Remove(item))
            {
                return false;
            }

            this.OnItemRemoved(item);
            this.UpdateCount();

            return true;
        }
        #endregion

        #region Methods

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return this.items.Select(item => (T)item).GetEnumerator();
        }

        private void UpdateCount()
        {
            this.Count = this.items.Count;
        }

        #endregion
    }
}