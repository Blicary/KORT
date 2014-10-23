using UnityEngine;
using System.Collections;
using System;

public delegate void SetAimDirectionEventHandler(object sender, EventArgs e);


public class CharAimInfoHub : MonoBehaviour 
{
    private Vector2 aim_direction;
    private float aim_rotation;

    public event EventHandler<EventArgs<Vector2>> event_set_aim_direction;
    public event EventHandler<EventArgs<float>> event_set_aim_rotation;


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

    public void InformAimDirection(Vector2 aim_direction)
    {
        this.aim_direction = aim_direction;
    }
    public void InformAimRotation(float aim_rotation)
    {
        this.aim_rotation = aim_rotation;
    }

    public void SetAimDirection(Vector2 direction)
    {
        event_set_aim_direction(this, new EventArgs<Vector2>(direction));
    }
    public void SetAimRotation(float rotation)
    {
        event_set_aim_rotation(this, new EventArgs<float>(rotation));
    }
}
