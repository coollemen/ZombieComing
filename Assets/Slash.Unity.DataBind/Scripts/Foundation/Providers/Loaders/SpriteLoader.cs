// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SpriteLoader.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Providers.Loaders
{
    using Slash.Unity.DataBind.Core.Presentation;

    using UnityEngine;

    /// <summary>
    ///   Data provider which loads a sprite from a sprite name.
    ///   <para>Input: String (Sprite name).</para>
    ///   <para>Output: Sprite (Loaded sprite).</para>
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Formatters/[DB] Sprite Loader")]
    public class SpriteLoader : DataProvider
    {
        #region Fields

        /// <summary>
        ///   Data which contains sprite name.
        /// </summary>
        [Tooltip("Data which contains sprite name.")]
        public DataBinding Data;

        private Sprite sprite;

        private string spriteName;

        #endregion

        #region Properties

        /// <summary>
        ///   Current data value.
        /// </summary>
        public override object Value
        {
            get
            {
                return this.sprite;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///   Unity callback.
        /// </summary>
        protected void Awake()
        {
            this.AddBinding(this.Data);
        }

        /// <summary>
        ///   Called when the value of the data provider should be updated.
        /// </summary>
        protected override void UpdateValue()
        {
            // Check if sprite name changed.
            var newSpriteName = this.Data.GetValue<string>();
            if (newSpriteName == this.spriteName)
            {
                return;
            }

            // Load new sprite.
            this.spriteName = newSpriteName;
            this.sprite = this.spriteName != null ? Resources.Load<Sprite>(this.spriteName) : null;
            if (this.sprite == null && !string.IsNullOrEmpty(this.spriteName))
            {
                Debug.LogWarningFormat("No sprite resource found at path '{0}'", this.spriteName);
            }

            this.OnValueChanged(this.sprite);
        }

        #endregion
    }
}