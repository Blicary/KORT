using UnityEngine;
using System.Collections;


[RequireComponent(typeof(RollerBladeMovement))]
[RequireComponent(typeof(CharAimInfoHub))]

public class AICloseAttackRollerBlade : AICloseAttackBase 
{
    // references
    public RollerBladeMovement movement;
    public CharAimInfoHub aim;
    
    private float turn_accuracy = 0.4f; // 0 is most accurate


    public new void Awake()
    {
        base.Awake();

        movement = GetComponent<RollerBladeMovement>();
        aim = GetComponent<CharAimInfoHub>();
    }

    protected override void UpdateMovement()
    {
        if (target == null) return;

        // chase the target
        Vector2 to_target = target.transform.position - transform.position;

        // turn
        Vector2 v = to_target.normalized;
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
