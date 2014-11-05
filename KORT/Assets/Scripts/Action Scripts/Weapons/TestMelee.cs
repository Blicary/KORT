using UnityEngine;
using System.Collections;

public class TestMelee : MeleeBase 
{
    public override string WeaponName { get { return "Test Melee"; } }


    public new void Start()
    {
        base.attack_duration = 0.5f;
        base.time_of_collision = 0.25f;
        
        base.Start();
    }
	
}
