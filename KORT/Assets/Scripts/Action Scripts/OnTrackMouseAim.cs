using UnityEngine;
using System.Collections;

public class OnTrackMouseAim : MonoBehaviour 
{
    // references
    public Character character;
    public CharAimInfoHub aim_infohub;
    public OnTrackMovement on_track_movement;
    public Transform graphics_object;

    // general
    private float aim_rotation = 0.0f; // radians


    // PUBLIC MODIFIERS

    public void Update()
    {
        if (character.IsStunned() || !character.IsAlive()) return;

        Vector2 mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        aim_rotation = GeneralHelpers.AngleBetweenVectors(transform.position, mouse_pos);

        graphics_object.localEulerAngles = new Vector3(0, 0, Mathf.Rad2Deg * aim_rotation - 90);


        // inform infohub
        aim_infohub.InformAimRotation(aim_rotation);
        aim_infohub.InformAimDirection(new Vector2(Mathf.Cos(aim_rotation), Mathf.Sin(aim_rotation)));
    }



    // PUBLIC MODIFERES

    public void OnEnable()
    {
    }
    public void OnDisable()
    {
    }


    // HELPERS

    /// <summary>
    /// Calculates the angle in degrees between p1 and p2
    /// </summary>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    /// <returns></returns>


}
