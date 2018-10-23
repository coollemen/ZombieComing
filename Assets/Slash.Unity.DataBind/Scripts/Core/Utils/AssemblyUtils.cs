﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AssemblyUtils.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Core.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

#if !UNITY_EDITOR && UNITY_METRO
    using Windows.ApplicationModel;
    using Windows.Foundation;
    using Windows.Storage;
#endif

    /// <summary>
    ///   Utility methods for operating on assemblies.
    /// </summary>
    public class AssemblyUtils
    {
#if !UNITY_EDITOR && UNITY_METRO
        #region Static Fields

        /// <summary>
        ///   Cached list of loaded assemblies.
        /// </summary>
        private static List<Assembly> loadedAssemblies;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Gets all assemblies that are loaded in the current application domain.
        /// </summary>
        /// <returns>All loaded assemblies.</returns>
        public static IEnumerable<Assembly> GetLoadedAssemblies()
        {
            // Check if cached.
            if (loadedAssemblies != null)
            {
                return loadedAssemblies;
            }

            // Find assemblies.
            StorageFolder folder = Package.Current.InstalledLocation;

            loadedAssemblies = new List<Assembly>();

            IAsyncOperation<IReadOnlyList<StorageFile>> folderFilesAsync = folder.GetFilesAsync();
            folderFilesAsync.AsTask().Wait();

            foreach (StorageFile file in folderFilesAsync.GetResults())
            {
                if (file.FileType == ".dll" || file.FileType == ".exe")
                {
                    try
                    {
                        var filename = file.ViewName.Substring(0, file.ViewName.Length - file.FileType.Length);
                        AssemblyName name = new AssemblyName { ViewName = filename };
                        Assembly asm = Assembly.Load(name);
                        loadedAssemblies.Add(asm);
                    }
                    catch (BadImageFormatException)
                    {
                        /*
                         * TODO(np): Thrown reflecting on C++ executable files for which the C++ compiler
                         * stripped the relocation addresses (such as Unity dlls):
                         * http://msdn.microsoft.com/en-us/library/x4cw969y(v=vs.110).aspx
                         */
                    }
                }
            }

            return loadedAssemblies;
        }

        #endregion
#else

        #region Public Methods and Operators

        /// <summary>
        ///   Gets all assemblies that are loaded in the current application domain.
        /// </summary>
        /// <returns>All loaded assemblies.</returns>
        public static IEnumerable<Assembly> GetLoadedAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies();
        }

        #endregion

#endif
    }
}