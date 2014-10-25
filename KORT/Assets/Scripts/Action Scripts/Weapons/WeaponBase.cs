using UnityEngine;
using System.Collections;

public class WeaponBase : MonoBehaviour 
{
    // Minimum time a player must wait between successive attacks
    //  with this weapon.
    public float time_between_attack = 1.0f;

    // Time that has passed since the player last actually used 
    //  this attack.
    public float last_attack = 0f;

    // Run Attack
    abstract public void RunAttack()
    {
        /// This method should ALWAYS be overrided by a child class
        /// in implementation.
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
