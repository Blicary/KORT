using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum WeaponType { TestMelee, RangedMelee }

[RequireComponent(typeof(Character))]
[RequireComponent(typeof(CharAimInfoHub))]

public class AttackInfoHub : MonoBehaviour
{
    // References
    private Character character;
    private CharAimInfoHub aim_info_hub;
    public SpriteRenderer weapon_renderer;
    

    // Weapons
    public WeaponType chosen_weapon = WeaponType.TestMelee;
    private WeaponBase active_weapon = null;
    

    // Weapon collision handling

    // dist from center of character at which weapon collision checking begins
    public float weapon_start_reach = 1f;
    // layers that weapons should collide with (terrain, characters...)
    public LayerMask weapon_collision_layer; 



    // PUBLIC MODIFIERS

    public void Awake()
    {
        // get references
        character = transform.parent.GetComponent<Character>();
        aim_info_hub = GetComponent<CharAimInfoHub>();
    }
    public void Start()
    {
        // character stun event
        character.event_on_stun += new EventHandler<EventArgs>(OnCharacterStun);

        // set initial active weapon
        CreateActiveWeaponInstance();
    }

    public void Update()
    {
        active_weapon.Update();
    }

    public void Attack()
    {
        if (character.IsStunned() || !character.IsAlive()) return;

        active_weapon.Attack();
    }
    public void SwitchWeapon()
    {
        int n = Enum.GetNames(typeof(WeaponType)).Length;
        chosen_weapon = (WeaponType)(((int)chosen_weapon + 1) % n);
        CreateActiveWeaponInstance();
    }


    // PRIVATE MODIFIERS

    private void CreateActiveWeaponInstance()
    {
        WeaponType t = chosen_weapon; // to save space

        active_weapon = 
            t == WeaponType.TestMelee ? (WeaponBase)new TestMelee() :
            t == WeaponType.TestMelee ? (WeaponBase)new TestRanged() :
            null;

        active_weapon.Initialize(transform, this, aim_info_hub, weapon_renderer);
    }

    private void OnCharacterStun(object sender, EventArgs e)
    {
        active_weapon.InterruptAttack();
    }


    // PUBLIC ACCESSORS

    public WeaponBase GetActiveWeapon()
    {
        return active_weapon;
    }
}
