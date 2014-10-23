using UnityEngine;
using System.Collections;

public class CharMovement : MonoBehaviour
{
    public CharMovement alternate;
    public bool start_with_control = true;
    private bool has_control = true;
     


    // CORE

    public void Start()
    {
        has_control = start_with_control;
    }
    /// <summary>
    /// Find an alternate action script that has control.
    /// Will log a warning and return this if no alternate script with control found.
    /// </summary>
    /// <returns></returns>
    protected CharMovement GetAlternate()
    {
        CharMovement a = alternate;

        while (a != null && !a.has_control)
        {
            a = a.alternate;
            if (a == this)
            {
                Debug.LogWarning("No alternate CharMovement with control (giving control to " + this.name + ")");
                has_control = true;
                return this;
            }
        }
        return a;
    }
    public bool HasControl() { return has_control; }

    protected void GainControl()
    {
        GetAlternate().has_control = false;
        has_control = true;
    }
    
    
    // PUBLIC ACCESSORS

    public virtual Vector2 GetVelocity()
    {
        return Vector2.zero;
    }
    public virtual Vector2 GetVelocityLastFrame()
    {
        return Vector2.zero;
    }


    // PUBLIC MODIFIERS

}
