using UnityEngine;
using System.Collections;

public class AIPatrolAreaRollerBlade : AIPatrolAreaBase
{
    // references
    public RollerBladeMovement movement;
    public CharAimInfoHub aim;
    
    private float stop_dist = 3;
    private float turn_accuracy = 0.4f; // 0 is most accurate
    


    protected override void UpdateMovement()
    {
        Vector2 to_dest = destination - (Vector2)transform.position;
        float dist_to_dest = to_dest.magnitude;

        //Debug.Log("dist to centre: " + dist_to_center + stop_dist);
        //Debug.Log("area r: " + area.radius);

        if (dist_to_dest <= stop_dist)
        {
            movement.MoveForward(false);
            movement.Turn(0);
            movement.Break(true);
            if (movement.GetVelocity().magnitude < 1f)
            {
                movement.Break(false);
                StopMovement();
            }
        }
        else
        {
            // turn
            Vector2 v = to_dest.normalized;
            float target_rotation = GeneralHelpers.PosifyRotation(Mathf.Atan2(v.y, v.x) % (2f * Mathf.PI));
            float actual_rotation = GeneralHelpers.PosifyRotation(aim.GetAimRotation() % (2f * Mathf.PI));

            float rot_diff = actual_rotation - target_rotation;

            int turn_dir = rot_diff > turn_accuracy ? 1 : rot_diff < -turn_accuracy ? -1 : 0;
            if (Mathf.Abs(rot_diff) > Mathf.PI) turn_dir *= -1;

            movement.Turn(turn_dir);


            // move
            movement.Break(false);
            movement.MoveForward(true);
        }
    }

}
