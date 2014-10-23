using UnityEngine;
using System.Collections;

public class OnTrackMouseAim : CharAim 
{
    // references
    public OnTrackMovement on_track_movement;
    public Transform graphics_object;

    // general
    private float aim_rotation = 0.0f;


    // PUBLIC MODIFIERS

    public void Update()
    {
        if (!HasControl()) return;

        Vector2 mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        aim_rotation = AngleBetweenVectors(transform.position, mouse_pos);

        graphics_object.localEulerAngles = new Vector3(0, 0, aim_rotation - 90);
    }

    public override void SetAimDirection(Vector2 direction)
    {
        if (!HasControl()) GetAlternate().SetAimDirection(direction);
        aim_rotation = Vector2.Angle(Vector2.right, direction);
    }
    public override void SetAimRotation(float rotation)
    {
        if (!HasControl()) GetAlternate().SetAimRotation(rotation);
        aim_rotation = rotation;
    }


    // PUBLIC ACCESSORS
    public override Vector2 GetAimDirection()
    {
        if (!HasControl()) return GetAlternate().GetAimDirection();
        return new Vector2(Mathf.Cos(aim_rotation), Mathf.Sin(aim_rotation));
    }
    public override float GetAimRotation()
    {
        if (!HasControl()) return GetAlternate().GetAimRotation();
        return aim_rotation;
    }

    public void OnTrackAttach()
    {
        GainControl();
    }


    // HELPERS

    /// <summary>
    /// Calculates the angle in degrees between p1 and p2
    /// </summary>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    /// <returns></returns>
    private float AngleBetweenVectors(Vector2 p1, Vector2 p2)
    {
        float theta = Mathf.Rad2Deg * Mathf.Atan2(Mathf.Abs(p2.y - p1.y), Mathf.Abs(p2.x - p1.x));
        //Debug.Log("Theta:" + "(" + (p2.y - p1.y) + ") / (" + (p2.x - p1.x) + ")");
        if (p2.y > p1.y)
        {
            if (p2.x > p1.x)
            {
                return theta;
            }
            else
            {
                return 180 - theta;
            }
        }
        else
        {
            if (p2.x > p1.x)
            {
                return 360 - theta;
            }
            else
            {
                return 180 + theta;
            }
        }
    }


}
