using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeleeBase : WeaponBase
{
    // Combat animation variables
    public float animation_length = 0.2f; // the time in seconds it takes for the
                                  //   the attack animation to complete.
    public float animation_start;        // the time in seconds when the animation
                                  //   began.
    protected bool in_animation = false;
    public SpriteRenderer animation; // the renderer with the actual animation
    
    public GameObject character_group; // reference to the characters object in the hierarchy that we will be get the characters from.

    // Override RunAttack() from WeaponBase 
    public override void RunAttack()
    {
        // Check if the player has waited long enough wince their last
        //   attack with this weapon.
        if ((Time.time - last_attack) > time_between_attack)
        {
            //Debug.Log("attack" + ((Time.time - last_attack) +">"+ time_between_attack));
            // If they have, do all the stuff that needs to happen when attack
            //   is run.
            last_attack = Time.time;
            HandleCollision();
            HandleAnimation();
            // Debug.Log("Melee Attack with " + weapon_name);
        }
    }

    // Helper functions for run attack
    private void HandleCollision()
    { 
        /// This is the function that is responsible for determining what
        /// objects have been hit by the attack and telling the objects
        /// they've been hit by the attack and should do some sort of 
        /// damage thing.
        //Debug.Log("Check for Collisions");
        
        //Debug.Log(character_array);
        foreach (Transform child in character_group.transform)
        {
            Vector2 other_position = child.position;
            float distance = Vector2.Distance((Vector2)transform.position,other_position);
            //Debug.Log(child.name + ":" + other_position + " DIS " + distance + " To Player " + (Vector2)transform.position);
            if (1f < distance && distance < 6f)
            {
                //Debug.Log("Distance: "+distance);
                //Debug.Log("  other: " + other_position);
                float angle = GeneralHelpers.AngleBetweenVectors((Vector2)transform.position, other_position);
                angle = Mathf.Rad2Deg*angle;
                float direction = aim_info_hub.GetAimRotation() * Mathf.Rad2Deg;
                Debug.Log(angle + "-" + direction);
                angle = Mathf.Abs(angle - direction);
                if ( angle < 45)
                {
                    Debug.Log("HIT "+Time.time);
                }
            }
        }

    }

    private void HandleAnimation()
    { 
        /// This is the function responcible for initiating the weapon 
        /// animation.
        /// The animation is ended by the update function when it determines
        /// that the animation is over.
        // Debug.Log("Animate Melee");
        animation.enabled = true;
        in_animation = true;
        animation_start = Time.time;
    }

	// Use this for initialization
	public void Start () 
    {
        weapon_name = "Weapon Melee";
        aim_info_hub = transform.parent.GetComponentInChildren<CharAimInfoHub>();
	}
	
	// Update is called once per frame
	public void Update () 
    {
        //Debug.Log("animation");
        if (in_animation && (Time.time - animation_start) > animation_length)
        {
            in_animation = false;
            animation.enabled = false;
        }
	}
}
