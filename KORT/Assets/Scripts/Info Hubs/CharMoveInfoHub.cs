using UnityEngine;
using System.Collections;
using System;

public delegate void SetVelocityEventHandler(object sender, EventArgs e);


public class CharMoveInfoHub : MonoBehaviour 
{
    private Vector2 velocity, velocity_last;

    public event SetVelocityEventHandler event_set_velocity;


    // PUBLIC ACCESSORS

    public Vector2 GetVelocity()
    {
        return Vector2.zero;
    }
    public Vector2 GetVelocityLastFrame()
    {
        return Vector2.zero;
    }


    // PUBLIC MODIFIERS

    public void InformVelocity(Vector2 velocity)
    {
        this.velocity = velocity; 
    }
    public void InformVelocityLastFrame(Vector2 velocity_last)
    {
        this.velocity_last = velocity_last;
    }
    
    public void SetVelocity(Vector2 velocity)
    {
        event_set_velocity(this, new GeneralHelpers.SingleEventArg<Vector2>(velocity));
    }

}
