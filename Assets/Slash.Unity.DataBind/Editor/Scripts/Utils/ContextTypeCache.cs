// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContextTypeCache.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Editor.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Slash.Unity.DataBind.Core.Data;
    using Slash.Unity.DataBind.Core.Utils;

    /// <summary>
    ///   Cache for context type reflection data.
    /// </summary>
    public static class ContextTypeCache
    {
        #region Constants

        private static readonly List<PathNode> CachedPaths = new List<PathNode>();

        /// <summary>
        ///   Maximum path depth to avoid infinite loops.
        /// </summary>
        private const int MaxPathDepth = 10;

        #endregion

        #region Constructors and Destructors

        static ContextTypeCache()
        {
            ContextTypes = new List<Type> { null };
            var availableContextTypes =
                ReflectionUtils.FindTypesWithBase<Context>().Where(type => !type.IsAbstract).ToList();
            availableContextTypes.Sort(
                (typeA, typeB) => String.Compare(typeA.FullName, typeB.FullName, StringComparison.Ordinal));
            ContextTypes.AddRange(availableContextTypes);
            ContextTypeNames = ContextTypes.Select(type => type != null ? type.FullName : "None").ToArray();
            ContextTypePaths = ContextTypeNames.Select(typeName => typeName.Replace(".", "/")).ToArray();
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Names of context types.
        /// </summary>
        public static string[] ContextTypeNames { get; private set; }

        /// <summary>
        ///   Names of context types.
        ///   But using slashes instead of dots to provide a (namespace) path to the class.
        /// </summary>
        public static string[] ContextTypePaths { get; private set; }

        public static List<Type> ContextTypes { get; private set; }

        #endregion

        #region Public Methods and Operators

        public static IEnumerable<ContextMemberInfo> GetMemberInfos(Type type)
        {
            // Collect all public members.
            var propertyInfos = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (var propertyInfo in propertyInfos)
            {
                // Skip data property.
                var dataPropertyType = typeof(Property);
                if (dataPropertyType.IsAssignableFrom(propertyInfo.PropertyType))
                {
                    continue;
                }

                // Skip indexer.
                if (propertyInfo.GetIndexParameters().Length > 0)
                {
                    continue;
                }

                yield return new ContextMemberInfo { Property = propertyInfo };
            }
            var fieldInfos = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
            foreach (var fieldInfo in fieldInfos)
            {
                // Skip data property.
                var dataPropertyType = typeof(Property);
                if (!dataPropertyType.IsAssignableFrom(fieldInfo.FieldType))
                {
                    yield return new ContextMemberInfo { Field = fieldInfo };
                }
            }
            var methodInfos =
                type.GetMethods(BindingFlags.Instance | BindingFlags.Public)
                    .Where(
                        methodInfo =>
                            !methodInfo.IsSpecialName && methodInfo.DeclaringType != typeof(Object)
                            && methodInfo.DeclaringType != typeof(Context) && methodInfo.ReturnType == typeof(void));
            foreach (var methodInfo in methodInfos)
            {
                yield return new ContextMemberInfo { Method = methodInfo };
            }
        }

        /// <summary>
        ///   Returns all available paths of the specified <see cref="Context" /> type.
        /// </summary>
        /// <param name="type">Type of context to get paths for.</param>
        /// <param name="filter">Filter to only return specific members of the context.</param>
        /// <returns>List of available paths of the specified <see cref="Context" /> type.</returns>
        public static List<string> GetPaths(Type type, ContextMemberFilter filter = ContextMemberFilter.All)
        {
            if (type == null)
            {
                return null;
            }

            var pathNode = GetPathNode(type);
            return CreatePaths(pathNode, filter, 0);
        }

        #endregion

        #region Methods

        private static bool CheckFilter(PathNode pathNode, ContextMemberFilter filter)
        {
            if (filter.IsOptionSet(ContextMemberFilter.Fields) && pathNode.Info.Field != null)
            {
                return true;
            }
            if (filter.IsOptionSet(ContextMemberFilter.Properties) && pathNode.Info.Property != null)
            {
                return true;
            }
            if (filter.IsOptionSet(ContextMemberFilter.Methods) && pathNode.Info.Method != null)
            {
                return true;
            }

            if (filter.IsOptionSet(ContextMemberFilter.Contexts) && typeof(Context).IsAssignableFrom(pathNode.Type))
            {
                return true;
            }

            if (filter.IsOptionSet(ContextMemberFilter.Triggers) && typeof(DataTrigger).IsAssignableFrom(pathNode.Type))
            {
                return true;
            }

            return false;
        }

        private static List<string> CreatePaths(PathNode pathNode, ContextMemberFilter filter, int depth)
        {
            var paths = new List<string>();

            // Check for maximum path depth.
            if (depth > MaxPathDepth)
            {
                return paths;
            }

            // Check if node has children.
            if (pathNode.Children == null)
            {
                return paths;
            }

            foreach (var childNode in pathNode.Children)
            {
                // Check if node should be considered.
                if (CheckFilter(childNode, filter))
                {
                    var childName = childNode.Info.Name;
                    paths.Add(childName);
                }

                // Check if to step into child nodes.
                if (filter.IsOptionSet(ContextMemberFilter.Recursive))
                {
                    var subContextPaths = CreatePaths(childNode, filter, depth + 1);
                    paths.AddRange(subContextPaths.Select(subContextPath => childNode.Info.Name + "." + subContextPath));
                }
            }

            return paths;
        }

        private static PathNode GetPathNode(Type type)
        {
            // Check if cached.
            var pathNode = CachedPaths.FirstOrDefault(node => node.Type == type);
            if (pathNode != null)
            {
                return pathNode;
            }

            // Create new.
            pathNode = new PathNode { Type = type };
            CachedPaths.Add(pathNode);

            // Check if collection or context.
            var dataContextType = typeof(Context);
            var collectionType = typeof(Collection);
            var isCollection = collectionType.IsAssignableFrom(type);
            if (!isCollection && !dataContextType.IsAssignableFrom(type))
            {
                return pathNode;
            }

            pathNode.Children = new List<PathNode>();

            var memberInfos = GetMemberInfos(type);
            foreach (var memberInfo in memberInfos)
            {
                if (isCollection)
                {
                    // Skip "IsReadOnly" property.
                    if (memberInfo.Name == "IsReadOnly")
                    {
                        continue;
                    }
                }

                var childNode = GetPathNode(memberInfo);
                pathNode.Children.Add(childNode);
            }

            return pathNode;
        }

        private static PathNode GetPathNode(ContextMemberInfo memberInfo)
        {
            PathNode pathNode;
            if (memberInfo.Field != null)
            {
                pathNode = new PathNode(GetPathNode(memberInfo.Field.FieldType));
            }
            else if (memberInfo.Property != null)
            {
                pathNode = new PathNode(GetPathNode(memberInfo.Property.PropertyType));
            }
            else
            {
                pathNode = new PathNode();
            }
            pathNode.Info = memberInfo;
            return pathNode;
        }

        #endregion

        public class ContextMemberInfo
        {
            #region Properties

            public FieldInfo Field { get; set; }

            public MethodInfo Method { get; set; }

            public string Name
            {
                get
                {
                    if (this.Field != null)
                    {
                        return this.Field.Name;
                    }
                    if (this.Property != null)
                    {
                        return this.Property.Name;
                    }
                    if (this.Method != null)
                    {
                        return this.Method.Name;
                    }
                    return null;
                }
            }

            public PropertyInfo Property { get; set; }

            #endregion
        }

        private class PathNode
        {
            #region Constructors and Destructors

            public PathNode(PathNode pathNode)
            {
                this.Children = pathNode.Children;
                this.Type = pathNode.Type;
                this.Info = pathNode.Info;
            }

            public PathNode()
            {
            }

            #endregion

            #region Properties

            public List<PathNode> Children { get; set; }

            public ContextMemberInfo Info { get; set; }

            public Type Type { get; set; }

            #endregion
        }
    }
}