using UnityEngine;
using System.Collections;

public class AIController1 : MonoBehaviour 
{
    public Behaviour patrol_script, close_attack_script, ranged_attack_script;


    // 0 - patrol
    // 1 - close attack
    // 2 - ranged attack
    private int state = 0;

    public LayerMask targets_layer;
    public float agro_radius, de_agro_radius;
    private Character target;


    public void Start()
    {
        state = 0;

        close_attack_script.enabled = false;
        ranged_attack_script.enabled = false;
        patrol_script.enabled = true;
    }

    public void Update()
    {
        if (state == 0)
        {
            if (FindTarget())
            {
                state = 1;
                patrol_script.enabled = false;
                close_attack_script.enabled = true;
            }
        }
        else if (state == 1)
        {
            float dist = (target.transform.position - transform.position).magnitude;
            if (dist > de_agro_radius)
            {
                target = null;
                Debug.Log("Target released");
                state = 0;
                close_attack_script.enabled = false;
                patrol_script.enabled = true;
            }
        }
        else if (state == 2)
        {

        }
    }
    private bool FindTarget()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, agro_radius, targets_layer);
        foreach (Collider2D col in cols)
        {
            Character c = col.GetComponent<Character>();
            if (c)
            {
                target = c;
                Debug.Log("Found target: " + target.name);
                return true;
            }
        }

        return false;
    }
}
