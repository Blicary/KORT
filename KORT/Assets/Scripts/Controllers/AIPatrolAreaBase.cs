using UnityEngine;
using System.Collections;

public abstract class AIPatrolAreaBase : MonoBehaviour 
{
    public CircleCollider2D area;

    

    // waiting and moving
    public float reaction_time = 0.1f; // time between update movement calls
    public float wait_time = 1;
    private float wait_timer = 0;

    // current movemment path
    protected Vector2 destination;


    public void Start()
    {
        wait_timer = Random.Range(wait_time / 2f, wait_time);
    }

    public void Update()
    {
        Debug.DrawLine(transform.position, destination);


        // waiting
        if (wait_timer > 0)
        {
            Debug.Log(wait_timer);
            UpdateWhileWaiting();
            wait_timer -= Time.deltaTime;
            if (wait_timer <= 0) StartMovement();
        }
    }

    
    protected virtual void StartMovement()
    {
        //Debug.Log("Start Movement");

        StartCoroutine("UpdateMovementRoutine");

        float angle = Random.Range(0, Mathf.PI * 2f);
        destination = (Vector2)area.transform.position +
            new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * area.radius;
        
    }
    private IEnumerator UpdateMovementRoutine()
    {
        while (true)
        {
            UpdateMovement();
            yield return new WaitForSeconds(reaction_time);
        }
    }
    protected virtual void UpdateMovement()
    {

    }
    protected virtual void StopMovement()
    {
        //Debug.Log("Stop Movement");
        StopCoroutine("UpdateMovementRoutine");
        wait_timer = wait_time;
    }

    protected virtual void UpdateWhileWaiting()
    {

    }
}
