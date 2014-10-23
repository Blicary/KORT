using UnityEngine;
using System.Collections;
using System;


public class RollerBladeMovement : ActionScript
{
    // references
    public CharMoveInfoHub move_infohub;
    public CharAimInfoHub aim_infohub;
    public Transform graphics_object;


    // movement
    public float speed = 20f;

    public float rotate_speed = 5f;
    private float rotation = 0;

    private float drag = 0.5f;
    private float break_drag = 6.5f; // amount of drag when breaking
    // the max speed at which a break turn can happen
    private float break_turn_speed_threshold = 8f;

    public Vector2 direction = new Vector2();
    private Vector2 velocity_last;


    // input
    private float input_turn = 0;
    private bool input_fwrd = false;
    private bool input_break = false;




    // PUBLIC MODIFIERS

    public void Start()
    {
        if (!graphics_object) Debug.LogWarning("No graphics object specified");

        // events
        aim_infohub.event_set_aim_direction += new EventHandler<EventArgs<Vector2>>(SetAimDirection);
        aim_infohub.event_set_aim_rotation += new EventHandler<EventArgs<float>>(SetAimRotation);
    }

    public void Update()
    {
        if (!has_control) return;


        // rotation
        rotation -= input_turn * rotate_speed * Time.deltaTime;
        graphics_object.localEulerAngles = new Vector3(0, 0, (rotation * Mathf.Rad2Deg) - 90);
        direction = new Vector2(Mathf.Cos(rotation), Mathf.Sin(rotation));

        // inform infohub
        aim_infohub.InformAimDirection(direction);
        aim_infohub.InformAimRotation(rotation);

        // reset input variables
        input_turn = 0;
    }
    public void FixedUpdate()
    {
        if (!has_control) return;


        velocity_last = rigidbody2D.velocity;


        if (input_break)
        {
            // breaking drag
            rigidbody2D.velocity /= 1 + break_drag * Time.deltaTime;
        }
        else
        {
            // faster than max roller blade speed, or not trying to move forward
            if (!input_fwrd || rigidbody2D.velocity.magnitude > speed)
            {
                // drag
                rigidbody2D.velocity /= 1 + drag * Time.deltaTime;
                rigidbody2D.velocity = direction * rigidbody2D.velocity.magnitude;
            }

            // trying to move forward
            else if (input_fwrd)
            {
                rigidbody2D.velocity = direction * Mathf.Max(speed, rigidbody2D.velocity.magnitude);
            }
        }


        // inform infohub
        move_infohub.InformVelocity(rigidbody2D.velocity);
        move_infohub.InformVelocityLastFrame(velocity_last);

        // reset input variables
        input_fwrd = false;
        input_break = false;
    }

    public void OnTrackAttach()
    {
        has_control = false;
        rigidbody2D.velocity = Vector2.zero;
    }
    public void OnTrackDetach()
    {
        has_control = true;
        rigidbody2D.velocity = move_infohub.GetVelocity();

        // set rotation based on movement direction coming off a track
        rotation = Mathf.Atan2(rigidbody2D.velocity.y, rigidbody2D.velocity.x);
    }

    // input
    public void MoveForward()
    {
        input_fwrd = true;
    }
    public void Break()
    {
        input_break = true;
    }
    public bool BreakTurn()
    {
        // only turn once and when moving slow enough
        if (rigidbody2D.velocity.magnitude <= break_turn_speed_threshold)
        {
            rotation += Mathf.PI;
            graphics_object.rotation = Quaternion.Euler(0, 0, (rotation * Mathf.Rad2Deg) - 90);
            return true;
        }
        return false;
    }
    public void Turn(float direction)
    {
        input_turn = direction == 0 ? 0 : direction > 0 ? 1 : -1;
    }

    // setters
    public void SetAimRotation(float rotation)
    {
        this.rotation = rotation;
        direction = new Vector2(Mathf.Cos(rotation), Mathf.Sin(rotation));
    }
    public void SetAimRotation(object sender, EventArgs<float> e)
    {
        SetAimRotation(e.Value);
    }
    public void SetAimDirection(Vector2 direction)
    {
        this.direction = direction;
        rotation = Mathf.Atan2(direction.y, direction.x);
    }
    public void SetAimDirection(object sender, EventArgs<Vector2> e)
    {
        SetAimDirection(e.Value);
    }


    // PUBLIC ACCESSORS

}
