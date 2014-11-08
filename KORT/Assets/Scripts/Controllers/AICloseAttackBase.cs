using UnityEngine;
using System.Collections;


[RequireComponent(typeof(AttackInfoHub))]

public class AICloseAttackBase : MonoBehaviour
{
    // references
    private AttackInfoHub attack;

    // waiting and moving
    public float input_reaction_time = 0.1f; // time between update movement calls
    protected Character target;

    public void Awake()
    {
        attack = GetComponent<AttackInfoHub>();
    }
    public void OnDisable()
    {
        target = null;
        StopCoroutine("InfrequentUpdate");
    }

    public void Update()
    {
        //Debug.DrawLine(transform.position, target.transform.position, Color.red);
    }

    public void SetTarget(Character target)
    {
        this.target = target;
        StartCoroutine("InfrequentUpdate");
    }
    
    private IEnumerator InfrequentUpdate()
    {
        while (true)
        {
            UpdateMovement();
            UpdateAttack();
            yield return new WaitForSeconds(input_reaction_time);
        }
    }
    protected virtual void UpdateMovement()
    {

    }
    protected virtual void UpdateAttack()
    {
        //Debug.Log("bot attack!");
        attack.Attack();
    }

}
