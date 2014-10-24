using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour
{
    //public CharAimInfoHub info_hub_aim;
    public CharMoveInfoHub info_hub_move;


    // general
    private bool alive = true;
    public bool invulnerable = false;

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

    public void Hit(Vector2 force)
    {
        if (invulnerable) return;
        info_hub_move.KnockBack(force);

        if (!alive) return;
        if (stunned) Kill();
        else Stun();
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
