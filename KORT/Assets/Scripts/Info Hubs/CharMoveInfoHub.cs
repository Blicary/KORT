using UnityEngine;
using System.Collections;
using System;


public class CharMoveInfoHub : MonoBehaviour 
{
    private Vector2 velocity, velocity_last;

    public event EventHandler<EventArgs<Vector2>> event_set_velocity;
    public event EventHandler<EventArgs<Vector2>> event_knockback;


    // PUBLIC ACCESSORS

    public Vector2 GetVelocity()
    {
        return velocity;
    }
    public Vector2 GetVelocityLastFrame()
    {
        return velocity_last;
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
        if (event_set_velocity != null) event_set_velocity(this, new EventArgs<Vector2>(velocity));
    }
    public void KnockBack(Vector2 force)
    {
        if (event_knockback != null) event_knockback(this, new EventArgs<Vector2>(force));
    }

}
