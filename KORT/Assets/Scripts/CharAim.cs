using UnityEngine;
using System.Collections;

public class CharAim : MonoBehaviour
{
    public CharAim alternate;
    public bool start_with_control = true;
    private bool has_control = true;



    // CORE

    public void Start()
    {
        has_control = start_with_control;
    }
    /// <summary>
    /// Find an alternate action script that has control.
    /// Will log a warning and give control to this script if on alternate with control found.
    /// </summary>
    /// <returns></returns>
    protected CharAim GetAlternate()
    {
        CharAim a = alternate;

        while (a != null && !a.has_control)
        {
            a = a.alternate;
            if (a == this)
            {
                Debug.LogWarning("No alternate CharAim with control (giving control to " + this.name + ")");
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

    public virtual Vector2 GetAimDirection()
    {
        return Vector2.zero;
    }
    public virtual float GetAimRotation()
    {
        return 0;
    }


    // PUBLIC MODIFIERS

    public virtual void SetAimDirection(Vector2 direction) { }
    public virtual void SetAimRotation(float rotation) { }

}
