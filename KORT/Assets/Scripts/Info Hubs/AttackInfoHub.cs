using UnityEngine;
using System.Collections;

public class AttackInfoHub : MonoBehaviour
{
    // Weapon Relay Variables
    public ArrayList weapon_list = new ArrayList();
    public int chosen_weapon = 0;
    
    // Weapon Relay Functions
    public void Attack()
    { 
        /// This function is called by the object that carries this script.
        /// Then it calls the RunAttack() script for the weapon that is 
        /// currently selected.
       
        // Use events to call a RunAttack() from the active weapon.


    }

    public void SwitchWeapon()
    {
        /// This function is called by the object that carries this script.
        /// It then rotates which weapon will be have the RunAttack() script 
        /// called when Attack() is called.

    }

    // ACCESSORS
    public string GetWeapon()
    {
        return "NotAWeapon";
    }
}
