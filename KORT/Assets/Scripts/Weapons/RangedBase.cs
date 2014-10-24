using UnityEngine;
using System.Collections;

public class RangedBase : MonoBehaviour {

    // Combat times
    public float time_between_ranged = 1.0f;

    // Combat variables
    private GameObject bolt;
    private float last_ranged = 0f;
    private bool can_ranged = true;

    public void RunAttack()
    { 
        // Check if the player has waited long enough wince their last
        //   attack with this weapon.
        if ((Time.time - last_ranged) > time_between_ranged)
        { 
            // If they have, do all the stuff that needs to happen when attack 
            //   is run.
            HandleBoltInstantiation();
            HandleAnimation();
            last_ranged = Time.time;
        }
    }

    // Helper functions for run attack
    private void HandleBoltInstantiation()
    { 
        /// This is the function that is responcible for instantiating a 
        /// bolt object, and giving that object a direction to travel in.
        /// Any other information that needs to be GIVEN to bolts, should
        /// be imparted to them here.
    }

    private void HandleAnimation()
    {
        /// This is the function that is responsible for determining what
        /// objects have been hit by the attack and telling the objects
        /// they've been hit by the attack and should do some sort of 
        /// damage thing.
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
