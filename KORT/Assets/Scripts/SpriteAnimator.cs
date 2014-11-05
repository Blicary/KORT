using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class SpriteAnimator
{
    public SpriteRenderer renderer;
    private float duration = 1; // duration in seconds
    private float current_anim_time = 0;
    private bool animating;


    public SpriteAnimator(SpriteRenderer renderer, float duration)
    {
        this.duration = duration;
        this.renderer = renderer;
    }

    /// <summary>
    /// Returns true if the animation ends
    /// </summary>
    /// <param name="delta_time"></param>
    /// <returns></returns>
    public bool Update(float delta_time)
    {
        if (animating)
        {
            current_anim_time += delta_time;

            if (current_anim_time >= duration)
            {
                StopAnimation();
                return true;
            }
        }
        return false;
    }

    public void BeginAnimation()
    {
        animating = true;
        current_anim_time = 0;
        renderer.enabled = true;
    }
    public void StopAnimation()
    {
        animating = false;
        current_anim_time = 0;
        renderer.enabled = false;
    }

    public float GetCurrentAnimationTime()
    {
        return current_anim_time;
    }
    public float GetDuration()
    {
        return duration;
    }
    public bool IsAnimating()
    {
        return animating;
    }
}
