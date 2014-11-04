using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeleeBase : WeaponBase
{
    // Animation

    // the time in seconds it takes for the attack animation to complete
    public float animation_length = 0.2f; 
    // the time in seconds when the animation begin
    public float animation_start;        
    protected bool in_animation = false;
    public SpriteRenderer animation;


    // Collision

    // (Later there could be derived classes for swing / thrust weapons...) 
    protected float swing_radius = 9.26f; 
    protected float swing_angle_start = Mathf.PI / 4f, swing_angle_end = Mathf.PI * 3 / 4f;

    // angle between rays
    private float ray_precision = Mathf.PI / 16f;
    protected float[] ray_cast_angles; // references angles (character aiming to the right)
    
    // layer specifying which objects can be hit by this weapon (maybe have it on the infohub...)
    public LayerMask collision_layer; 
    //public GameObject character_group; // reference to the characters object in the hierarchy that we will be get the characters from.



    public void Start()
    {
        weapon_name = "Weapon Melee";
        aim_info_hub = transform.parent.GetComponentInChildren<CharAimInfoHub>();


        // collision physics preperations
        PrepareRaycastDirections();
        
    }
    private void PrepareRaycastDirections()
    {
        float total_swing_angle = swing_angle_end - swing_angle_start;
        int n = (int)Mathf.Ceil(total_swing_angle / ray_precision);

        ray_cast_angles = new float[n];
        float inter_angle = total_swing_angle / (n-1);

        for (int i = 0; i < n; ++i)
        {
            // reference angles for if the character was aim to the right
            ray_cast_angles[i] = (swing_angle_start + inter_angle * i) - (Mathf.PI / 2f);
        }
    }

    public void Update()
    {
        //DebugDrawRayCasts();

        //Debug.Log("animation");
        if (in_animation && (Time.time - animation_start) > animation_length)
        {
            in_animation = false;
            //animation.enabled = false;
            animation.color = new Color(1, 1, 1, 25f/255f);
        }
    }

    // Override RunAttack() from WeaponBase 
    public override void RunAttack()
    {
        // Check if the player has waited long enough wince their last
        //   attack with this weapon.
        if ((Time.time - last_attack) > time_between_attack)
        {
            //Debug.Log("attack" + ((Time.time - last_attack) +">"+ time_between_attack));
            // If they have, do all the stuff that needs to happen when attack
            //   is run.
            last_attack = Time.time;
            HandleAnimation();
            HandleCollision();
            
            // Debug.Log("Melee Attack with " + weapon_name);
        }
    }

    // Helper functions for run attack
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
                ray, swing_radius - attack_info_hub.weapon_start_reach, collision_layer);

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
        if (hit_character) animation.color = Color.red;
        else if (hit_terrain) animation.color = new Color(1, 0.8f, 0.1f);

    }

    private void HandleAnimation()
    { 
        /// This is the function responcible for initiating the weapon 
        /// animation.
        /// The animation is ended by the update function when it determines
        /// that the animation is over.
        // Debug.Log("Animate Melee");
        animation.enabled = true;
        in_animation = true;
        animation_start = Time.time;
        animation.color = Color.white;
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
