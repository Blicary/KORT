using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour
{
    //public CharAimInfoHub info_hub_aim;
    public CharMoveInfoHub info_hub_move;


    // general
    public string name = "no name";
    private bool alive = true;
    public bool weak = false; // a weak character will die in one hit (no stun)
    public bool invulnerable = false; // cannot be knocked back or stunned or killed when hit

    // damage
    private bool stunned = false;
    public float stun_duration = 0.5f; // seconds duration of being stunned
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

        info_hub_move.KnockBack(force);
    }
    /// <summary>
    /// Instantly kill, no stun
    /// </summary>
    /// <param name="force"></param>
    public void HitPowerful(Vector2 force)
    {
        if (invulnerable) return;

        if (alive) Kill();

        info_hub_move.KnockBack(force);
    }


    // PRIVATE MODIFIERS

    private void Stun()
    {
        stunned = true;
        recover_timer = stun_duration;
        SendMessage("OnStun", SendMessageOptions.DontRequireReceiver);
    }
    private void UnStun()
    {
        stunned = false;
        SendMessage("OnUnStun", SendMessageOptions.DontRequireReceiver);
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
