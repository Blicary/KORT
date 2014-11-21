using UnityEngine;
using System.Collections;

public class Mob : Character
{
    // the portcullis which the mob came from
    public Portcullis associated_port;


    protected override void Kill(Combatant killer)
    {
        // only record the mob death if killed by a combatant
        associated_port.RecordMobDeath();
        base.Kill();
    }
    protected override void Kill()
    {
        // record death by non combatant
        associated_port.RecordMobDeathToNonCombatant();
        base.Kill();
    }
}
