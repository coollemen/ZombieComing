﻿namespace Slash.Unity.DataBind.Foundation.Providers.Formatters
{
    using System.Collections.Generic;

    using Slash.Unity.DataBind.Core.Data;
    using Slash.Unity.DataBind.Core.Presentation;

    using UnityEngine;

    /// <summary>
    ///   Smoothes adding item to a collection.
    ///   Most commonly used to avoid creating multiple game objects in the same frame.
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Formatters/[DB] Smooth Collection Changes Formatter")]
    public class SmoothCollectionChangesFormatter : DataProvider
    {
        #region Fields

        /// <summary>
        ///   Collection to smooth.
        /// </summary>
        [Tooltip("Collection to smooth.")]
        public DataBinding Collection;

        /// <summary>
        ///   Interval between two items to be added, in seconds.
        /// </summary>
        [Tooltip("Interval between two items to be added, in seconds.")]
        public float Interval;

        /// <summary>
        ///   Collection to smooth.
        /// </summary>
        private Collection boundCollection;

        /// <summary>
        ///   Items to add one by one to the smoothed version of the collection.
        /// </summary>
        private Queue<object> queue;

        /// <summary>
        ///   Smoothed version of the bound collection.
        ///   Might contain less items than the bound collection, but fills up over time.
        /// </summary>
        private Collection<object> smoothedCollection;

        /// <summary>
        ///   Time remaining before the next item is added to the smoothed collection, in seconds.
        /// </summary>
        private float timeRemaining;

        #endregion

        #region Properties

        /// <summary>
        ///   Current data value.
        /// </summary>
        public override object Value
        {
            get
            {
                return this.smoothedCollection;
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
            this.AddBinding(this.Collection);
        }

        /// <summary>
        ///   Unity callback.
        /// </summary>
        protected override void OnDisable()
        {
            base.OnDisable();

            // Clear collection. Prevents item setters from immediately re-creating all game objects on enable.
            this.OnCollectionCleared();
        }

        /// <summary>
        ///   Unity callback.
        /// </summary>
        protected override void OnEnable()
        {
            base.OnEnable();

            // Smooth in all collection items again.
            this.ResetQueue();
        }

        /// <summary>
        ///   Called when the value of the data provider should be updated.
        /// </summary>
        protected override void UpdateValue()
        {
            var newCollection = this.Collection.GetValue<Collection>();

            if (newCollection == this.boundCollection)
            {
                return;
            }

            if (this.boundCollection != null)
            {
                // Remove from modifications of the collection.
                this.boundCollection.ItemAdded -= this.OnCollectionItemAdded;
                this.boundCollection.Cleared -= this.OnCollectionCleared;
            }

            this.boundCollection = newCollection;

            if (this.boundCollection != null)
            {
                // Register for modifications of the collection.
                this.boundCollection.ItemAdded += this.OnCollectionItemAdded;
                this.boundCollection.Cleared += this.OnCollectionCleared;
            }

            this.ResetQueue();
        }

        private void OnCollectionCleared()
        {
            // Apply to smoothed collection.
            this.queue.Clear();
            this.smoothedCollection.Clear();
        }

        private void OnCollectionItemAdded(object item)
        {
            this.queue.Enqueue(item);
        }

        private void ResetQueue()
        {
            this.queue = new Queue<object>();
            this.smoothedCollection = new Collection<object>();
            this.timeRemaining = 0;

            if (this.boundCollection != null)
            {
                foreach (var item in this.boundCollection)
                {
                    this.queue.Enqueue(item);
                }
            }

            this.OnValueChanged(this.Value);
        }

        /// <summary>
        ///   Unity callback.
        /// </summary>
        protected void Update()
        {
            if (this.queue.Count == 0)
            {
                return;
            }

            this.timeRemaining -= Time.deltaTime;

            if (this.timeRemaining > 0)
            {
                return;
            }

            // Add next item to smoothed collection.
            var item = this.queue.Dequeue();
            this.smoothedCollection.Add(item);

            // Update timer.
            this.timeRemaining += this.Interval;
        }

        #endregion
    }
}