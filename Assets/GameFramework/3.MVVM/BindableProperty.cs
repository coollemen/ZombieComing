using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{
    public class BindableProperty<T>
    {

        public delegate void ValueChangedHandler();

        public event ValueChangedHandler OnValueChanged;

        private T _value;

        public T Value
        {
            get { return _value; }
            set
            {
                if (!object.Equals(_value, value))
                {
                    T old = _value;
                    _value = value;
                    if (OnValueChanged != null)
                    {
                        OnValueChanged();
                    }
                }
            }
        }

        public override string ToString()
        {
            return (Value != null ? Value.ToString() : "null");
        }
    }
}