// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PrefabInstantiator.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Setters
{
    using Slash.Unity.DataBind.Core.Presentation;

    using UnityEngine;

    /// <summary>
    ///   Instantiates a game object from the bound prefab if one is provided.
    /// </summary>
    public class PrefabInstantiator : DataBindingOperator
    {
        /// <summary>
        ///   Parent transform to add instantiated game object to.
        /// </summary>
        public DataBinding Parent;

        /// <summary>
        ///   Prefab to instantiate.
        /// </summary>
        public DataBinding Prefab;

        private GameObject prefabGameObject;

        /// <summary>
        ///   Unity callback.
        /// </summary>
        protected void Awake()
        {
            this.AddBinding(this.Prefab);
        }

        /// <summary>
        ///   Unity callback.
        /// </summary>
        protected void OnDestroy()
        {
            this.RemoveBinding(this.Prefab);
        }

        /// <summary>
        ///   Unity callback.
        /// </summary>
        protected override void OnDisable()
        {
            base.OnDisable();
            this.Prefab.ValueChanged -= this.OnPrefabChanged;
        }

        /// <summary>
        ///   Unity callback.
        /// </summary>
        protected override void OnEnable()
        {
            base.OnEnable();
            this.Prefab.ValueChanged += this.OnPrefabChanged;
            this.UpdateGameObject();
        }

        private void CreateGameObject()
        {
            var prefab = this.Prefab.GetValue<GameObject>();
            if (prefab == null)
            {
                return;
            }

            var parent = this.Parent.GetValue<Transform>() ?? this.transform;
            
            this.prefabGameObject = Instantiate(prefab, parent, false);
            this.prefabGameObject.transform.localPosition = Vector3.zero;
        }

        private void OnPrefabChanged(object newvalue)
        {
            this.UpdateGameObject();
        }

        private void RemoveGameObject()
        {
            if (this.prefabGameObject != null)
            {
                Destroy(this.prefabGameObject);
                this.prefabGameObject = null;
            }
        }

        private void UpdateGameObject()
        {
            this.RemoveGameObject();
            this.CreateGameObject();
        }
    }
}