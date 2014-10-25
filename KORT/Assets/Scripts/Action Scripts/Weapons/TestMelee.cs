using UnityEngine;
using System.Collections;

public class TestMelee : MeleeBase 
{

	// Use this for initialization
	void Start () 
    {
        weapon_name = "Test Weapon Melee";
	}
	
	// Update is called once per frame
	void Update () 
    {
        /// Delete this if you are doing your own animation management here
        base.Update();
	}
}
