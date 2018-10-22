// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContextHolder.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Core.Presentation
{
    using System;

    using Slash.Unity.DataBind.Core.Utils;

    using UnityEngine;

    /// <summary>
    ///   Holds a data context to specify the context to use on the presentation side.
    /// </summary>
    [AddComponentMenu("Data Bind/Core/[DB] Context Holder")]
    public class ContextHolder : MonoBehaviour
    {
        #region Fields

        private object context;

        [SerializeField]
        [ContextType]
        [Tooltip("Type of context this holder expects.")]
        private string contextType;

        /// <summary>
        ///   Should a context of the specified type be created at startup?
        /// </summary>
        [SerializeField]
        [Tooltip("Create context on startup?")]
        private bool createContext;

        #endregion

        #region Delegates

        /// <summary>
        ///   Delegate for ContextChanged event.
        /// </summary>
        /// <param name="newContext">New context.</param>
        public delegate void ContextChangedDelegate(object newContext);

        #endregion

        #region Events

        /// <summary>
        ///   Called when the context of this holder changed.
        /// </summary>
        public event ContextChangedDelegate ContextChanged;

        #endregion

        #region Properties

        /// <summary>
        ///   Data context.
        /// </summary>
        public object Context
        {
            get
            {
                return this.context;
            }
            set
            {
                this.SetContext(value, null);
            }
        }

        /// <summary>
        ///   Type of context to create on startup.
        /// </summary>
        public Type ContextType
        {
            get
            {
                try
                {
                    return this.contextType != null ? ReflectionUtils.FindType(this.contextType) : null;
                }
                catch (TypeLoadException)
                {
                    Debug.LogError("Can't find context type '" + this.contextType + "'.", this);
                    return null;
                }
            }
        }

        /// <summary>
        ///   Indicates if a context should be created from the specified context type.
        /// </summary>
        public bool CreateContext
        {
            get
            {
                return this.createContext;
            }
            set
            {
                this.createContext = value;
            }
        }

        /// <summary>
        ///   Path from parent to the context.
        ///   Used to resolve relative paths.
        /// </summary>
        public string Path { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Sets the context and its path from the parent to the context.
        /// </summary>
        /// <param name="newContext">Context.</param>
        /// <param name="path">Path from the parent context to the specified one.</param>
        public void SetContext(object newContext, string path)
        {
            if (newContext == this.context && path == this.Path)
            {
                return;
            }

            this.context = newContext;
            this.Path = path;

            this.OnContextChanged();
        }

        #endregion

        #region Methods

        /// <summary>
        ///   Unity callback.
        /// </summary>
        protected virtual void Awake()
        {
            if (this.Context == null && this.ContextType != null && this.CreateContext)
            {
                this.SetContext(Activator.CreateInstance(this.ContextType), null);
            }
        }

        /// <summary>
        ///   Called when the context of this holder changed.
        /// </summary>
        protected virtual void OnContextChanged()
        {
            // Update child bindings as context changed.
            var contextOperators = this.gameObject.GetComponentsInChildren<IContextOperator>(true);
            foreach (var contextOperator in contextOperators)
            {
                contextOperator.OnContextChanged();
            }

            var handler = this.ContextChanged;
            if (handler != null)
            {
                handler(this.Context);
            }
        }

        #endregion
    }
}