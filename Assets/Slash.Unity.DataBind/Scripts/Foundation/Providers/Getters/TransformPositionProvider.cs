namespace Slash.Unity.DataBind.Foundation.Providers.Getters
{
    using Slash.Unity.DataBind.Core.Presentation;

    using UnityEngine;

    /// <summary>
    ///   Provides the position of the target transform.
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Getters/[DB] Transform Position Provider")]
    public class TransformPositionProvider : DataProvider
    {
        #region Properties

        /// <summary>
        ///   Current data value.
        /// </summary>
        public override object Value
        {
            get
            {
                return this.transform.position;
            }
        }

        #endregion

        #region Methods

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