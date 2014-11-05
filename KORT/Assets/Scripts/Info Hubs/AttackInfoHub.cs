using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class AttackInfoHub : MonoBehaviour
{
    // dist from center of character at which weapon collision checking begins
    public float weapon_start_reach = 1f;
    // layers that weapons should collide with (terrain, characters...)
    public LayerMask weapon_collision_layer; 

    // Weapon Relay Variables
    public GameObject weapon_manager;
    private WeaponBase[] weapon_list;
    public int chosen_weapon = 0;


    public void Awake()
    {
        if (weapon_manager == null)
        {
            Debug.LogWarning("No weapon manager specified.");
            weapon_list = new WeaponBase[0];
        }
        else
        {
            weapon_list = weapon_manager.GetComponents<WeaponBase>();
        }
    }

    // Weapon Relay Functions
    public void Attack()
    {
        /// This function is called by the object that carries this script.
        /// Then it calls the RunAttack() script for the weapon that is 
        /// currently selected.

        if (weapon_list.Length == 0)
        {
            // if the character has no weapon, don't do the rest of the method.
            return;
        }
        
        WeaponBase active_weapon = (WeaponBase) weapon_list[chosen_weapon];
        // Debug.Log("Attack with (" + chosen_weapon + ") " + active_weapon);
        active_weapon.Attack();
    }

    public void SwitchWeapon()
    {
        /// This function is called by the object that carries this script.
        /// It then rotates which weapon will be have the RunAttack() script 
        /// called when Attack() is called.

        if (weapon_list.Length == 0)
        {
            // if the character has no weapon, don't do the rest of the method.
            return;
        }

        // Cycle to the next weapon.
        if (chosen_weapon < (weapon_list.Length-1) )
        {
            chosen_weapon += 1;
        }
        else
        {
            chosen_weapon = 0;
        }

        // comment these lines out in non-debug builds:
        WeaponBase active_weapon = (WeaponBase)weapon_list[chosen_weapon];
        // Debug.Log( "Switched Weapon to (" + chosen_weapon + ") " + active_weapon );

    }

    // ACCESSORS
    public string GetWeapon()
    {
        return "NotAWeapon";
    }

    
}
