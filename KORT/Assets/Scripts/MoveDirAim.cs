using UnityEngine;
using System.Collections;

public class MoveDirAim : CharAim 
{
    public CharMovement movement;


    // PUBLIC ACCESSORS

    public override Vector2 GetAimDirection()
    {
        if (!HasControl()) return GetAlternate().GetAimDirection();
        Debug.Log(movement.GetVelocity().normalized);
        return movement.GetVelocity().normalized;
    }
    public override float GetAimRotation()
    {
        if (!HasControl()) return GetAlternate().GetAimRotation();

        Vector2 aim_dir = movement.GetVelocity().normalized;
        return Vector2.Angle(Vector2.right, aim_dir);
    }


    // PUBLIC MODIFIERS

    public override void SetAimDirection(Vector2 direction) 
    {
        if (!HasControl()) GetAlternate().SetAimDirection(direction);
    }
    public override void SetAimRotation(float rotation)
    {
        if (!HasControl()) GetAlternate().SetAimRotation(rotation);
    }


    public void OnTrackDetach()
    {
        GainControl();
    }
}
