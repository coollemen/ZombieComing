// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameObjectItemsSetter.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Setters
{
    using System.Collections.Generic;
    using System.Linq;

    using Slash.Unity.DataBind.Core.Data;
    using Slash.Unity.DataBind.Core.Presentation;

    using UnityEngine;

    /// <summary>
    ///   Simplest implementation of an items setter that just instantiates a game object for each
    ///   item in a collection.
    /// </summary>
    public class GameObjectItemsSetter : GameObjectItemsSetter<MonoBehaviour>
    {
    }

    /// <summary>
    ///   Base class which adds game objects for each item of an ItemsSetter.
    /// </summary>
    /// <typeparam name="TBehaviour">Type of mono behaviour.</typeparam>
    public abstract class GameObjectItemsSetter<TBehaviour> : ItemsSetter<TBehaviour>
        where TBehaviour : MonoBehaviour
    {
        /// <summary>
        ///   Items.
        /// </summary>
        private readonly List<Item> items = new List<Item>();

        /// <summary>
        ///   Prefab to create the items from.
        /// </summary>
        public GameObject Prefab;

        /// <summary>
        ///   Returns an enumerator for the game objects of all items.
        /// </summary>
        protected IEnumerable<GameObject> ItemGameObjects
        {
            get
            {
                return this.items.Select(item => item.GameObject);
            }
        }

        /// <summary>
        ///   Unity callback.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            if (this.Prefab != null)
            {
                // Deactivate prefab.
                this.Prefab.SetActive(false);
            }
        }

        /// <summary>
        ///   Clears all created items.
        /// </summary>
        protected override void ClearItems()
        {
            foreach (var item in this.items)
            {
                Destroy(item.GameObject);
            }
            this.items.Clear();
        }

        /// <summary>
        ///   Creates an item for the specified item context.
        /// </summary>
        /// <param name="itemContext">Item context for the item to create.</param>
        /// <param name="itemIndex">Index of item to create.</param>
        protected override void CreateItem(object itemContext, int itemIndex)
        {
            // Instantiate item game object inactive to avoid duplicate initialization.
            this.Prefab.SetActive(false);
            var item = Instantiate(this.Prefab);

            this.items.Add(new Item { GameObject = item, Context = itemContext });
            this.OnItemCreated(itemContext, item);

            // Set item context after setup as the parent may change which influences the context path.
            if (itemContext != null)
            {
                // Set item data context.
                var itemContextHolder = item.GetComponent<ContextHolder>();
                if (itemContextHolder == null)
                {
                    itemContextHolder = item.AddComponent<ContextHolder>();
                }

                var path = this.Data.Type == DataBindingType.Context ? this.Data.Path : string.Empty;
                itemContextHolder.SetContext(itemContext, path + Context.PathSeparator + itemIndex);
            }

            // Activate after the context was set.
            item.SetActive(true);
        }

        /// <summary>
        ///   Called when an item for an item context was created.
        /// </summary>
        /// <param name="itemContext">Item context the item is for.</param>
        /// <param name="itemObject">Item game object.</param>
        protected virtual void OnItemCreated(object itemContext, GameObject itemObject)
        {
            // By default use Target transform as parent.
            itemObject.transform.SetParent(this.Target.transform, false);
        }

        /// <summary>
        ///   Called when an item for an item context was destroyed.
        /// </summary>
        /// <param name="itemContext">Item context the item was for.</param>
        /// <param name="itemObject">Item game object.</param>
        protected virtual void OnItemDestroyed(object itemContext, GameObject itemObject)
        {
        }

        /// <summary>
        ///   Removes the item with the specified item context.
        /// </summary>
        /// <param name="itemContext">Item context of the item to remove.</param>
        protected override void RemoveItem(object itemContext)
        {
            // Get item.
            var item = this.items.FirstOrDefault(existingItem => existingItem.Context == itemContext);
            if (item == null)
            {
                Debug.LogWarning("No item found for collection item " + itemContext, this);
                return;
            }

            // Remove item.
            this.items.Remove(item);
            this.OnItemDestroyed(item.Context, item.GameObject);

            // Destroy item.
            Destroy(item.GameObject);
        }

        private class Item
        {
            public object Context { get; set; }

            public GameObject GameObject { get; set; }
        }
    }
}