using UnityEngine;
using System.Collections;

public class TestMelee : MeleeBase 
{
    public override string WeaponName { get { return "Test Melee"; } }

    protected override float AttackDuration { get { return 0.75f; } }
    protected override float TimeOfCollision { get { return 0.25f; } }


    public TestMelee() : base()
    {
 
    }
	
}
