using UnityEngine;
using System.Collections;

public abstract class AIPatrolAreaBase : MonoBehaviour 
{
    public CircleCollider2D area;

    // input speed
    private float update_input_timer_max = 0.1f; // USE COREROUTINE INSTEAD...
    private float update_input_timer = 0;

    // waiting and moving
    public float wait_timer_max = 3;
    private float wait_timer = 0;

    // current movemment path
    protected Vector2 destination;


    public void Start()
    {
        wait_timer = Random.Range(wait_timer_max / 2f, wait_timer_max);
    }

    public void Update()
    {
        Debug.DrawLine(transform.position, destination);

        // input speed
        if (update_input_timer <= 0) update_input_timer = update_input_timer_max;
        else update_input_timer -= Time.deltaTime;


        // waiting
        if (wait_timer > 0)
        {
            UpdateWhileWaiting();
            wait_timer -= Time.deltaTime;
            if (wait_timer <= 0) StartMovement();
        }

        // moving
        else if (update_input_timer <= 0)
        {
            UpdateMovement();
        }
    }

    
    protected virtual void StartMovement()
    {
        //Debug.Log("Start Movement");
        
        float angle = Random.Range(0, Mathf.PI * 2f);
        destination = (Vector2)area.transform.position +
            new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * area.radius;
        
    }
    protected virtual void UpdateMovement()
    {

    }
    protected virtual void StopMovement()
    {
        //Debug.Log("Stop Movement");
        wait_timer = wait_timer_max;
    }

    protected virtual void UpdateWhileWaiting()
    {

    }
}
