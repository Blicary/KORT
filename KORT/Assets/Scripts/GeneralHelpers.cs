using UnityEngine;
using System.Collections;
using System;


public class EventArgs<T> : EventArgs
{
    public T Value { get; private set; }

    public EventArgs(T val)
    {
        Value = val;
    }
}

public class GeneralHelpers 
{
    public static Vector2 Perpendicular(Vector2 v)
    {
        return new Vector2(v.y, -v.x);
    }
}
