using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class MeleeBase : WeaponBase
{
    // Collision

    // time (into the animation) at which collision detection happens (when damage is done)
    protected float time_of_collision = 0;
    // whether collision handling has yet been done for the current attack
    protected bool has_collided = false; 

    // (Later there could be derived classes for swing / thrust weapons...) 
    protected float swing_radius = 9.26f; 
    protected float swing_angle_start = Mathf.PI / 4f, swing_angle_end = Mathf.PI * 3 / 4f;

    // angle between rays
    private float ray_precision = Mathf.PI / 16f;
    protected float[] ray_cast_angles; // references angles (character aiming to the right)
    



    // PUBLIC MODIFIERS

    public new void Start()
    {
        base.Start();

        // collision physics preperations
        PrepareRaycastDirections();
    }
    
    public new void Update()
    {
        base.Update();
        //DebugDrawRayCasts();

        // collision handling midway through attack animation
        if (base.animator.GetCurrentAnimationTime() >= time_of_collision && !has_collided)
        {
            HandleCollision();
            has_collided = true;
        }

        // TEMP
        if (animator.IsAnimating())
        {
            if (base.animator.GetCurrentAnimationTime() < time_of_collision)
            {
                float a = base.animator.GetCurrentAnimationTime() / time_of_collision;
                animator.renderer.color = new Color(1, 1, 1, a / 4f);
            }
            else if (base.animator.GetCurrentAnimationTime() < time_of_collision + 0.1f)
            {
                Color c = animator.renderer.color;
                animator.renderer.color = new Color(c.r, c.g, c.b, 1);
            }
            else
            {
                float a = 1 - (base.animator.GetCurrentAnimationTime() - time_of_collision) / (base.animator.GetDuration() - time_of_collision);
                Color c = animator.renderer.color;
                animator.renderer.color = new Color(c.r, c.g, c.b, a / 4f);
            }
            
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

        has_collided = false;
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

        HashSet<Collider2D> all_colliders = new HashSet<Collider2D>();

        // Ray cast
        for (int i = 0; i < ray_cast_angles.Length; ++i)
        {
            float a = ray_cast_angles[i] + aim_info_hub.GetAimRotation();
            Vector2 ray = new Vector2(Mathf.Cos(a), Mathf.Sin(a));

            RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position + ray * attack_info_hub.weapon_start_reach,
                ray, swing_radius - attack_info_hub.weapon_start_reach, attack_info_hub.weapon_collision_layer);

            if (hit) all_colliders.Add(hit.collider);
        }


        // Act on hit objects
        bool hit_character = false;
        bool hit_terrain = false;

        foreach (Collider2D col in all_colliders)
        {
            Character c = col.GetComponent<Character>();
            if (c)
            {
                hit_character = true;
                Vector2 dir = (col.transform.position - transform.position).normalized;
                c.Hit(dir * 30f, true);
            }
            else
            {
                hit_terrain = true;
            }
        }


        // TEMP
        if (hit_character) animator.renderer.color = Color.red;
        else if (hit_terrain) animator.renderer.color = new Color(1, 0.8f, 0.1f);
    }
    private void HandleAnimation()
    { 
        /// This is the function responcible for initiating the weapon 
        /// animation.
        /// The animation is ended by the update function when it determines
        /// that the animation is over.
        // Debug.Log("Animate Melee");

        animator.BeginAnimation();
    }


	private void DebugDrawRayCasts()
    {
        for (int i = 0; i < ray_cast_angles.Length; ++i)
        {
            float a = ray_cast_angles[i] + aim_info_hub.GetAimRotation();
            Vector2 ray = new Vector2(Mathf.Cos(a), Mathf.Sin(a));
            Debug.DrawLine((Vector2)transform.position + ray * attack_info_hub.weapon_start_reach, (Vector2)transform.position + ray * swing_radius);
        }
    }

}
