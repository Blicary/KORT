using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]

public class ExitDoor : MonoBehaviour
{
    public bool start_open = false;
    private bool open;
    private SpriteRenderer sprite_renderer;


    public void Start()
    {
        open = start_open;

        // TEMP
        sprite_renderer = GetComponent<SpriteRenderer>();
        if (open) sprite_renderer.color = Color.black;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Combatant c = collision.collider.GetComponent<Combatant>();
        if (c != null && c.player_controlled)
        {
            if (open) GameManager.ClearArena();
        }
    }

    public void Open()
    {
        open = true;
        sprite_renderer.color = Color.black;
    }
    public void Close()
    {
        open = false;
    }

    public bool IsOpen()
    {
        return open;
    }
}
