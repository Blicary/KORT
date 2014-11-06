using UnityEngine;
using System.Collections;
using System;

public class Character : MonoBehaviour
{
    // references
    private CharMoveInfoHub move_info_hub;

    // events
    public event EventHandler<EventArgs> event_on_stun, event_on_unstun;

    // general
    //public string character_name = "no name";
    private bool alive = true;
    public bool weak = false; // a weak character will die in one hit (no stun)
    public bool invulnerable = false; // cannot be knocked back or stunned or killed when hit

    // damage
    private bool stunned = false;
    public float stun_duration = 0.5f; // seconds duration of being stunned
    private float recover_timer = 0; // seconds time before recover from stun


    // PUBLIC MODIFIERS

    public void Awake()
    {
        move_info_hub = GetComponentInChildren<CharMoveInfoHub>();
        if (!move_info_hub) Debug.LogError("Missing CharMoveInfoHub component");
    }

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

    /// <summary>
    /// Stun a non stunned character,
    /// Kill a stunned or weak character
    /// </summary>
    /// <param name="force"></param>
    public void Hit(Vector2 force, bool can_kill)
    {
        if (invulnerable) return;

        if (alive)
        {
            if (can_kill && (stunned || weak)) Kill();
            else Stun();
        }

        move_info_hub.KnockBack(force);
    }
    /// <summary>
    /// Instantly kill, no stun
    /// </summary>
    /// <param name="force"></param>
    public void HitPowerful(Vector2 force)
    {
        if (invulnerable) return;

        if (alive) Kill();

        move_info_hub.KnockBack(force);
    }


    // PRIVATE MODIFIERS

    private void Stun()
    {
        stunned = true;
        recover_timer = stun_duration;
        if (event_on_stun != null) event_on_stun(this, new EventArgs());
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
