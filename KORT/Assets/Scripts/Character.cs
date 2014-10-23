using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour
{
    public CharAimInfoHub aim;

    // general
    private bool alive = true;


    public void Update()
    {
        // TESTING
        Debug.DrawRay(transform.position, aim.GetAimDirection(), Color.white, 1);
    }

    // PUBLIC ACCESSORS 
    
    public bool IsAlive()
    {
        return alive;
    }

}
