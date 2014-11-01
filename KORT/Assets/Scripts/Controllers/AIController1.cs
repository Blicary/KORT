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
        close_attack_script.enabled = false;
        ranged_attack_script.enabled = false;
        StartPatrol();
    }

    public void Update()
    {
        if (state == 0)
        {
            
        }
        else if (state == 1)
        {
            float dist = (target.transform.position - transform.position).magnitude;
            if (dist > de_agro_radius)
            {
                //Debug.Log("Target released");

                target = null;

                close_attack_script.enabled = false;
                StartPatrol();
            }
        }
        else if (state == 2)
        {

        }
    }

    private void StartPatrol()
    {
        state = 0;
        patrol_script.enabled = true;
        StartCoroutine("UpdateTargetSearch");
    }
    private void StopPatrol()
    {
        StopCoroutine("UpdateTargetSearch");
        patrol_script.enabled = false;
    }
    private void StartCloseAttack()
    {
        state = 1;
        close_attack_script.enabled = true;
    }


    private IEnumerator UpdateTargetSearch()
    {
        while (true)
        {
            if (FindTarget())
            {
                StopPatrol();
                StartCloseAttack();
            }
            yield return new WaitForSeconds(0.5f);
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
