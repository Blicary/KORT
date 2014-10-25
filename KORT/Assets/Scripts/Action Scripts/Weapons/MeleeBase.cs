using UnityEngine;
using System.Collections;

public class MeleeBase : WeaponBase
{
    // Override RunAttack() from WeaponBase 
    public void RunAttack()
    {
        // Check if the player has waited long enough wince their last
        //   attack with this weapon.
        if ((Time.time - last_attack) > time_between_attack)
        {
            // If they have, do all the stuff that needs to happen when attack
            //   is run.
            Debug.Log("Melee Attack with " + weapon_name);
            HandleCollision();
            HandleAnimation();
            last_attack = Time.time;
        }
    }

    // Helper functions for run attack
    private void HandleCollision()
    { 
        /// This is the function that is responsible for determining what
        /// objects have been hit by the attack and telling the objects
        /// they've been hit by the attack and should do some sort of 
        /// damage thing.
        Debug.Log("Check for Collisions");
    }

    private void HandleAnimation()
    { 
        /// This is the function responcible for initiating the weapon 
        /// animation.
        Debug.Log("Animate Melee");
    }

	// Use this for initialization
	void Start () 
    {
        weapon_name = "Weapon Melee";
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}
}
