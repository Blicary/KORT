using UnityEngine;
using System.Collections;

public class AttackHandler : MonoBehaviour
{
    float since_m_attack;           // time since the last attempt to attack with melee.
    bool can_m_attack;              // keep track of whether the player can use the attack at this time.
    float time_between_melee;       // time required for one attack to execute and another ot start.

    public void start()
    {
        // Initialize this stuff so that there won't be a crash and
        //    the player will be able to attack the first time.
        since_m_attack = Time.time;
        can_m_attack = true;
    }

    public void update()
    { 
        // check whether the player is locked out of their melee attack and restore it if
        //   enough time has passed.
    }

    public void AttemptAttack()
    { 
        
    }
}
