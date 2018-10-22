// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CollectionExampleContext.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Examples.Collection
{
    using System.Linq;

    using Slash.Unity.DataBind.Core.Data;

    /// <summary>
    ///   Context for Collection example.
    /// </summary>
    public class CollectionExampleContext : Context
    {
        #region Fields

        private readonly Collection<CollectionExampleItemContext> items = new Collection<CollectionExampleItemContext>();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Constructor.
        /// </summary>
        public CollectionExampleContext()
        {
            this.items.Add(new CollectionExampleItemContext { Text = "This" });
            this.items.Add(new CollectionExampleItemContext { Text = "Is" });
            this.items.Add(new CollectionExampleItemContext { Text = "Data Bind" });
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Items.
        /// </summary>
        public Collection<CollectionExampleItemContext> Items
        {
            get
            {
                return this.items;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Adds an item to the collection.
        /// </summary>
        public void AddItem()
        {
            this.items.Add(new CollectionExampleItemContext { Text = this.items.Count.ToString() });
        }

        /// <summary>
        ///   Removes an item from the collection.
        /// </summary>
        public void RemoveItem()
        {
            if (this.items.Count > 0)
            {
                this.items.Remove(this.items.Last());
            }
        }

        #endregion
    }
}