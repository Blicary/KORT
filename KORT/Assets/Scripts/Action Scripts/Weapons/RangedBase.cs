using UnityEngine;
using System.Collections;

public abstract class RangedBase : WeaponBase 
{
    // Combat variables
    private GameObject bolt;
    // Override RunAttack() from WeaponBase


    // Helper functions for run attack
    private void HandleBoltInstantiation()
    { 
        /// This is the function that is responcible for instantiating a 
        /// bolt object, and giving that object a direction to travel in.
        /// Any other information that needs to be GIVEN to bolts, should
        /// be imparted to them here.
        // Debug.Log("Instantiate Bolt");
    }

    private void HandleAnimation()
    {
        /// This is the function that is responsible for determining what
        /// objects have been hit by the attack and telling the objects
        /// they've been hit by the attack and should do some sort of 
        /// damage thing.
        // Debug.Log("Animate Ranged");
    }

	// Use this for initialization
	void Start () 
    {
        base.Start();
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}
}
