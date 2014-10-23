using UnityEngine;
using System.Collections;
using System;


public class GeneralHelpers 
{
    public class SingleEventArg<T> : EventArgs
    {
        public T Value { get; private set; }

        public SingleEventArg(T val)
        {
            Value = val;
        }
    }
}
