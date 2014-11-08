using UnityEngine;
using System.Collections;

public class ExitDoor : MonoBehaviour
{
    public bool open = false;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Character c = collision.collider.GetComponent<Character>();
        if (c != null && c.player_controlled)
        {
            if (open) GameManager.ClearArena();
        }
    }
}
