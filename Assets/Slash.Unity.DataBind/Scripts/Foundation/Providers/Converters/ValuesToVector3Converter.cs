using Slash.Unity.DataBind.Core.Presentation;
using UnityEngine;

namespace Slash.Unity.DataBind.Foundation.Providers.Converters
{
    /// <summary>
    ///     Converts 3 single numbers to a <see cref="Vector3" /> object.
    /// </summary>
    public class ValuesToVector3Converter : DataProvider
    {
        /// <summary>
        ///     Data to use for x component of vector.
        /// </summary>
        [Tooltip("Data to use for x component of vector.")]
        public DataBinding ValueX;

        /// <summary>
        ///     Data to use for y component of vector.
        /// </summary>
        [Tooltip("Data to use for y component of vector.")]
        public DataBinding ValueY;

        /// <summary>
        ///     Data to use for z component of vector.
        /// </summary>
        [Tooltip("Data to use for z component of vector.")]
        public DataBinding ValueZ;

        /// <summary>
        ///   Current data value.
        /// </summary>
        public override object Value
        {
            get
            {
                return new Vector3(this.ValueX.GetValue<float>(), this.ValueY.GetValue<float>(),
                    this.ValueZ.GetValue<float>());
            }
        }

        /// <summary>
        ///   Unity callback.
        /// </summary>
        protected void Awake()
        {
            this.AddBinding(this.ValueX);
            this.AddBinding(this.ValueY);
            this.AddBinding(this.ValueZ);
        }

        /// <summary>
        ///   Unity callback.
        /// </summary>
        protected void OnDestroy()
        {
            this.RemoveBinding(this.ValueX);
            this.RemoveBinding(this.ValueY);
            this.RemoveBinding(this.ValueZ);
        }

        /// <summary>
        ///   Called when the value of the data provider should be updated.
        /// </summary>
        protected override void UpdateValue()
        {
            this.OnValueChanged(this.Value);
        }
    }
}