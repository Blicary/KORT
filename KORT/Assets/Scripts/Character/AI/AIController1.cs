using UnityEngine;
using System.Collections;

public class AIController1 : MonoBehaviour 
{
    public Behaviour patrol_script, ranged_attack_script;
    public AICloseAttackBase close_attack_script;

    private enum State { Disabled, Patrol, CloseAttack, RangedAttack }
    private State state = State.Disabled;
    

    public LayerMask targets_layer;
    public float agro_radius, de_agro_radius;
    private Character target;


    public void Start()
    {
        close_attack_script.enabled = false;
        ranged_attack_script.enabled = false;
        ChangeState(State.Patrol);
    }
    public void OnDisable()
    {
        ChangeState(State.Disabled);
    }

    public void Update()
    {
        if (state == State.Patrol)
        {
            
        }
        else if (state == State.CloseAttack)
        {
            // target destroyed
            if (target == null)
            {
                ChangeState(State.Patrol);
            }
            else
            {
                // release target
                float dist = (target.transform.position - transform.position).magnitude;
                if (dist > de_agro_radius)
                {
                    target = null;

                    ChangeState(State.Patrol);
                }
            }
        }
        else if (state == State.RangedAttack)
        {

        }
    }

    private void ChangeState(State new_state)
    {
        if (state == State.Patrol)
            StopPatrol();
        else if (state == State.CloseAttack)
            StopCloseAttack();

        if (new_state == State.Patrol)
            StartPatrol();
        else if (new_state == State.CloseAttack)
            StartCloseAttack();


        this.state = new_state;
    }

    private void StartPatrol()
    {
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
        close_attack_script.SetTarget(target);
        close_attack_script.enabled = true;
    }
    private void StopCloseAttack()
    {
        close_attack_script.enabled = false;
    }


    private IEnumerator UpdateTargetSearch()
    {
        while (true)
        {
            if (FindTarget())
            {
                ChangeState(State.CloseAttack);
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
                //Debug.Log("Found target: " + target.name);
                return true;
            }
        }

        return false;
    }

}
