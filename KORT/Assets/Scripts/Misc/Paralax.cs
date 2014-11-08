using UnityEngine;
using System.Collections;

public class Paralax : MonoBehaviour 
{
    private Vector3 original_pos;
    public float amount = 0.3f;


    public void Start()
    {
        original_pos = transform.position;
    }

    public void Update()
    {
        transform.position = original_pos - (Camera.main.transform.position - original_pos) * amount;
    }
}
