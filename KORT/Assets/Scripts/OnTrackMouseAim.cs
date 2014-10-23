using UnityEngine;
using System.Collections;

public class OnTrackMouseAim : ActionScript 
{
    // references
    public CharAimInfoHub aim_infohub;
    public OnTrackMovement on_track_movement;
    public Transform graphics_object;

    // general
    private float aim_rotation = 0.0f;


    // PUBLIC MODIFIERS

    public void Update()
    {
        if (!has_control) return;

        Vector2 mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        aim_rotation = AngleBetweenVectors(transform.position, mouse_pos);

        graphics_object.localEulerAngles = new Vector3(0, 0, aim_rotation - 90);


        // inform infohub
        aim_infohub.InformAimRotation(aim_rotation);
        aim_infohub.InformAimDirection(new Vector2(Mathf.Cos(aim_rotation), Mathf.Sin(aim_rotation)));
    }



    // PUBLIC ACCESSORS

    public void OnTrackAttach()
    {
        has_control = true;
    }
    public void OnTrackDetach()
    {
        has_control = false;
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
