using UnityEngine;
using System.Collections;

public class MeleeBase : MonoBehaviour {

    // Combat times
    public float time_between_melee = 1.0f;

    // Combat variables
    private float last_melee = 0f;

    // Run Attack
    public void RunAttack()
    {
        // Check if the player has waited long enough wince their last
        //   attack with this weapon.
        if ((Time.time - last_melee) > time_between_melee)
        {
            // If they have, do all the stuff that needs to happen when attack
            //   is run.
            HandleCollision();
            HandleAnimation();
            last_melee = Time.time;
        }
    }

    // Helper functions for run attack
    private void HandleCollision()
    { 
        /// This is the function that is responsible for determining what
        /// objects have been hit by the attack and telling the objects
        /// they've been hit by the attack and should do some sort of 
        /// damage thing.
    }

    private void HandleAnimation()
    { 
        /// This is the function responcible for initiating the weapon 
        /// animation.
    }

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}
}
