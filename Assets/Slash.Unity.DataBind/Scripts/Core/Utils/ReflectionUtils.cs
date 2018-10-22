// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReflectionUtils.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Core.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    ///   Provides utility methods for reflecting types and members.
    /// </summary>
    public static class ReflectionUtils
    {
        #region Public Methods and Operators

#if UNITY_EDITOR || !UNITY_METRO
        /// <summary>
        ///   Creates a delegate of the specified type that represents the specified static or instance method, with the specified
        ///   first argument.
        /// </summary>
        /// <param name="type">The Type of delegate to create.</param>
        /// <param name="target">The object to which the delegate is bound, or null to treat method as static. </param>
        /// <param name="method">The MethodInfo describing the static or instance method the delegate is to represent.</param>
        /// <returns>A delegate of the specified type that represents the specified static or instance method. </returns>
        public static Delegate CreateDelegate(Type type, object target, MethodInfo method)
        {
            return Delegate.CreateDelegate(type, target, method);
        }

        /// <summary>
        ///   Searches all loaded assemblies and returns the types which have the specified attribute.
        /// </summary>
        /// <param name="baseType">Base type to get the types of.</param>
        /// <returns>List of found types.</returns>
        public static List<Type> FindTypesWithBase(Type baseType)
        {
            var types = new List<Type>();
            foreach (var assembly in AssemblyUtils.GetLoadedAssemblies())
            {
                try
                {
                    types.AddRange(assembly.GetTypes().Where(baseType.IsAssignableFrom));
                }
                catch (ReflectionTypeLoadException)
                {
                    // Some assemblies might not be accessible, skip them.
                }
            }

            return types;
        }

        /// <summary>
        ///   Returns the base type of the specified type.
        /// </summary>
        /// <param name="type">Type to get base type for.</param>
        /// <returns>Base type of specified type.</returns>
        public static Type GetBaseType(Type type)
        {
            return type.BaseType;
        }

        /// <summary>
        ///   Searches for the public field with the specified name.
        /// </summary>
        /// <param name="type">Type to search in.</param>
        /// <param name="name">The string containing the name of the data field to get.</param>
        /// <returns>An object representing the public field with the specified name, if found; otherwise, null.</returns>
        public static FieldInfo GetPublicField(Type type, string name)
        {
            return type.GetField(name, BindingFlags.Instance | BindingFlags.Public);
        }

        /// <summary>
        ///   Searches for the private field with the specified name.
        /// </summary>
        /// <param name="type">Type to search in.</param>
        /// <param name="name">The string containing the name of the data field to get.</param>
        /// <returns>An object representing the private field with the specified name, if found; otherwise, null.</returns>
        public static FieldInfo GetPrivateField(Type type, string name)
        {
            return type.GetField(name, BindingFlags.Instance | BindingFlags.NonPublic);
        }

        /// <summary>
        ///   Searches for the public method with the specified name.
        /// </summary>
        /// <param name="type">Type to search in.</param>
        /// <param name="name">The string containing the name of the public method to get. </param>
        /// <returns>An object that represents the public method with the specified name, if found; otherwise, null.</returns>
        public static MethodInfo GetPublicMethod(Type type, string name)
        {
            return type.GetMethod(name);
        }

        /// <summary>
        ///   Searches for the public property with the specified name.
        /// </summary>
        /// <param name="type">Type to search in.</param>
        /// <param name="name">The string containing the name of the public property to get. </param>
        /// <returns>An object that represents the public property with the specified name, if found; otherwise, null.</returns>
        public static PropertyInfo GetPublicProperty(Type type, string name)
        {
            return type.GetProperty(name);
        }

        /// <summary>
        ///   Indicates if the specified type is an enum type.
        /// </summary>
        /// <param name="type">Type to check.</param>
        /// <returns>True if the specified type is an enum type; otherwise, false.</returns>
        public static bool IsEnum(Type type)
        {
            return type.IsEnum;
        }
#else
        /// <summary>
        ///   Creates a delegate of the specified type that represents the specified static or instance method, with the specified
        ///   first argument.
        /// </summary>
        /// <param name="type">The Type of delegate to create.</param>
        /// <param name="target">The object to which the delegate is bound, or null to treat method as static. </param>
        /// <param name="method">The MethodInfo describing the static or instance method the delegate is to represent.</param>
        /// <returns>A delegate of the specified type that represents the specified static or instance method. </returns>
        public static Delegate CreateDelegate(Type type, object target, MethodInfo method)
        {
            return method.CreateDelegate(type, target);
        }
        
        /// <summary>
        ///   Searches all loaded assemblies and returns the types which have the specified attribute.
        /// </summary>
        /// <param name="baseType">Base type to get the types of.</param>
        /// <returns>List of found types.</returns>
        public static List<Type> FindTypesWithBase(Type baseType)
        {
            var types = new List<Type>();
            foreach (var assembly in AssemblyUtils.GetLoadedAssemblies())
            {
                types.AddRange(
                    assembly.DefinedTypes.Where(baseType.GetTypeInfo().IsAssignableFrom)
                            .Select(typeInfo => typeInfo.AsType()));
            }

            return types;
        }
        
        /// <summary>
        ///   Returns the base type of the specified type.
        /// </summary>
        /// <param name="type">Type to get base type for.</param>
        /// <returns>Base type of specified type.</returns>
        public static Type GetBaseType(Type type)
        {
            return type.GetTypeInfo().BaseType;
        }

        /// <summary>
        ///   Searches for the public field with the specified name.
        /// </summary>
        /// <param name="type">Type to search in.</param>
        /// <param name="name">The string containing the name of the data field to get.</param>
        /// <returns>An object representing the public field with the specified name, if found; otherwise, null.</returns>
        public static FieldInfo GetPublicField(Type type, string name)
        {
            return
                GetBaseTypes(type)
                    .Select(baseType => baseType.GetTypeInfo().GetDeclaredField(name))
                    .FirstOrDefault(field => field != null && field.IsPublic);
        }

        /// <summary>
        ///   Searches for the private field with the specified name.
        /// </summary>
        /// <param name="type">Type to search in.</param>
        /// <param name="name">The string containing the name of the data field to get.</param>
        /// <returns>An object representing the private field with the specified name, if found; otherwise, null.</returns>
        public static FieldInfo GetPrivateField(Type type, string name)
        {
            return
                GetBaseTypes(type)
                    .Select(baseType => baseType.GetTypeInfo().GetDeclaredField(name))
                    .FirstOrDefault(field => field != null && field.IsPrivate);
        }

        /// <summary>
        ///   Searches for the public method with the specified name.
        /// </summary>
        /// <param name="type">Type to search in.</param>
        /// <param name="name">The string containing the name of the public method to get. </param>
        /// <returns>An object that represents the public method with the specified name, if found; otherwise, null.</returns>
        public static MethodInfo GetPublicMethod(Type type, string name)
        {
            return
                GetBaseTypes(type)
                    .Select(baseType => baseType.GetTypeInfo().GetDeclaredMethod(name))
                    .FirstOrDefault(method => method != null && method.IsPublic);
        }

        /// <summary>
        ///   Searches for the public property with the specified name.
        /// </summary>
        /// <param name="type">Type to search in.</param>
        /// <param name="name">The string containing the name of the public property to get. </param>
        /// <returns>An object that represents the public property with the specified name, if found; otherwise, null.</returns>
        public static PropertyInfo GetPublicProperty(Type type, string name)
        {
            // https://msdn.microsoft.com/en-us/library/kz0a8sxy(v=vs.110).aspx
            // A property is considered public to reflection if it has at least one accessor that is public.
            return
                GetBaseTypes(type)
                    .Select(baseType => baseType.GetTypeInfo().GetDeclaredProperty(name))
                    .FirstOrDefault(property => property != null && (property.GetMethod.IsPublic || property.SetMethod.IsPublic));
        }

        private static IEnumerable<Type> GetBaseTypes(Type type)
        {
            yield return type;

            var baseType = type.GetTypeInfo().BaseType;

            if (baseType != null)
            {
                foreach (var t in GetBaseTypes(baseType))
                {
                    yield return t;
                }
            }
        }
        
        /// <summary>
        ///   Indicates if the specified type is an enum type.
        /// </summary>
        /// <param name="type">Type to check.</param>
        /// <returns>True if the specified type is an enum type; otherwise, false.</returns>
        public static bool IsEnum(Type type)
        {
            return type.GetTypeInfo().IsEnum;
        }
#endif

        /// <summary>
        ///   <para>
        ///     Looks up the specified full type name in all loaded assemblies,
        ///     ignoring assembly version.
        ///   </para>
        ///   <para>
        ///     In order to understand how to access generic types,
        ///     see http://msdn.microsoft.com/en-us/library/w3f99sx1.aspx.
        ///   </para>
        /// </summary>
        /// <param name="fullName">Full name of the type to find.</param>
        /// <returns>Type with the specified name.</returns>
        /// <exception cref="TypeLoadException">If the type couldn't be found.</exception>
        public static Type FindType(string fullName)
        {
            if (string.IsNullOrEmpty(fullName))
            {
                return null;
            }

            // Split type name from .dll version.
            fullName = SystemExtensions.RemoveAssemblyInfo(fullName);

            var type = Type.GetType(fullName);

            if (type != null)
            {
                return type;
            }

            foreach (var asm in AssemblyUtils.GetLoadedAssemblies())
            {
                type = asm.GetType(fullName);
                if (type != null)
                {
                    return type;
                }
            }

            throw new TypeLoadException(string.Format("Unable to find type {0}.", fullName));
        }

        /// <summary>
        ///   Searches all loaded assemblies and returns the types which have the specified attribute.
        /// </summary>
        /// <returns>List of found types.</returns>
        /// <typeparam name="T">Type of the attribute to get the types of.</typeparam>
        public static IEnumerable<Type> FindTypesWithBase<T>() where T : class
        {
            return FindTypesWithBase(typeof(T));
        }
        
        /// <summary>
        ///   Tries to convert the specified value to the specified value.
        /// </summary>
        /// <param name="rawValue">Value to convert.</param>
        /// <param name="type">Type to convert to.</param>
        /// <param name="convertedValue">Converted value.</param>
        /// <returns>True if value could be converted; otherwise, false.</returns>
        public static bool TryConvertValue(object rawValue, Type type, out object convertedValue)
        {
            try
            {
                // Try convert enum.
                if (IsEnum(type) && rawValue is string)
                {
                    convertedValue = Enum.Parse(type, (string)rawValue);
                    return true;
                }
                convertedValue = Convert.ChangeType(rawValue, type);
                return true;
            }
            catch (Exception)
            {
                convertedValue = null;
                return false;
            }
        }

        #endregion
    }
}