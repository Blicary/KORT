using UnityEngine;
using System.Collections;
using UnityEngine;


public class Combatant : Character
{
    public bool player_controlled = false;
    public HouseName house_name;


    public void RecordKill()
    {
        HouseManager.RecordKill(house_name);
    }

    protected override void Kill()
    {
        base.Kill();
        HouseManager.RecordDeath(house_name, this);

        // replace with dead body and destroy
        Destroy(gameObject);
    }
}
