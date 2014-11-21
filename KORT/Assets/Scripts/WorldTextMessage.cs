using UnityEngine;
using System.Collections;



public class WorldTextMessage : MonoBehaviour 
{
    public float fade_time = 1;
    public TextMesh text_mesh;
    private float timer;

    public void Start()
    {
        timer = fade_time;
    }

    public void Update()
    {
        timer -= Time.deltaTime;

        text_mesh.color = new Color(text_mesh.color.r, text_mesh.color.g,
            text_mesh.color.b, timer / fade_time);

        if (timer <= 0)
            GameObject.Destroy(this);
    }
}
