using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class AttackInfoHub : MonoBehaviour
{
    // references
    public Character character;
    public CharAimInfoHub aim_info_hub;
    public GameObject weapons_object;
    

    // weapons
    private WeaponBase[] weapons_list;
    public int chosen_weapon = 0;
    private WeaponBase active_weapon = null;

    // weapon collision handling

    // dist from center of character at which weapon collision checking begins
    public float weapon_start_reach = 1f;
    // layers that weapons should collide with (terrain, characters...)
    public LayerMask weapon_collision_layer; 



    // PUBLIC MODIFIERS

    public void Awake()
    {
        // setup weapons list
        if (weapons_object == null)
        {
            Debug.LogWarning("No weapon manager specified.");
            weapons_list = new WeaponBase[0];
        }
        else
        {
            weapons_list = weapons_object.GetComponents<WeaponBase>();
        }
    }
    public void Start()
    {
        character.event_on_stun += new EventHandler<EventArgs>(OnCharacterStun);

        // set initial active weapon
        if (weapons_list.Length > 0) active_weapon = weapons_list[chosen_weapon];
    }

    public void Attack()
    {
        if (character.IsStunned() || !character.IsAlive() || active_weapon == null) return;

        active_weapon.Attack();
    }
    public void SwitchWeapon()
    {
        // active weapon remains null if there are no weapons available
        if (weapons_list.Length == 0) return;


        chosen_weapon = (chosen_weapon + 1) % weapons_list.Length;
        active_weapon = weapons_list[chosen_weapon];
    }


    // PRIVATE MODIFIERS

    private void OnCharacterStun(object sender, EventArgs e)
    {
        if (!active_weapon) return;

        active_weapon.InterruptAttack();
    }


    // PUBLIC ACCESSORS

    public WeaponBase GetActiveWeapon()
    {
        return active_weapon;
    }
}
