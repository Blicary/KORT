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
    public bool invulnerable = false; // cannot be knocked back or stunned or killed when hit
    public bool can_block = false;

    private bool alive = true;


    // health
    public int max_hit_points = 3;
    protected int hit_points;

    // stun
    private const float stun_duration = 0.5f; // seconds duration of being stunned
    private const float mini_stun_duration = 0.25f;
    private bool stunned = false;

    private float recover_timer = 0; // seconds time before recover from stun



    // PUBLIC MODIFIERS

    public void Start()
    {
        hit_points = max_hit_points;
        stunned = false;
        alive = true;
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

    public void Hit(Vector2 force, bool can_damage, Transform attacker)
    {
        if (invulnerable) return;

        if (alive)
        {
            if (can_damage)
            {
                hit_points -= 1;
                OnTakeDamage();

                if (hit_points == 0)
                {

                    Combatant com = attacker.GetComponent<Combatant>();
                    if (com != null)
                    {
                        com.RecordKill();
                        Kill(com);
                    }
                    else Kill();
                }
            }

            Stun(force);
        }
    }

    protected virtual void OnTakeDamage()
    {

    }

    // PRIVATE MODIFIERS

    public void Stun(Vector2 force)
    {
        stunned = true;
        recover_timer = stun_duration;
        if (event_stun != null) event_stun(this, new EventArgs<Vector2>(force));
    }
    public void MiniStun(Vector2 force)
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
    protected virtual void Kill()
    {
        stunned = false;
        alive = false;
    }
    protected virtual void Kill(Combatant killer)
    {
        Kill();
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
