// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumGetter.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Providers.Getters
{
    using System;

    using Slash.Unity.DataBind.Core.Presentation;
    using Slash.Unity.DataBind.Core.Utils;

    using UnityEngine;

    /// <summary>
    ///   Provides the enum values of a specified enum type.
    /// </summary>
    public class EnumGetter : DataProvider
    {
        #region Fields

        [SerializeField]
        [TypeSelection(BaseType = typeof(Enum))]
        private string enumType;

        #endregion

        #region Properties

        /// <summary>
        ///   Type of enum to get.
        /// </summary>
        public Type EnumType
        {
            get
            {
                try
                {
                    return this.enumType != null ? ReflectionUtils.FindType(this.enumType) : null;
                }
                catch (TypeLoadException)
                {
                    Debug.LogError("Can't find type '" + this.enumType + "'.", this);
                    return null;
                }
            }
            set
            {
                this.enumType = value != null ? value.AssemblyQualifiedName : null;
            }
        }

        /// <summary>
        ///   Current data value.
        /// </summary>
        public override object Value
        {
            get
            {
                return Enum.GetValues(this.EnumType);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///   Called when the value of the data provider should be updated.
        /// </summary>
        protected override void UpdateValue()
        {
        }

        #endregion
    }
}