using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class AIAgro : MonoBehaviour 
{
    // refereces
    protected List<Character> potential_targets; // list of targetable characters in awareness range
    protected Character target;


    public void Start()
    {
        potential_targets = new List<Character>();
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        Character c = collider.GetComponent<Character>();
        
        if (c)
        {
            Debug.Log("Add " + c.name + "as potential target.");
            potential_targets.Add(c);
            ChooseTarget();
        }
    }
    public void OnTriggerExit2D(Collider2D collider)
    {
        Character c = collider.GetComponent<Character>();
        if (c)
        {
            potential_targets.Remove(c);

            // must choose a new target if we removed the current one
            if (target == c) ChooseTarget(); 
        }
    }

    /// <summary>
    /// Select the best target from potential targets.
    /// </summary>
    protected virtual void ChooseTarget()
    {
        if (potential_targets.Count == 0)
        {
            target = null;
            Debug.Log("No target");
            return;
        }
        target = potential_targets[potential_targets.Count - 1];

        Debug.Log("Choose target: " + target.name);
    }

    public Character GetTarget()
    {
        return target;
    }
    public bool HasTarget()
    {
        return target != null;
    }

}
