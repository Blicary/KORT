using UnityEngine;
using System.Collections;


[RequireComponent(typeof(CharAimInfoHub))]

public class RollerbladeTrail : MonoBehaviour
{
    private CharAimInfoHub aim;
    public ParticleSystem psystem;
    public float character_radius;
    
    public Color color_bloody;
    public Color color_normal;

    private const float fade_speed = 2f;

    private Color color;


    public void Start()
    {
        aim = GetComponent<CharAimInfoHub>();

        color = color_normal;
        psystem.startColor = color;
    }

    public void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.tag == "Blood")
        {
            if (collider.OverlapPoint((Vector2)transform.position - Vector2.up * character_radius))
                StartBloodTrail();
        }
    }

    public void Update()
    {
        if (!psystem.enableEmission) return;

        // rotation
        psystem.startRotation = -aim.GetAimRotation();


        // color
        float a = color.a;
        if (a - color_normal.a <= 0.1f)
        {
            color = Color.Lerp(color, color_normal, Time.deltaTime);
        }
        else
        {
            a = Mathf.Lerp(a, color_normal.a, Time.deltaTime * fade_speed);
            color = new Color(color.r, color.g, color.b, a);
        }

        psystem.startColor = color;
    }

    public void OnTrackAttach()
    {
        psystem.enableEmission = false;
    }
    public void OnTrackDettach()
    {
        psystem.enableEmission = true;
    }

    private void StartBloodTrail()
    {
        color = color_bloody;
    }

}
