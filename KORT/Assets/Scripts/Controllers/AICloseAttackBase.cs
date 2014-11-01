using UnityEngine;
using System.Collections;

public class AICloseAttackBase : MonoBehaviour
{
    // references
    public AttackInfoHub attack;

    // waiting and moving
    public float reaction_time = 0.1f; // time between update movement calls
    protected Character target;



    public void SetTarget(Character target)
    {
        this.target = target;
    }


    public void Update()
    {
        //Debug.DrawLine(transform.position, target.transform.position, Color.red);
    }

    public void OnEnable()
    {
        StartCoroutine("InfrequentUpdate");
    }
    public void OnDisable()
    {
        target = null;
        StopCoroutine("InfrequentUpdate");
    }

    private IEnumerator InfrequentUpdate()
    {
        while (true)
        {
            UpdateMovement();
            UpdateAttack();
            yield return new WaitForSeconds(reaction_time);
        }
    }
    protected virtual void UpdateMovement()
    {

    }
    protected virtual void UpdateAttack()
    {
        Debug.Log("bot attack!");
        attack.Attack();
    }

}
