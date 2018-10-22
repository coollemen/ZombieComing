// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextAssetLoader.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Providers.Loaders
{
    using Slash.Unity.DataBind.Core.Presentation;

    using UnityEngine;

    /// <summary>
    ///   Fetches the data from a file resource.
    ///   <para>Input: String (Path to file resource).</para>
    ///   <para>Output: String (File resource contents).</para>
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Formatters/[DB] Text Asset Loader")]
    public class TextAssetLoader : DataProvider
    {
        #region Fields

        /// <summary>
        ///   Data value which contains the path to the file resource.
        /// </summary>
        public DataBinding FileResource;

        #endregion

        #region Properties

        /// <summary>
        ///   Current data value.
        /// </summary>
        public override object Value
        {
            get
            {
                if (this.FileResource == null)
                {
                    return null;
                }

                var fileResource = this.FileResource.GetValue<string>();
                var textAsset = Resources.Load<TextAsset>(fileResource);

                return textAsset != null ? textAsset.text : null;
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
            this.AddBinding(this.FileResource);
        }

        /// <summary>
        ///   Called when the value of the data provider should be updated.
        /// </summary>
        protected override void UpdateValue()
        {
            this.OnValueChanged(this.Value);
        }

        #endregion
    }
}