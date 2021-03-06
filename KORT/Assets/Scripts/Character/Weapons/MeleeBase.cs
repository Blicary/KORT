﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class MeleeBase : WeaponBase
{
    // Collision

    // time (into the animation) at which collision detection happens (when damage is done)
    protected abstract float TimeOfCollision { get; }

    // whether collision handling has yet been done for the current attack
    protected bool has_checked_collision = false;
    private bool hit_character = false, hit_terrain = false;

    // (Later there could be derived classes for swing / thrust weapons...) 
    protected float swing_radius = 7.39f; 
    protected float swing_angle_start = Mathf.PI / 4f, swing_angle_end = Mathf.PI * 3 / 4f;

    // angle between rays
    private float ray_precision = Mathf.PI / 16f;
    private float[] ray_cast_angles; // references angles (character aiming to the right)
    
    // blocking
    private bool is_blocking = false;
    protected float block_duration = 0.3f;

    // knockback
    protected float block_knock_back = 3;
    protected float hit_knock_back = 18;



    // PUBLIC MODIFIERS

    public MeleeBase() : base()
    {
        
        // collision physics preperations
        PrepareRaycastDirections();
    }
    
    public override void Update()
    {
        base.Update();
        //DebugDrawRayCasts();

        // collision handling midway through attack animation
        if (!has_checked_collision && base.tempanimator.GetCurrentAnimationTime() >= TimeOfCollision)
        {
            HandleCollision();
            has_checked_collision = true;
        }

        // TEMP
        if (IsAttacking() && !is_blocking)
        {
            if (base.tempanimator.GetCurrentAnimationTime() < TimeOfCollision)
            {
                float a = base.tempanimator.GetCurrentAnimationTime() / TimeOfCollision;
                tempanimator.renderer.color = new Color(1, 1, 1, a / 4f);
            }
            else if (base.tempanimator.GetCurrentAnimationTime() < TimeOfCollision + 0.1f)
            {
                Color c = tempanimator.renderer.color;
                tempanimator.renderer.color = new Color(c.r, c.g, c.b, 1);
            }
            else
            {
                float a = 1 - (base.tempanimator.GetCurrentAnimationTime() - TimeOfCollision) / (base.tempanimator.GetDuration() - TimeOfCollision);
                Color c = tempanimator.renderer.color;
                tempanimator.renderer.color = new Color(c.r, c.g, c.b, a / 4f);
            }
            
        }
    }

    public override void InterruptAttack()
    {
        if (is_blocking)
        {
            AttackTimeLeft = block_duration;
            has_checked_collision = true;
        }
        else
        {
            base.InterruptAttack();
        }
    }

    
    // PRIVATE / PROTECTED MODIFIERS

    private void PrepareRaycastDirections()
    {
        float total_swing_angle = swing_angle_end - swing_angle_start;
        int n = (int)Mathf.Ceil(total_swing_angle / ray_precision);

        ray_cast_angles = new float[n];
        float inter_angle = total_swing_angle / (n - 1);

        for (int i = 0; i < n; ++i)
        {
            // reference angles for if the character was aim to the right
            ray_cast_angles[i] = (swing_angle_start + inter_angle * i) - (Mathf.PI / 2f);
        }
    }

    protected override void OnAnimationEnd()
    {
        base.OnAnimationEnd();

        has_checked_collision = false;
        is_blocking = false;
        hit_character = false;
        hit_terrain = false;
    }

    protected override void HandleAttack()
    {
        base.HandleAttack();

        HandleAnimation();
    }
    
    private void HandleCollision()
    { 
        /// This is the function that is responsible for determining what
        /// objects have been hit by the attack and telling the objects
        /// they've been hit by the attack and should do some sort of 
        /// damage thing.
        //Debug.Log("Check for Collisions");

        HashSet<Collider2D> all_colliders = GetCollidedObjects();

        // Act on hit objects
        foreach (Collider2D col in all_colliders)
        {
            Character c = col.GetComponent<Character>();
            if (c)
            {
                // check for block
                AttackInfoHub other_attack = c.GetComponent<AttackInfoHub>();
                if (other_attack != null)
                {
                    MeleeBase other_weapon = (MeleeBase)other_attack.GetActiveWeapon();
                    if (other_weapon != null)
                    {
                        if (other_weapon.CanBlockAttack(c.collider2D))
                        {
                            is_blocking = true;
                            other_weapon.is_blocking = true;
                            tempanimator.renderer.color = new Color(0.9f, 1f, 1f, 0.5f);
                            other_weapon.tempanimator.renderer.color = new Color(0.9f, 1f, 1f, 0.5f);

                            // stun
                            Vector2 dir = (col.transform.position - owner.transform.position).normalized;
                            c.MiniStun(dir * block_knock_back);
                            owner.MiniStun(-dir * block_knock_back);

                            return; // don't hit anything else, there was a block
                        }
                    }
                }
                
                // hit
                hit_character = true;
                Vector2 dir2 = (col.transform.position - owner.transform.position).normalized;
                c.Hit(dir2 * hit_knock_back, true, owner.transform);
            }
            else
            {
                hit_terrain = true;
            }
        }


        // TEMP
        if (hit_character)
        {
            tempanimator.renderer.color = Color.red;
            SoundManager.MakeSoundSwordKill(owner.transform.position);
        }
        else if (hit_terrain)
        {
            tempanimator.renderer.color = new Color(1, 0.8f, 0.1f);
            SoundManager.MakeSoundSwordHit(owner.transform.position);
        }
        else
        {
            SoundManager.MakeSoundSwordMiss(owner.transform.position);
        }
    }
    private HashSet<Collider2D> GetCollidedObjects()
    {
        HashSet<Collider2D> all_colliders = new HashSet<Collider2D>();

        // Ray cast
        for (int i = 0; i < ray_cast_angles.Length; ++i)
        {
            float a = ray_cast_angles[i] + aim_info_hub.GetAimRotation();
            Vector2 ray = new Vector2(Mathf.Cos(a), Mathf.Sin(a));

            RaycastHit2D hit = Physics2D.Raycast((Vector2)owner.transform.position + ray * attack_info_hub.weapon_start_reach,
                ray, swing_radius - attack_info_hub.weapon_start_reach, attack_info_hub.weapon_collision_layer);

            if (hit) all_colliders.Add(hit.collider);
        }

        return all_colliders;
    }

    private void HandleAnimation()
    { 
        /// This is the function responcible for initiating the weapon 
        /// animation.
        /// The animation is ended by the update function when it determines
        /// that the animation is over.
        // Debug.Log("Animate Melee");

        tempanimator.BeginAnimation();
    }


	private void DebugDrawRayCasts()
    {
        for (int i = 0; i < ray_cast_angles.Length; ++i)
        {
            float a = ray_cast_angles[i] + aim_info_hub.GetAimRotation();
            Vector2 ray = new Vector2(Mathf.Cos(a), Mathf.Sin(a));
            Debug.DrawLine((Vector2)owner.transform.position + ray * attack_info_hub.weapon_start_reach, (Vector2)owner.transform.position + ray * swing_radius);
        }
    }


    // PUBLIC ACCESSORS

    /// <summary>
    /// Can this weapon block an attacking melee weapon on the object whose collider is specified?
    /// </summary>
    /// <param name="collider"></param>
    /// <returns></returns>
    public bool CanBlockAttack(Collider2D other)
    {
        if (!owner.can_block) return false; // type of character cannot block
        if (!IsAttacking()) return false; // not attacking


        //return true;
        //HashSet<Collider2D> colliders = GetCollidedObjects();
        return !hit_character;


        /*
        if (is_blocking || !has_checked_collision)
        {

            return true;
        }
        
        return false;
         * */
    }

}
