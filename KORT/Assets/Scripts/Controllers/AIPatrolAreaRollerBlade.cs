using UnityEngine;
using System.Collections;

public class AIPatrolAreaRollerBlade : AIPatrolAreaBase
{
    // references
    public RollerBladeMovement movement;
    public CharAimInfoHub aim;
    
    private float stop_dist = 3;
    private float rotation_allowance = 0;

    


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
            if (movement.GetVelocity().magnitude < 0.1f)
            {
                movement.Break(false);
                StopMovement();
            }
        }
        else
        {
            // turn

            Vector2 v = to_dest.normalized;
            float direction_rotation = Mathf.Atan2(v.y, v.x);
            float rot_diff = Mathf.Clamp(aim.GetAimRotation(), -Mathf.PI*2f, Mathf.PI*2f) - Mathf.Clamp(direction_rotation, -Mathf.PI*2f, Mathf.PI*2f);
            //Debug.Log(rot_diff);

            if (rot_diff > rotation_allowance)
                movement.Turn(1);
            else if (rot_diff < rotation_allowance)
                movement.Turn(-1);
            else
                movement.Turn(0);


            // move
            movement.MoveForward(true);
        }
    }

}
