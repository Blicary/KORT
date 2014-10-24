using UnityEngine;
using System.Collections;
using System;

public delegate void SetAimDirectionEventHandler(object sender, EventArgs e);


public class CharAimInfoHub : MonoBehaviour 
{
    private Vector2 aim_direction;
    private float aim_rotation; // radians

    public event EventHandler<EventArgs<Vector2>> event_set_aim_with_direction;
    public event EventHandler<EventArgs<float>> event_set_aim_with_rotation;


    // PUBLIC ACCESSORS

    public Vector2 GetAimDirection()
    {
        return aim_direction;
    }
    public float GetAimRotation()
    {
        return aim_rotation;
    }


    // PUBLIC MODIFIERS

    /// <summary>
    /// inform the info hub of only the direction of aim
    /// </summary>
    /// <param name="aim_direction"></param>
    public void InformAimDirection(Vector2 aim_direction)
    {
        this.aim_direction = aim_direction;
    }
    /// <summary>
    /// inform the info hub of only the rotation (in radians) of aim
    /// </summary>
    /// <param name="aim_rotation"></param>
    public void InformAimRotation(float aim_rotation)
    {
        this.aim_rotation = aim_rotation;
    }

    public void SetAim(Vector2 direction)
    {
        if (event_set_aim_with_direction != null) event_set_aim_with_direction(this, new EventArgs<Vector2>(direction));
    }
    public void SetAim(float rotation)
    {
        if (event_set_aim_with_rotation != null) event_set_aim_with_rotation(this, new EventArgs<float>(rotation));
    }
}
