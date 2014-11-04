using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    // references
    public OnTrackMovement on_track_movement;
    public RollerBladeMovement roller_blade_movement;
    public OnTrackMouseAim on_track_mouse_aim;

    public AttackInfoHub attack_infohub;
    public CharAimInfoHub aim_info_hub;
    

    private bool break_turned = false;

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
