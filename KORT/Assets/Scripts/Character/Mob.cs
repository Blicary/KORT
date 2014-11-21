using UnityEngine;
using System.Collections;

public class Mob : Character
{
    // the portcullis which the mob came from
    public Portcullis associated_port;
    public TextMesh text_message_prefab;


    protected override void Kill(Combatant killer)
    {
        // only record the mob death if killed by a combatant
        associated_port.RecordMobDeath();

        TextMesh tm = (TextMesh)Instantiate(text_message_prefab, transform.position, Quaternion.identity);
        House killer_house = HouseManager.GetHouse(killer.house_name);
        tm.text = killer_house.KillsCurrentArena + " / " + ArenaDetails.GetRequiredKills() + " KILLED";
        //tm.color = Color.green;

        base.Kill();
    }
    protected override void Kill()
    {
        // record death by non combatant
        associated_port.RecordMobDeathToNonCombatant();
        base.Kill();
    }
}
