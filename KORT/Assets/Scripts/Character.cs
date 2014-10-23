using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour
{
    public CharAimInfoHub aim;
    public CharMoveInfoHub move;


    // general
    private bool alive = true;


    public void Update()
    {
        // TESTING
        //Debug.DrawRay(transform.position, aim.GetAimDirection(), Color.white, 1);
        //move.SetVelocity(Vector2.zero);
        if (Input.GetKeyDown(KeyCode.T))
        {
            aim.SetAimDirection(Vector2.up);
        }
    }

    // PUBLIC ACCESSORS 
    
    public bool IsAlive()
    {
        return alive;
    }

}
