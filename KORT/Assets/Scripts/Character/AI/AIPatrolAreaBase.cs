using UnityEngine;
using System.Collections;

public abstract class AIPatrolAreaBase : MonoBehaviour 
{
    // circles around which to pick patrol destinations
    // if there are multiple areas, will patrol from one to the next in order
    public CircleCollider2D[] areas; 
    private int current_area = 0;

    // waiting and moving

    public float input_reaction_time = 0.1f; // time between update movement calls
    public float wait_time = 1;
    private float wait_timer = 0;

    // current movemment path
    protected Vector2 destination;


    public void Start()
    {
        // check that areas are specified
        if (areas.Length == 0)
        {
            Debug.LogError("Missing patrol zone references.");
        }
        foreach (CircleCollider2D cc in areas)
        {
            if (cc == null)
            {
                Debug.LogError("Missing patrol zone references.");
                return;
            }
        }
    }


    public void OnEnable()
    {
        wait_timer = Random.Range(wait_time / 2f, wait_time);
    }
    public void OnDisable()
    {
        StopCoroutine("UpdateMovementRoutine");
    }

    public void Update()
    {
        //Debug.DrawLine(transform.position, destination);


        // waiting
        if (wait_timer > 0)
        {
            UpdateWhileWaiting();
            wait_timer -= Time.deltaTime;
            if (wait_timer <= 0) StartMovement();
        }
    }

    protected virtual void StartMovement()
    {
        //Debug.Log("Start Movement");

        // pick destination
        current_area = (current_area + 1) % areas.Length;

        float angle = Random.Range(0, Mathf.PI * 2f);
        destination = (Vector2)areas[current_area].transform.position +
            new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * areas[current_area].radius;


        // start
        StartCoroutine("UpdateMovementRoutine");
    }
    private IEnumerator UpdateMovementRoutine()
    {
        while (true)
        {
            UpdateMovement();
            yield return new WaitForSeconds(input_reaction_time);
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
