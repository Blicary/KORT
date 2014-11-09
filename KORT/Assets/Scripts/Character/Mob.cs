using UnityEngine;
using System.Collections;

public class Mob : Character
{
    // the portcullis which the mob came from
    public Portcullis associated_port;


    protected override void Kill()
    {
        associated_port.RecordMobDeath();
        base.Kill();
    }
}
