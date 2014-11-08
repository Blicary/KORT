using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(CharMoveInfoHub))]

public class Character : MonoBehaviour
{
    // events
    public event EventHandler<EventArgs<Vector2>> event_stun;
    public event EventHandler<EventArgs> event_on_unstun;

    // general
    public bool player_controlled = false;
    public bool weak = false; // a weak character will die in one hit (no stun)
    public bool invulnerable = false; // cannot be knocked back or stunned or killed when hit
    public bool can_block = false;

    private bool alive = true;


    // damage
    private float stun_duration = 0.5f; // seconds duration of being stunned
    private float mini_stun_duration = 0.25f;
    private bool stunned = false;

    private float recover_timer = 0; // seconds time before recover from stun



    // PUBLIC MODIFIERS

    public void Update()
    {
        // stunned
        if (stunned)
        {
            recover_timer -= Time.deltaTime;
            if (recover_timer <= 0) UnStun();
        }


        // dying
        if (!alive)
        {
            GameObject.Destroy(gameObject);
        }
        
    }

    public void Hit(Vector2 force, bool can_kill)
    {
        if (invulnerable) return;

        if (alive)
        {
            if (can_kill && (stunned || weak)) Kill();
            else Stun(force);
        }
    }
    public void MiniHit(Vector2 force)
    {
        if (invulnerable) return;

        MiniStun(force);
    }
    public void HitPowerful(Vector2 force)
    {
        if (invulnerable) return;

        if (alive) Kill();
    }


    // PRIVATE MODIFIERS

    private void Stun(Vector2 force)
    {
        stunned = true;
        recover_timer = stun_duration;
        if (event_stun != null) event_stun(this, new EventArgs<Vector2>(force));
    }
    private void MiniStun(Vector2 force)
    {
        stunned = true;
        recover_timer = mini_stun_duration;
        if (event_stun != null) event_stun(this, new EventArgs<Vector2>(force));
    }
    private void UnStun()
    {
        stunned = false;
        if (event_on_unstun != null) event_on_unstun(this, new EventArgs());
    }
    private void Kill()
    {
        stunned = false;
        alive = false;

        if (player_controlled) GameManager.GameOver();
    }


    // PUBLIC ACCESSORS 
    
    public bool IsAlive()
    {
        return alive;
    }
    public bool IsStunned()
    {
        return stunned;
    }

}
