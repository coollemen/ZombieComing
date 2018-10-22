// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContextNode.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Core.Presentation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Slash.Unity.DataBind.Core.Data;

    using UnityEngine;

    /// <summary>
    ///   Node which works with a data context and caches contexts and master paths.
    ///   Always bound to a specific game object which specifies the hierarchy.
    /// </summary>
    public sealed class ContextNode
    {
        #region Constants

        private const int MaxPathDepth = 100500;

        #endregion

        #region Fields

        /// <summary>
        ///   Context cache for faster look up.
        /// </summary>
        private readonly Dictionary<int, ContextHolder> contexts = new Dictionary<int, ContextHolder>();

        /// <summary>
        ///   Game object to do the lookup for.
        /// </summary>
        private readonly GameObject gameObject;

        /// <summary>
        ///   Master path cache for faster look up.
        /// </summary>
        private readonly Dictionary<int, string> masterPaths = new Dictionary<int, string>();

        /// <summary>
        ///   Path in context this node is bound to.
        /// </summary>
        private readonly string path;

        /// <summary>
        ///   Context to use for data lookup.
        /// </summary>
        private object context;

        /// <summary>
        ///   Full path to data starting from context.
        /// </summary>
        private string contextPath;

        /// <summary>
        ///   Callback when value changed.
        /// </summary>
        private Action<object> valueChangedCallback;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Constructor.
        /// </summary>
        /// <param name="gameObject">Game object this node is assigned to.</param>
        /// <param name="path">Path in context this node is bound to.</param>
        public ContextNode(GameObject gameObject, string path)
        {
            this.gameObject = gameObject;
            this.path = path;
            this.OnHierarchyChanged();
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Current context for this node.
        /// </summary>
        public object Context
        {
            get
            {
                return this.context;
            }
            private set
            {
                if (value == this.context)
                {
                    return;
                }

                // Remove listener from old context.
                this.RemoveListener();

                this.context = value;

                // Add listener to new context.
                var initialValue = this.RegisterListener();
                if (this.valueChangedCallback != null)
                {
                    this.valueChangedCallback(initialValue);
                }
            }
        }

        /// <summary>
        ///   Indicates if the context node already holds a valid value.
        /// </summary>
        public bool IsInitialized
        {
            get
            {
                return this.context != null;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Informs the context node that the hierarchy changed, so the context and/or master paths may have changed.
        ///   Has to be called:
        ///   - Anchestor context changed.
        ///   - Anchestor master path changed.
        /// </summary>
        public void OnHierarchyChanged()
        {
            // Update master paths.
            this.UpdateCache();

            // Update context.
            var depthToGo = GetPathDepth(this.path);

            // Take first context holder which is deep enough to use as a starting point.
            var contextHolderPair = depthToGo == MaxPathDepth
                ? this.contexts.OrderByDescending(pair => pair.Key).FirstOrDefault()
                : this.contexts.FirstOrDefault(pair => depthToGo <= pair.Key);
            var contextHolder = contextHolderPair.Value;

            object newContext = contextHolder != null ? contextHolder.Context : null;

            // Adjust full path.
            this.contextPath = this.GetFullCleanPath(depthToGo, contextHolderPair.Key);

            this.Context = newContext;
        }

        /// <summary>
        ///   Sets the specified value at the specified path.
        /// </summary>
        /// <param name="value">Value to set.</param>
        public void SetValue(object value)
        {
            // Set value on data context.
            var dataContext = this.context as Context;
            if (dataContext == null)
            {
                return;
            }

            try
            {
                dataContext.SetValue(this.contextPath, value);
            }
            catch (ArgumentException e)
            {
                Debug.LogError(e, this.gameObject);
            }
            catch (InvalidOperationException e)
            {
                Debug.LogError(e, this.gameObject);
            }
        }

        /// <summary>
        ///   Sets the callback which is called when the value of the monitored data in the context changed.
        /// </summary>
        /// <param name="onValueChanged">Callback to invoke when the value of the monitored data in the context changed.</param>
        /// <returns>Initial value.</returns>
        public object SetValueListener(Action<object> onValueChanged)
        {
            // Remove old callback.
            this.RemoveListener();

            this.valueChangedCallback = onValueChanged;

            // Add new callback.
            return this.RegisterListener();
        }

        #endregion

        #region Methods

        private static string GetCleanPath(string path)
        {
            if (!path.StartsWith("#"))
            {
                return path;
            }
            var dotIndex = path.IndexOf(Data.Context.PathSeparator);
            var result = (dotIndex < 0) ? null : path.Substring(dotIndex + 1);
            return result;
        }

        /// <summary>
        ///   Converts the specified path to a full, clean path. I.e. replaces the depth value and prepends the master paths.
        /// </summary>
        /// <returns>Full clean path for the specified path.</returns>
        private string GetFullCleanPath(int startDepth, int endDepth)
        {
            var cleanPath = GetCleanPath(this.path);

            var fullPath = cleanPath;
            for (var depth = startDepth; depth < endDepth; ++depth)
            {
                string masterPath;
                if (this.masterPaths.TryGetValue(depth, out masterPath) && !string.IsNullOrEmpty(masterPath))
                {
                    fullPath = masterPath + Data.Context.PathSeparator + fullPath;
                }
            }

            return fullPath;
        }

        private static int GetPathDepth(string path)
        {
            if (!path.StartsWith("#"))
            {
                return 0;
            }
            var depthString = path.Substring(1);
            var dotIndex = depthString.IndexOf(Data.Context.PathSeparator);
            if (dotIndex >= 0)
            {
                depthString = depthString.Substring(0, dotIndex);
            }
            if (depthString == "#")
            {
                return MaxPathDepth;
            }
            int depth;
            if (int.TryParse(depthString, out depth))
            {
                return depth;
            }
            Debug.LogWarning("Failed to get binding context depth for: " + path);
            return 0;
        }

        /// <summary>
        ///   Registers a callback at the current context.
        /// </summary>
        /// <returns>Current value.</returns>
        private object RegisterListener()
        {
            // Return context itself if no path set.
            if (string.IsNullOrEmpty(this.contextPath))
            {
                return this.context;
            }

            var dataContext = this.context as Context;
            if (dataContext != null)
            {
                try
                {
                    return this.valueChangedCallback != null
                        ? dataContext.RegisterListener(this.contextPath, this.valueChangedCallback)
                        : null;
                }
                catch (ArgumentException e)
                {
                    Debug.LogError(e, this.gameObject);
                    return null;
                }
            }

            // If context is not null, but path is set, it is not derived from context class, so 
            // the path can't be resolved. Log an error to inform the user that the context should be derived
            // from the Context class.
            if (this.context != null)
            {
                Debug.LogError(
                    string.Format(
                        "Context of type '{0}' is not derived from '{1}', but path is set to '{2}'. Not able to get data from a non-context type.",
                        this.context.GetType(),
                        typeof(Context),
                        this.contextPath),
                    this.gameObject);
            }

            return null;
        }

        /// <summary>
        ///   Removes the callback from the current context.
        /// </summary>
        private void RemoveListener()
        {
            // Return if no path set.
            if (string.IsNullOrEmpty(this.contextPath))
            {
                return;
            }

            var dataContext = this.context as Context;
            if (dataContext == null || this.valueChangedCallback == null)
            {
                return;
            }

            // Remove listener.
            try
            {
                dataContext.RemoveListener(this.contextPath, this.valueChangedCallback);
            }
            catch (ArgumentException e)
            {
                Debug.LogError(e, this.gameObject);
            }
        }

        /// <summary>
        ///   Updates the master path and context cache.
        /// </summary>
        private void UpdateCache()
        {
            // Clear cache.
            this.contexts.Clear();
            this.masterPaths.Clear();

            var p = this.gameObject;

            var depth = 0;

            while (p != null)
            {
                var contextHolder = p.GetComponent<ContextHolder>();
                if (contextHolder != null)
                {
                    if (!this.contexts.ContainsKey(depth))
                    {
                        this.contexts.Add(depth, contextHolder);
                    }

                    // Process path.
                    string pathRest = contextHolder.Path;
                    while (!string.IsNullOrEmpty(pathRest))
                    {
                        var separatorIndex = pathRest.LastIndexOf(Data.Context.PathSeparator);
                        string pathSection;
                        if (separatorIndex >= 0)
                        {
                            pathSection = pathRest.Substring(separatorIndex + 1);
                            pathRest = pathRest.Substring(0, separatorIndex);
                        }
                        else
                        {
                            pathSection = pathRest;
                            pathRest = null;
                        }
                        this.masterPaths.Add(depth, pathSection);
                        ++depth;
                    }
                }

                var masterPath = p.GetComponent<MasterPath>();
                if (masterPath != null)
                {
                    // Process path.
                    string pathRest = masterPath.Path;
                    while (!string.IsNullOrEmpty(pathRest))
                    {
                        var separatorIndex = pathRest.LastIndexOf(Data.Context.PathSeparator);
                        string pathSection;
                        if (separatorIndex >= 0)
                        {
                            pathSection = pathRest.Substring(separatorIndex + 1);
                            pathRest = pathRest.Substring(0, separatorIndex);
                        }
                        else
                        {
                            pathSection = pathRest;
                            pathRest = null;
                        }
                        this.masterPaths.Add(depth, pathSection);
                        ++depth;
                    }
                }
                p = (p.transform.parent == null) ? null : p.transform.parent.gameObject;
            }
        }

        #endregion
    }
}