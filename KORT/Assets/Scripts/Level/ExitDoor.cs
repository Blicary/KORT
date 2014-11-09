using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]

public class ExitDoor : MonoBehaviour
{
    public bool start_open = false;
    private bool open;
    public bool open_to_all = false;
    private SpriteRenderer sprite_renderer;


    public void Start()
    {
        open = start_open;

        // TEMP
        sprite_renderer = GetComponent<SpriteRenderer>();
        if (open) sprite_renderer.color = Color.black;
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        Combatant c = collider.GetComponent<Combatant>();
        if (!c) return; // only let combatants through

        if (GameManager.Scenestate == SceneState.Arena)
        {
            // only let the combatant through if this is their door
            if (c != null && (this == HouseManager.GetHouseDoor(c.house_name)) || open_to_all)
            {
                if (c.player_controlled)
                {
                    if (open) GameManager.ClearArena();
                }
                else
                {
                    // ai exit
                }
            }
        }
        else
        {
            GameManager.ClearArena();
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
