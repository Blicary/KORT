using UnityEngine;
using System.Collections;

public class TestRanged : RangedBase 
{
    public override string WeaponName { get { return "Test Ranged"; } }

    protected override float AttackDuration { get { return 0.5f; } }
}
