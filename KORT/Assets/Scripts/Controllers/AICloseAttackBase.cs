using UnityEngine;
using System.Collections;

public class AICloseAttackBase : MonoBehaviour
{
    // waiting and moving
    public float reaction_time = 0.1f; // time between update movement calls
    protected Character target;



    public void SetTarget(Character target)
    {
        this.target = target;
    }


    public void Update()
    {
        Debug.DrawLine(transform.position, target.transform.position, Color.red);
    }

    public void OnEnable()
    {
        StartCoroutine("UpdateMovementRoutine");
    }
    public void OnDisable()
    {
        target = null;
        StopCoroutine("UpdateMovementRoutine");
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

}
