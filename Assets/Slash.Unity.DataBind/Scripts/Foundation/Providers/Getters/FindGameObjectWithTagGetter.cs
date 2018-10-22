using Slash.Unity.DataBind.Core.Presentation;
using UnityEngine;

namespace Slash.Unity.DataBind.Foundation.Providers.Getters
{
    /// <summary>
    ///   Returns the game object with the specific tag or null if none was found.
    /// </summary>
    public class FindGameObjectWithTagGetter : DataProvider
    {
        /// <summary>
        ///   Tag to find.
        /// </summary>
        public DataBinding Tag;

        /// <summary>
        ///   Current data value.
        /// </summary>
        public override object Value
        {
            get
            {
                var tagToFind = this.Tag.GetValue<string>();
                if (string.IsNullOrEmpty(tagToFind))
                {
                    return null;
                }
                try
                {
                    return GameObject.FindWithTag(tagToFind);
                }
                catch (UnityException)
                {
                    return null;
                }
            }
        }

        /// <summary>
        ///   Called when the value of the data provider should be updated.
        /// </summary>
        protected override void UpdateValue()
        {
            this.OnValueChanged(this.Value);
        }

        /// <summary>
        ///   Unity callback.
        /// </summary>
        protected void Awake()
        {
            this.AddBinding(this.Tag);
        }

        /// <summary>
        ///   Unity callback.
        /// </summary>
        protected void OnDestroy()
        {
            this.RemoveBinding(this.Tag);
        }
    }
}