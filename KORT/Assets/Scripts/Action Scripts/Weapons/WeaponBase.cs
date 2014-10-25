using UnityEngine;
using System.Collections;

public class WeaponBase : MonoBehaviour 
{
    // Minimum time a player must wait between successive attacks
    //  with this weapon.
    public float time_between_attack = 1.0f;
    public CharAimInfoHub AimScript;
    public string weapon_name = "Weapon Base";

    // Time that has passed since the player last actually used 
    //  this attack.
    public float last_attack = 0f;

    // Run Attack
    virtual public void RunAttack()
    {
        /// This method should ALWAYS be overrided by a child class
        /// in implementation.
        /// 
        // Debug.Log("weapon base runattack");
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
