using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class AttackInfoHub : MonoBehaviour
{
    // Weapon Relay Variables
    public WeaponBase[] weapon_list;
    public int weapon_count;
    public int chosen_weapon = 0;
    
    // Weapon Relay Functions
    public void Attack()
    {
        /// This function is called by the object that carries this script.
        /// Then it calls the RunAttack() script for the weapon that is 
        /// currently selected.

        if (weapon_count == 0)
        {
            // if the character has no weapon, don't do the rest of the method.
            return;
        }

        WeaponBase active_weapon = (WeaponBase) weapon_list[chosen_weapon];
        active_weapon.RunAttack();

    }

    public void SwitchWeapon()
    {
        /// This function is called by the object that carries this script.
        /// It then rotates which weapon will be have the RunAttack() script 
        /// called when Attack() is called.

        if (weapon_count == 0)
        {
            // if the character has no weapon, don't do the rest of the method.
            return;
        }

        // Cycle to the next weapon.
        if (chosen_weapon < weapon_count)
        {
            chosen_weapon += 1;
        }
        else
        {
            chosen_weapon = 0;
        }

    }

    // ACCESSORS
    public string GetWeapon()
    {
        return "NotAWeapon";
    }

    // Unity Functions
    public void start()
    {
        if (weapon_count != 0)
        { 
            weapon_list = new WeaponBase[weapon_count];
        }
    }
}
