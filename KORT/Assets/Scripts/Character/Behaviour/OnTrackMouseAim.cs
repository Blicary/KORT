using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Character))]
[RequireComponent(typeof(CharAimInfoHub))]
[RequireComponent(typeof(Animator))]

public class OnTrackMouseAim : MonoBehaviour 
{
    // references
    private Character character;
    private CharAimInfoHub aim_infohub;
    
    public Transform graphics_object;
    private Animator animator;


    // general
    private float aim_rotation = 0.0f; // radians


    // PUBLIC MODIFIERS

    public void Awake()
    {
        // get references
        character = GetComponent<Character>();
        aim_infohub = GetComponent<CharAimInfoHub>();
        animator = GetComponent<Animator>();

        if (!animator) Debug.LogWarning("No Animator component on graphics object");
    }

    public void Update()
    {
        if (character.IsStunned() || !character.IsAlive()) return;

        Vector2 mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        aim_rotation = GeneralHelpers.AngleBetweenVectors(transform.position, mouse_pos);

        // animation
        //graphics_object.localEulerAngles = new Vector3(0, 0, Mathf.Rad2Deg * aim_rotation - 90);
        UpdateAnimationDirection();


        // inform infohub
        aim_infohub.InformAimRotation(aim_rotation);
        aim_infohub.InformAimDirection(new Vector2(Mathf.Cos(aim_rotation), Mathf.Sin(aim_rotation)));
    }

    public void OnEnable()
    {
    }
    public void OnDisable()
    {
    }


    // PRIVATE MODIFERES

    private void UpdateAnimationDirection()
    {
        if (!animator) return;
        if (aim_rotation > 0 && aim_rotation < Mathf.PI)
        {
            //animator.SetInteger("State", 1);
        }
        else
        {
            //animator.SetInteger("State", 2);
        }
    }


    // HELPERS

    /// <summary>
    /// Calculates the angle in degrees between p1 and p2
    /// </summary>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    /// <returns></returns>


}
