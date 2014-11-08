using UnityEngine;
using System.Collections;


[RequireComponent(typeof(OnTrackMovement))]
[RequireComponent(typeof(RollerBladeMovement))]
[RequireComponent(typeof(OnTrackMouseAim))]
[RequireComponent(typeof(CharAimInfoHub))]

public class PlayerController : MonoBehaviour
{
    // references
    private OnTrackMovement on_track_movement;
    private RollerBladeMovement roller_blade_movement;
    private OnTrackMouseAim on_track_mouse_aim;

    private AttackInfoHub attack_infohub;
    private CharAimInfoHub aim_info_hub;
    

    private bool break_turned = false;


    public void Awake()
    {
        on_track_movement = GetComponent<OnTrackMovement>();
        roller_blade_movement = GetComponent<RollerBladeMovement>();
        on_track_mouse_aim = GetComponent<OnTrackMouseAim>();
        attack_infohub = GetComponent<AttackInfoHub>();
        aim_info_hub = GetComponent<CharAimInfoHub>();
    }

    public void Update()
    {
        float input_turn = Input.GetAxis("Horizontal");
        bool input_fwrd = Input.GetButton("Forward");
        bool input_break = Input.GetButton("Break");
        bool input_break_up = Input.GetButtonUp("Break");
        bool input_action = Input.GetButtonDown("Action");
        bool input_fire = Input.GetButtonDown("Fire");
        bool input_switch = Input.GetButtonUp("SwitchWeapon");



        // on track
        if (on_track_movement.IsOnTrack())
        {
            // track detach
            if (input_action)
                on_track_movement.DetachManually(aim_info_hub.GetAimDirection());
        }

        // rollerblading
        else
        {
            roller_blade_movement.Break(false);
            roller_blade_movement.MoveForward(false);

            // turn
            roller_blade_movement.Turn(input_turn);


            // forward, break, break turn
            if (input_break)
            {
                roller_blade_movement.Break(true);
                if (!break_turned && input_fwrd)
                {
                    break_turned = roller_blade_movement.BreakTurn();
                }


            }
            else if (input_fwrd)
            {
                roller_blade_movement.MoveForward(true);
            }

            if (input_break_up) break_turned = false;
        }

        


        // Attack actions
        {
            if (input_fire)
            {
                attack_infohub.Attack();
            }
            else if (input_switch)
            {
                attack_infohub.SwitchWeapon();
            }
            

        }


    }

    public void OnTrackAttach()
    {
        roller_blade_movement.enabled = false;
        on_track_movement.enabled = true;
        on_track_mouse_aim.enabled = true;
    }
    public void OnTrackDetach()
    {
        roller_blade_movement.enabled = true;
        on_track_movement.enabled = false;
        on_track_mouse_aim.enabled = false;
    }

}
