using UnityEngine;
using System.Collections;

public abstract class WeaponBase : MonoBehaviour 
{
    // references
    protected AttackInfoHub attack_info_hub;
    protected CharAimInfoHub aim_info_hub;


    // general
    public abstract string WeaponName { get; }

    // time between two attacks (time before another attack can be made) - defines weapon speed
    protected float attack_duration = 0.5f;
    // time of the most recent attack (when input was received)
    protected float attack_start_time = 0f;

    // animation
    public SpriteRenderer animation_renderer; // on info hub?...
    protected SpriteAnimator animator;
    


    /// <summary>
    /// Make sure to set attack_duration before calling the original function.
    /// </summary>
    public void Start()
    {
        attack_info_hub = transform.parent.GetComponentInChildren<AttackInfoHub>();
        if (!attack_info_hub) Debug.LogWarning("Missing attack info hub");

        aim_info_hub = transform.parent.GetComponentInChildren<CharAimInfoHub>();
        if (!aim_info_hub) Debug.LogWarning("Missing aim info hub");


        animator = new SpriteAnimator(animation_renderer, attack_duration);
    }

    public void Update()
    {
        if (animator.Update(Time.deltaTime)) OnAnimationEnd();
    }

    /// <summary>
    /// Will attack if allowed.
    /// </summary>
    public void Attack()
    {
        if (CanAttack()) HandleAttack();
    }

    /// <summary>
    /// Called by WeaponBase when an attack is commanded and CanAttack.
    /// When overiding, be sure the call the original.
    /// </summary>
    protected virtual void HandleAttack()
    {
        attack_start_time = Time.time;
    }

    /// <summary>
    /// Has there been enough time since the last attack, etc.
    /// If overiding, be sure to use the return of the original.
    /// </summary>
    /// <returns></returns>
    protected virtual bool CanAttack()
    {
        return (Time.time - attack_start_time) > attack_duration;
    }

    protected virtual void OnAnimationEnd() { }

}
