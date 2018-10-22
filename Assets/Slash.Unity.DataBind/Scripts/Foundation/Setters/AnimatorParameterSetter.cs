namespace Slash.Unity.DataBind.Foundation.Setters
{
    using System.Collections;

    using UnityEngine;

    /// <summary>
    ///   Base class for a setter that updates a parameter of an <see cref="Animator"/>.
    /// </summary>
    /// <typeparam name="T">Type of parameter this setter handles.</typeparam>
    public abstract class AnimatorParameterSetter<T> : ComponentSingleSetter<Animator, T>
    {
        /// <summary>
        ///   Name of the animator parameter.
        /// </summary>
        [Tooltip("Name of an animator parameter.")]
        public string AnimatorParameterName;

        /// <summary>
        ///   Coroutine which will set the initial value.
        /// </summary>
        private Coroutine initializerCoroutine;

        /// <summary>
        ///   Called when the data binding value changed.
        /// </summary>
        /// <param name="newValue">New data value.</param>
        protected override void OnValueChanged(T newValue)
        {
            // Stop previous initializer.
            if (this.initializerCoroutine != null)
            {
                this.StopCoroutine(this.initializerCoroutine);
                this.initializerCoroutine = null;
            }

            if (this.Target.isInitialized)
            {
                this.SetAnimatorParameter(newValue);
            }
            else
            {
                // Delay setting parameter.
                this.initializerCoroutine = this.StartCoroutine((IEnumerator)this.InitializeAnimatorParameter(newValue));
            }
        }

        /// <summary>
        ///   Called when the animator parameter should be set to the specified value.
        /// </summary>
        /// <param name="newValue">Value to set animator parameter to.</param>
        protected abstract void SetAnimatorParameter(T newValue);

        private IEnumerator InitializeAnimatorParameter(T value)
        {
            while (!this.Target.isInitialized)
            {
                yield return new WaitForEndOfFrame();
            }

            this.SetAnimatorParameter(value);

            this.initializerCoroutine = null;
        }
    }
}